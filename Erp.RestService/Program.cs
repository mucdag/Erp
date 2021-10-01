using Core.Settings;
using Core.WebSockets;
using Erp.Business.Infrastructure;
using Erp.Data.Contexts;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Core.Aspects.ApiFilters;

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

//builder.Services.LoadDependencyResolvers(new ICoreModule[] { new SharedModuleBinding(),
//                new WriteRepositoryModuleBinding(), new ReadRepositoryModuleBinding() ,new WriteManagerModuleBinding(), new ReadManagerModuleBinding()});

//builder.Services.AddDependencyResolvers(new ICoreModule[] { new CoreModule(), new BusinessInfrastructureCoreModule() });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Erp.RestService", Version = "v1" });
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

app.MapControllers();

app.Run();
