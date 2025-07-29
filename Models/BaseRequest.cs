using System.Text.Json.Serialization;

namespace Remundo.Firebase.Api.Models
{
    public class BaseRequest
    {
        public string? TenantId { get; set; }
    }
}
