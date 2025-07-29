using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Remundo.Firebase.Api.Models;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var settings = builder.Configuration.Get<AppSettings>();
builder.Services.AddHttpClient<FirebaseManager>((sp, client) =>
{
    var apiKey = settings.FirebaseApiKey;

    // Append key as default query string
    client.DefaultRequestHeaders.Add("X-Firebase-ApiKey", apiKey);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddSingleton<AppSettings>(settings);

// Configure Firebase


var app = builder.Build();
app.UseCors(x =>
{
    x.AllowAnyOrigin();
    x.AllowAnyHeader();
    x.AllowAnyMethod();
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
