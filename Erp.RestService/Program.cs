using Core.Settings;
using Core.WebSockets;
using Erp.Business.Infrastructure;
using Erp.Data.Contexts;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Core.Aspects.ApiFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Core.Utilities.IoC;
using Erp.Business.DependencyResolvers.DotNetCore.RepositoryModuleBinding;
using Erp.Business.DependencyResolvers.DotNetCore.ManagerModuleBinding;
using Erp.Core.Extensions;
using Erp.Business.DependencyResolvers.DotNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string policyName = "AllowOrigin";

// Add services to the container.
#region Services

builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);
builder.Services.AddCors(options =>
{
    options.AddPolicy(policyName, options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build());
});

ModelMappingProfile.ModelMappingInitialize();
AutoMapper.Mapper.AssertConfigurationIsValid();

builder.Services.AddTransient<WebSocketConnectionManager>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<ErpContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(TransactionFilter));
    options.Filters.Add(typeof(ExceptionLogFilter));
    options.Filters.Add(typeof(InfoLogFilter));
    options.Filters.Add(typeof(PerformanceLogFilter));
}).AddDataAnnotationsLocalization().AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

builder.Services.LoadDependencyResolvers(new ICoreModule[] { new SharedModuleBinding(), new WriteRepositoryModuleBinding(), new ReadRepositoryModuleBinding(), new WriteManagerModuleBinding(), new ReadManagerModuleBinding() });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Erp.RestService", Version = "v1" });
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["AppSettings.JwtIssuer"],
        ValidAudience = builder.Configuration["AppSettings.JwtAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings.JwtSecurityKey"]))
    };
});

#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Erp.RestService v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
