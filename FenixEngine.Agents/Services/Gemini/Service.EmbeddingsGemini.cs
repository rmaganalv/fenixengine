namespace FenixEngine.Agents.Services.Gemini
{
    public class ServiceEmbeddingsGemini
    {
        private readonly IGeminiClient _client;
        private readonly string _model;

        public ServiceEmbeddingsGemini(IGeminiClient client, string model = "embedding-001")
        {
            _client = client;
            _model = model;
        }

        public async Task<string> CreateEmbeddingsAsync(string text, string apiKey)
        {
            var payload = new { model = _model, content = new { parts = new[] { new { text } } } };
            return await _client.PostJsonAsync("models/" + _model + ":embedContent", payload, apiKey);
        }
    }
}
