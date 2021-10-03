using Erp.Data.Mappings.People;
using Erp.Data.Mappings.Users;
using Erp.Data.Models.People;
using Erp.Data.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Erp.Data.Contexts
{
    public class ErpContext : DbContext
    {
        public ErpContext(DbContextOptions<ErpContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        #region People
        public DbSet<Person> People { get; set; }
        public DbSet<PersonEmailAddress> PersonEmailAddresses { get; set; }
        #endregion

        #region Users
        public DbSet<User> Users { get; set; }

        #endregion


        public override int SaveChanges()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }

            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region People
            modelBuilder.ApplyConfiguration(new PersonMap());
            modelBuilder.ApplyConfiguration(new PersonEmailAddressMap());
            #endregion

            #region Users
            modelBuilder.ApplyConfiguration(new UserMap());
            #endregion
        }
    }
}
