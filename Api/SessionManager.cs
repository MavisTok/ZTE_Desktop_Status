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

            // 关键：login_info 与 login 都必须用 0000 session 调用
            _client.Session = "00000000000000000000000000000000";

            try
            {
                // 1) 获取 salt
                var loginInfo = await _client.CallAsJsonAsync("zwrt_web", "web_login_info", new { });

                if (!loginInfo.TryGetProperty("zte_web_sault", out var saltElement))
                    throw new InvalidOperationException("Cannot find zte_web_sault in web_login_info response.");

                var salt = saltElement.GetString();
                if (string.IsNullOrWhiteSpace(salt))
                    throw new InvalidOperationException("Salt is empty.");

                // 2) 计算哈希：SHA256( SHA256(password) + salt )
                var h1 = ComputeSha256HexUpper(password);      // 第一层
                var h2 = ComputeSha256HexUpper(h1 + salt);     // 第二层（最终发送）

                // 3) 调 web_login
                var loginResp = await _client.CallAsJsonAsync("zwrt_web", "web_login", new { password = h2 });

                if (!loginResp.TryGetProperty("result", out var resultElement) || resultElement.GetInt32() != 0)
                    throw new InvalidOperationException("Login failed. Please verify the password.");

                if (!loginResp.TryGetProperty("ubus_rpc_session", out var sessionElement))
                    throw new InvalidOperationException("Login failed: missing ubus_rpc_session.");

                var sessionToken = sessionElement.GetString();
                if (string.IsNullOrWhiteSpace(sessionToken))
                    throw new InvalidOperationException("Login failed: ubus_rpc_session is empty.");

                // 4) 切换为登录后的 session
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

        private static string ComputeSha256HexUpper(string input)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            var sb = new System.Text.StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
                sb.Append(b.ToString("X2")); // 大写 HEX
            return sb.ToString();
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