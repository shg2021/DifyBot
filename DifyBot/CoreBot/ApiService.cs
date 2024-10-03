using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _bearerToken;

    public ApiService(HttpClient httpClient, string bearerToken)
    {
        _httpClient = httpClient;
        _bearerToken = bearerToken;
    }

    public async Task<string> MakeApiCall(string user, string var1)
    {
        try
        {
            var url = "https://api.dify.ai/v1/workflows/run";

            var payload = new
            {
                user = user,
                inputs = new
                {
                    VAR_1 = var1
                }
            };

            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseContent);

            // Extract the "result" field from the response
            var result = jsonResponse["data"]["outputs"]["result"].ToString();

            return result;
        }
        catch (HttpRequestException ex)
        {
            return $"Error: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Unexpected error: {ex.Message}";
        }
    }
}