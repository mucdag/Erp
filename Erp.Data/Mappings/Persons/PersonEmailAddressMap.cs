using Erp.Data.Models.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Data.Mappings.Persons
{
    public class PersonEmailAddressMap : MappingBase<PersonEmailAddress, int>, IEntityTypeConfiguration<PersonEmailAddress>
    {
        public void Configure(EntityTypeBuilder<PersonEmailAddress> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            builder.ToTable("PersonEmailAddresses", "Person");
            builder.Property(t => t.Id).IsRequired().HasColumnName("Id");
            builder.Property(t => t.PersonId).IsRequired().HasColumnName("PersonId");
            builder.Property(t => t.EmailAddress).HasMaxLength(100).IsRequired().HasColumnName("EmailAddress");

            builder.HasOne(t => t.Person)
                .WithMany(t => t.PersonEmailAddresses)
                .HasForeignKey(d => d.PersonId);
        }
    }
}