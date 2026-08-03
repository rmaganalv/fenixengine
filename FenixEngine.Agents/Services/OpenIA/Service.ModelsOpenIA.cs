using FenixEngine.Agents.Services;

namespace FenixEngine.Agents.Services.OpenAI
{
    public class ServiceModelsOpenAI
    {
        private readonly IOpenAIClient _client;

        public ServiceModelsOpenAI(IOpenAIClient client)
        {
            _client = client;
        }

        public async Task<string> ListModelsAsync(string apiKey)
        {
            return await _client.GetAsync("models", apiKey);
        }
    }
}
