namespace FenixEngine.Shared.Services;


public interface IaServiceClient
{
    Task<string> PostJsonAsync(string endpoint, object payload, string? apiKey = null);
    Task<string> GetExternalAsync(string BaseUrl, string endpoint, string? apiKey = null);
    Task<string> ListModelsAsync(string apiKey);
}