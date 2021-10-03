using Erp.Data.Interfaces;
using System;

namespace Erp.Data.Models
{
    public class EntityBase<KeyType> : IEntityBase<KeyType>
    {
        public int? UpdatedByUserId { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatingDate { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}