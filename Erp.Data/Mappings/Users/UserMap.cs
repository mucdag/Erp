using Erp.Data.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Data.Mappings.Users
{
    public class UserMap : MappingBase<User, int>, IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            builder.ToTable("Users", "User");
            builder.Property(x => x.PersonId).IsRequired().HasColumnName("PersonId");
            builder.Property(x => x.PersonEmailAddressId).IsRequired().HasColumnName("PersonEmailAddressId");
            builder.Property(x => x.Password).HasColumnName("Password");
            builder.Property(x => x.IsActive).IsRequired().HasColumnName("IsActive");
            builder.Property(x => x.Description).IsRequired(false).HasColumnName("Description");

            builder.Property(x => x.PasswordResetExpiryDate).IsRequired(false).HasColumnName("PasswordResetExpiryDate");
            builder.Property(x => x.PasswordResetCode).IsRequired(false).HasColumnName("PasswordResetCode");
            builder.Property(x => x.IsPasswordShouldBeReset).IsRequired().HasColumnName("IsPasswordShouldBeReset");

            builder.HasOne(x => x.Person).WithMany(x => x.Users).HasForeignKey(x => x.PersonId);
            builder.HasOne(x => x.PersonEmailAddress).WithMany(x => x.Users).HasForeignKey(x => x.PersonEmailAddressId);

        }
    }
}