using Newtonsoft.Json;

namespace Remundo.Firebase.Api.Models
{
    public class LoginResponse(string email, string uid, string idToken, string refreshToken, bool emailVerified, string displayName, DateTime expiresAt)
    {
        public string Email { get; set; } = email;
        public string Uid { get; set; } = uid;
        public string IdToken { get; set; } = idToken;
        public string RefreshToken { get; set; } = refreshToken;
        public bool EmailVerified { get; set; } = emailVerified;
        public string DisplayName { get; set; } = displayName;
        public DateTime ExpiresAt { get; set; } = expiresAt;
    }
    public class FirebaseAuthResponse
    {
        [JsonProperty("idToken")]
        public string IdToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("expiresIn")]
        public long ExpiresIn { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("localId")]
        public string LocalId { get; set; }

        [JsonProperty("registered")]
        public bool Registered { get; set; }
    }

    public class FirebaseUserLookupResponse
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("users")]
        public List<FirebaseUserInfo> Users { get; set; }
    }

    public class FirebaseUserInfo
    {
        [JsonProperty("localId")]
        public string LocalId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("lastLoginAt")]
        public string LastLoginAt { get; set; }

        [JsonProperty("lastRefreshAt")]
        public string LastRefreshAt { get; set; }

        [JsonProperty("passwordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty("passwordUpdatedAt")]
        public long PasswordUpdatedAt { get; set; }

        [JsonProperty("validSince")]
        public long ValidSince { get; set; }
    }
}
