namespace Core.Settings
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string FolderPath { get; set; }
        public int JwtExpiryinDays { get; set; }
        public string JwtSecurityKey { get; set; }
        public string JwtIssuer { get; set; }
        public int FailedLoginAttemptCount { get; set; }
        public int FailedLoginAttemptMinutes { get; set; }
        public bool SecuredOperation { get; set; }
        public PushNotificationsOptions PushNotificationsOptions { get; set; }
    }
    public class PushNotificationsOptions
    {
        public string PublicKey { get; set; }

        public string PrivateKey { get; set; }
    }
}
