namespace Erp.Data.Interfaces
{
    public interface IEntityBase<KeyType>
    {
        DateTime? UpdatingDate { get; set; }
        DateTime RegistrationDate { get; set; }
        int? UpdatedByUserId { get; set; }
        int? CreatedByUserId { get; set; }
    }
}
