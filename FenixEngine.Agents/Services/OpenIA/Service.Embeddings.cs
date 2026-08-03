using FenixEngine.Agents.Services;

namespace FenixEngine.Agents.Services.OpenAI
{
    public class ServiceEmbeddingOpenAI
    {
        private readonly IOpenAIClient _client;
        private readonly string _model;

        public ServiceEmbeddingOpenAI(IOpenAIClient client, string model = "text-embedding-3-small")
        {
            _client = client;
            _model = model;
        }

        public async Task<string> CreateEmbeddingsAsync(string input, string apiKey)
        {
            var payload = new
            {
                model = _model,
                input
            };

            return await _client.PostJsonAsync("embeddings", payload, apiKey);
        }
    }
}
