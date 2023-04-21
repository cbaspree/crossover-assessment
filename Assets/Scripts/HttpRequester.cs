using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor.PackageManager;

public class HttpRequester
{
    private static readonly HttpClient _client = new HttpClient();
    public static async Task<RequestResult> Get(string url)
    {
        RequestResult result;
        using HttpResponseMessage response = await _client.GetAsync(url);
        
        if (response.IsSuccessStatusCode == false)
        {
            Error error = new Error(response.StatusCode, response.ReasonPhrase);
            result = new RequestResult(null, error);
            return result;
        }

        string responseBody = await response.Content.ReadAsStringAsync();
        result = new RequestResult(responseBody, null);
        return result;
    }

}
