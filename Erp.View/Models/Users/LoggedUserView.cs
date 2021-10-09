namespace Erp.View.Models.Users
{
    public class LoggedUserView
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string AccessToken { get; set; }
    }
}
