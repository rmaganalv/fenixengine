using FenixEngine.Agents.Services;

namespace FenixEngine.Agents.Services.OpenAI
{
    public class ServiceChatOpenAI
    {
        private readonly IOpenAIClient _client;
        private readonly string _model;

        public ServiceChatOpenAI(IOpenAIClient client, string model = "gpt-4o-mini")
        {
            _client = client;
            _model = model;
        }

        public async Task<string> SendMessageAsync(string prompt, string apiKey)
        {
            var payload = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            return await _client.PostJsonAsync("chat/completions", payload, apiKey);
        }
    }
}
