using Newtonsoft.Json;

namespace Remundo.Firebase.Api.Models
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenResponse
    {
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        public DateTime ExpiresAt => Helpers.GetTokenExpiry(IdToken);
    }
}
