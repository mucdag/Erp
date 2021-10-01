using Erp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Data.Mappings
{
    public class MappingBase<T, KeyType> : IEntityTypeConfiguration<T> where T : EntityBase<KeyType>
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(t => t.RegistrationDate).HasColumnName("RegistrationDate");
            builder.Property(t => t.UpdatingDate).IsRequired(false).HasColumnName("UpdatingDate");
            builder.Property(t => t.CreatedByUserId).IsRequired(false).HasColumnName("CreatedByUserId");
            builder.Property(t => t.UpdatedByUserId).IsRequired(false).HasColumnName("UpdatedByUserId");

        }
    }
}