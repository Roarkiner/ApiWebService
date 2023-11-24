using ApiWebService.Contracts;
using System.Text;
using System.Text.Json;

namespace ApiWebService.Services
{
    public class ETagService : IETagService
    {
        public string GenerateETag<T>(T value) where T : class
        {
            var serializedValue = JsonSerializer.Serialize(value);
            var encodedValue = Encoding.UTF8.GetBytes(serializedValue);
            return Convert.ToBase64String(encodedValue);
        }
    }
}
