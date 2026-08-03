
namespace FenixEngine.Src.Control.Speaches;

public interface ISpeechToTextService
{
    Task<string> RecognizeAsync();
}
