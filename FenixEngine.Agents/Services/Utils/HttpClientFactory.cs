using System.Net.Http;

namespace FenixEngine.Agents.Utils
{
    public static class HttpClientFactoryHelper
    {
        private static readonly Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(() =>
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            return client;
        });

        public static HttpClient Instance => _httpClient.Value;
    }
}
