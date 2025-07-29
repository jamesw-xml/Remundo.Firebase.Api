using Remundo.Firebase.Api.Models;

public class LoginRequest : BaseRequest
{
    public string password {get; set;}
    public string email {get; set; }
    public bool returnSecureToken { get; set; } = true;
}