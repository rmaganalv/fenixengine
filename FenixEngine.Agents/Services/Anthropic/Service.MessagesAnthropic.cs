using FenixEngine.Agents.Services;

namespace Service.Agents.Services.Anthropic
{
    public class ServiceMessagesAnthropic
    {
        private readonly IAnthropicClient _client;
        private readonly string _model;

        public ServiceMessagesAnthropic(IAnthropicClient client, string model = "claude-3-opus-20240229")
        {
            _client = client;
            _model = model;
        }

        public async Task<string> SendMessageAsync(string prompt, string apiKey)
        {
            var payload = new
            {
                model = _model,
                max_tokens = 1024,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            return await _client.PostJsonAsync("messages", payload, apiKey);
        }
    }
}
