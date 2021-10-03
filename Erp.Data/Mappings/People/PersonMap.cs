using Erp.Data.Models.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Data.Mappings.People
{
    public class PersonMap : MappingBase<Person, int>, IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            builder.ToTable("People", "Person");
            builder.Property(t => t.Id).IsRequired().HasColumnName("Id");
            builder.Property(t => t.FullName).IsRequired().HasColumnName("FullName");
            builder.Property(t => t.Gender).IsRequired().HasColumnName("Gender");

        }
    }
}