using Newtonsoft.Json;
using System.Text;

namespace Remundo.Firebase.Api
{
    public static class Helpers
    {
        public static DateTime GetTokenExpiry(string idToken)
        {
            var parts = idToken.Split('.');
            if (parts.Length != 3) throw new FormatException("Invalid JWT format");

            var payloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(PadBase64(parts[1])));
            var payload = JsonConvert.DeserializeObject<JwtPayload>(payloadJson);

            return DateTimeOffset.FromUnixTimeSeconds(payload.Exp).UtcDateTime;
        }

        private static string PadBase64(string base64)
        {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }

        private class JwtPayload
        {
            [JsonProperty("exp")]
            public long Exp { get; set; }
        }
    }
}
