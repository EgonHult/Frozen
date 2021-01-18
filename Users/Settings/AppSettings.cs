namespace Users.Settings
{
    /// <summary>
    /// Constants representing values in appsettings.json
    /// </summary>
    public static class AppSettings
    {
        public const string JWT_KEY             = "JWT:Key";
        public const string JWT_REFRESHKEY      = "JWT:RefreshKey";
        public const string JWT_ISSUER          = "JWT:Issuer";
        public const string JWT_EXPIRE_MINUTES  = "JWT:ExpireMinutes";
        public const string JWT_EXPIRE_MONTHS   = "JWT:ExpireMonths";
    }
}
