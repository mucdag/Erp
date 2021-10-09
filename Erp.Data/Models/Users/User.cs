using Erp.Data.Models.People;
using System;

namespace Erp.Data.Models.Users
{
    public class User : EntityBase<int>
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int PersonEmailAddressId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }

        #region PasswordReset
        public string PasswordResetCode { get; set; }
        public DateTime? PasswordResetExpiryDate { get; set; }
        public bool IsPasswordShouldBeReset { get; set; }
        #endregion

        public virtual Person Person { get; set; }
        public virtual PersonEmailAddress PersonEmailAddress { get; set; }
    }
}
