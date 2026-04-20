namespace YoutubeClone.Shared.Constants
{
    public class ConfigurationConstants
    {
        // First app
        public const string FIRST_APP_TIME_USER_USERNAME = "FirstAppTime:User:UserName";
        public const string FIRST_APP_TIME_USER_DISPLAYNAME = "FirstAppTime:User:DisplayName";
        public const string FIRST_APP_TIME_USER_LOCATION = "FirstAppTime:User:Location";
        public const string FIRST_APP_TIME_USER_EMAIL = "FirstAppTime:User:Email";
        public const string FIRST_APP_TIME_USER_PASSWORD = "FirstAppTime:User:Password";

        // Connection strings
        public const string CONNECTION_STRING_DATABASE = "ConnectionStrings:Database";

        // JWT
        public const string JWT_PRIVATE_KEY = "Jwt:PrivateKey";
        public const string JWT_AUDIENCE = "Jwt:Audience";
        public const string JWT_ISSUER = "Jwt:Issuer";
        public const string JWT_EXPIRATION_IN_MINUTES_MIN = "Jwt:ExpirationInMinutesMin";
        public const string JWT_EXPIRATION_IN_MINUTES_MAX = "Jwt:ExpirationInMinutesMax";

        // AUTH
        public const string AUTH_REFRESH_TOKEN_EXPIRATION_IN_DAYS = "Auth:RefreshToken:ExpirationDays";

        // SMTP
        public const string SMTP_HOST = "SMTP:Host";
        public const string SMTP_FROM = "SMTP:From";
        public const string SMTP_PORT = "SMTP:Port";
        public const string SMTP_USER = "SMTP:User";
        public const string SMTP_PASSWORD = "SMTP:Password";
    }
}
