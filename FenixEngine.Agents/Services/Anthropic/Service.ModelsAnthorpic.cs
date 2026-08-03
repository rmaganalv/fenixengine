using FenixEngine.Agents.Services;

namespace Service.Agents.Services.Anthropic
{
    public class ServiceModelsAnthorpic
    {
        private readonly IAnthropicClient _client;

        public ServiceModelsAnthorpic(IAnthropicClient client)
        {
            _client = client;
        }

        public async Task<string> ListModelsAsync(string apiKey)
        {
            return await _client.GetAsync("models", apiKey);
        }
    }
}
