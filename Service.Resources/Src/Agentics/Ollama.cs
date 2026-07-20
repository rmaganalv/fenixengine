using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Resources.Agentics;
    public class OllamaClient
    {
        private readonly HttpClient _httpClient;
        private const string OllamaUrl = "http://localhost:11434/api/generate";

        public OllamaClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> QueryCodeGemmaAsync(string systemPrompt, string userPrompt)
        {
            // Unimos los prompts para el endpoint /api/generate de Ollama
            // CodeGemma responde muy bien cuando delimitamos claramente las instrucciones del sistema
            string fullPrompt = $"<start_of_turn>user\n{systemPrompt}\n\nESPECIFICACIÓN DEL USUARIO:\n{userPrompt}<end_of_turn>\n<start_of_turn>model\n";

            var requestBody = new
            {
                model = "codegemma", // Asegurar que el usuario tenga este modelo en Ollama
                prompt = fullPrompt,
                stream = false,      // Desactivamos el streaming para procesar el bloque completo
                format = "json",     // Obliga a Ollama/CodeGemma a escupir JSON válido
                options = new
                {
                    temperature = 0.1, // Temperatura muy baja = Más determinismo, menos alucinación
                    top_p = 0.9
                }
            };

            string jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(OllamaUrl, content);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            
            // Ollama envuelve la respuesta en un objeto con metadatos. Extraemos solo el campo "response".
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            string rawLlmOutput = doc.RootElement.GetProperty("response").GetString();

            return rawLlmOutput;
        }
    }
