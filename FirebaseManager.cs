using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using Remundo.Firebase.Api;
using Remundo.Firebase.Api.Models;
using System.Text;
using System.Text.RegularExpressions;

public class FirebaseManager
{
    private IDictionary<string, AbstractFirebaseAuth> TenantsAuthInstances = new Dictionary<string, AbstractFirebaseAuth>();
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string _baseApiUrl = "https://identitytoolkit.googleapis.com/v1/";

    public FirebaseManager(AppSettings settings, HttpClient httpClient)
    {
        CreateApp(settings.FirebaseConnection);
        _httpClient = httpClient;
        _apiKey = settings.FirebaseApiKey;
    }

    private AbstractFirebaseAuth GetConnection(string tenantId)
    {
        if (TenantsAuthInstances.ContainsKey(tenantId)) return TenantsAuthInstances[tenantId];

        var auth = FirebaseAuth.DefaultInstance.TenantManager.AuthForTenant(tenantId);
        TenantsAuthInstances.Add(tenantId, auth);
        return auth;
    }

    public async Task<LoginResponse?> SignInWithEmailAndPasswordAsync(LoginRequest req)
    {
        var endpoint = $"{_baseApiUrl}accounts:signInWithPassword?key={_apiKey}";

        var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
        var res = await _httpClient.PostAsync(endpoint, content);

        var msg = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            throw new Exception(msg);
        }

        var authRes = JsonConvert.DeserializeObject<FirebaseAuthResponse>(msg);
        var lookup = await LookupUserAsync(authRes.IdToken);
        var user = lookup.Users.First();

        var expiry = Helpers.GetTokenExpiry(authRes.IdToken);

        return new LoginResponse(
            user.Email,
            user.LocalId,
            authRes.IdToken,
            authRes.RefreshToken,
            user.EmailVerified,
            user.DisplayName,
            expiry
        );
    }

    public async Task<FirebaseUserLookupResponse> LookupUserAsync(string idToken)
    {
        var endpoint = $"{_baseApiUrl}accounts:lookup?key={_apiKey}";
        var obj = new { idToken };
        var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        var res = await _httpClient.PostAsync(endpoint, content);
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<FirebaseUserLookupResponse>(json);
    }

    public async Task<RefreshTokenResponse?> RefreshTokenAsync(string refreshToken)
    {
        var url = $"https://securetoken.googleapis.com/v1/token?key={_apiKey}";

        var keyValues = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken }
        };

        var content = new FormUrlEncodedContent(keyValues);
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<RefreshTokenResponse>(json);
    }

    private void CreateApp(FirebaseConnection connectionSetting)
    {
        var connJson = JsonConvert.SerializeObject(connectionSetting);

        connJson = Regex.Replace(connJson, @"/\\n/g", @"\n");
        connJson = connJson.Replace(@"\n", "\n");
        connJson = Regex.Unescape(connJson);

        try
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(connJson)
            });
        }
        catch
        {
            // Firebase app already created
        }

        TenantsAuthInstances.Add("XML-INT", FirebaseAuth.DefaultInstance);
    }
}
