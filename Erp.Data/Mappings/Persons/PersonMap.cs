using Erp.Data.Models.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Data.Mappings.Persons
{
    public class PersonMap : MappingBase<Person, int>, IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            builder.ToTable("Persons", "Person");
            builder.Property(t => t.Id).IsRequired().HasColumnName("Id");
            builder.Property(t => t.FullName).IsRequired().HasMaxLength(50).HasColumnName("FullName");
            builder.Property(t => t.Gender).IsRequired().HasColumnName("Gender");

        }
    }
}