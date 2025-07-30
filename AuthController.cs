using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using Remundo.Firebase.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly FirebaseManager _firebaseManager;
    public AuthController(FirebaseManager firebaseManager)
    {
        _firebaseManager = firebaseManager;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        try {
            var res = await _firebaseManager.SignInWithEmailAndPasswordAsync(req);
            return Ok(res);
        }
        catch (Exception ex) {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest req)
    {
        try
        {
            var res = await _firebaseManager.RefreshTokenAsync(req.RefreshToken);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest req)
    {
        try
        {
            var res = await _firebaseManager.CreateUserWithEmailAndPasswordAsync(req);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("verify/{oobCode}")]
    public async Task<IActionResult> VerifyEmail(string oobCode)
    {
        try
        {
            var res = await _firebaseManager.VerifyEmail(oobCode);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("protected")]
    public IActionResult ProtectedEndpoint()
    {
        var user = HttpContext.User;
        return Ok(new { 
            message = "Hello authenticated user!", 
            uid = user.FindFirst("user_id")?.Value 
        });
    }
}
