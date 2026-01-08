namespace ZTE
{
    /// <summary>
    /// Application configuration settings
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Ubus session token for unauthenticated mode (32 zeros).
        /// </summary>
        public const string UnauthenticatedSessionToken = "00000000000000000000000000000000";

        /// <summary>
        /// Router base URL (default: http://192.168.0.1)
        /// </summary>
        public static string RouterUrl { get; set; } = "http://192.168.0.1";

        /// <summary>
        /// Ubus session token for no-auth mode
        /// Use "00000000000000000000000000000000" (32 zeros) for unauthenticated access
        /// This allows reading basic router information without login
        /// </summary>
        public static string SessionToken { get; set; } = UnauthenticatedSessionToken;

        /// <summary>
        /// Dashboard refresh interval in seconds (default: 2 seconds)
        /// </summary>
        public static int RefreshIntervalSeconds { get; set; } = 2;

        /// <summary>
        /// HTTP request timeout in seconds (default: 3 seconds)
        /// </summary>
        public static int RequestTimeoutSeconds { get; set; } = 3;
    }
}
