using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZTE;

namespace ZTE.Api
{
    /// <summary>
    /// Manages ubus session lifecycle
    /// TODO: Implement automatic session creation/login when the actual API is available
    /// </summary>
    public class SessionManager
    {
        private readonly UbusClient _client;
        private string _currentSession;

        public string CurrentSession => _currentSession;

        public SessionManager(UbusClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Initialize with a fixed session token (temporary solution)
        /// TODO: Replace with actual session.create or session.login API call
        /// </summary>
        public void InitializeWithFixedSession(string session)
        {
            _currentSession = session;
            _client.Session = session;
        }

        /// <summary>
        /// Create anonymous session (to be implemented)
        /// This should call something like: ubus call session create
        /// </summary>
        public async Task<string> CreateSessionAsync()
        {
            // TODO: Implement actual session creation
            // Example: await _client.CallAsync<SessionResponse>("session", "create", new {})
            throw new NotImplementedException("Session creation API not yet captured. Use InitializeWithFixedSession for now.");
        }

        /// <summary>
        /// Login with password using zwrt_web.web_login based on captured HAR flow.
        /// </summary>
        public async Task<string> LoginAsync(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.", nameof(password));

            var previousSession = _client.Session;
            _client.Session = AppSettings.UnauthenticatedSessionToken;

            try
            {
                var loginInfo = await _client.CallAsJsonAsync("zwrt_web", "web_login_info", new { });
                var salt = loginInfo.TryGetProperty("zte_web_sault", out var saltElement)
                    ? saltElement.GetString()
                    : string.Empty;

                var passwordHashCandidates = BuildPasswordHashCandidates(password, salt);
                string sessionToken = null;

                foreach (var passwordHash in passwordHashCandidates)
                {
                    var loginResponse = await _client.CallAsJsonAsync("zwrt_web", "web_login", new { password = passwordHash });
                    if (!loginResponse.TryGetProperty("result", out var resultElement) || resultElement.GetInt32() != 0)
                        continue;

                    if (!loginResponse.TryGetProperty("ubus_rpc_session", out var sessionElement))
                        throw new InvalidOperationException("Login failed: missing session token.");

                    sessionToken = sessionElement.GetString();
                    if (!string.IsNullOrWhiteSpace(sessionToken))
                        break;
                }

                if (string.IsNullOrWhiteSpace(sessionToken))
                    throw new InvalidOperationException("Login failed. Please verify the password.");

                _currentSession = sessionToken;
                _client.Session = sessionToken;
                AppSettings.SessionToken = sessionToken;
                return sessionToken;
            }
            catch
            {
                _client.Session = previousSession;
                throw;
            }
        }

        private static string ComputeSha256Hex(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
                builder.Append(b.ToString("X2"));
            return builder.ToString();
        }

        private static string[] BuildPasswordHashCandidates(string password, string salt)
        {
            if (string.IsNullOrEmpty(salt))
                return new[] { ComputeSha256Hex(password) };

            return new[]
            {
                ComputeSha256Hex(salt + password),
                ComputeSha256Hex(password + salt),
                ComputeSha256Hex(password)
            };
        }

        /// <summary>
        /// Check if current session is valid
        /// </summary>
        public bool HasValidSession()
        {
            return !string.IsNullOrEmpty(_currentSession);
        }

        /// <summary>
        /// Handle session expiration and refresh (to be implemented)
        /// </summary>
        public async Task RefreshSessionIfNeededAsync()
        {
            // TODO: Implement session refresh logic
            // Check if session is still valid, if not, create new one
            await Task.CompletedTask;
        }
    }
}
