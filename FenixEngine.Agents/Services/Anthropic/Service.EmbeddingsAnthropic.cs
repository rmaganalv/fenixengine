using FenixEngine.Agents.Services;

namespace Service.Agents.Services.Anthropic
{
    public class ServiceEmbeddingsAnthropic
    {
        private readonly IAnthropicClient _client;
        private readonly string _model;

        public ServiceEmbeddingsAnthropic(IAnthropicClient client, string model = "claude-3-embeddings-20240229")
        {
            _client = client;
            _model = model;
        }

        public async Task<string> CreateEmbeddingsAsync(string text, string apiKey)
        {
            var payload = new
            {
                model = _model,
                input = text
            };

            return await _client.PostJsonAsync("embeddings", payload, apiKey);
        }
    }
}
