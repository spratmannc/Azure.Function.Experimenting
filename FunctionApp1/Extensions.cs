using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public static class Extensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpRequest request)
        {
            using var reader = new StreamReader(request.Body);

            var json = await reader.ReadToEndAsync();

            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
