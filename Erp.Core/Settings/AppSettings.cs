namespace Core.Settings
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string FolderPath { get; set; }
        public string MongoDbName { get; set; }
        public string LogDbName { get; set; }
        public string EmailConfirmExpiryDate { get; set; }
        public string PasswordResetExpiryDate { get; set; }
        public string FailedLoginAttemptCount { get; set; }
        public string FailedLoginAttemptMinutes { get; set; }
        public string SecuredOperation { get; set; }
        public string LogoCon { get; set; }
        public PushNotificationsOptions PushNotificationsOptions { get; set; }
    }
    public class PushNotificationsOptions
    {
        public string PublicKey { get; set; }

        public string PrivateKey { get; set; }
    }
}
