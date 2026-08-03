#if ANDROID
using Android.Speech;
using Android.Content;

public class SpeechToTextService : ISpeechToTextService
{
    public Task<string> RecognizeAsync()
    {
        var tcs = new TaskCompletionSource<string>();
        var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
        intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
        intent.PutExtra(RecognizerIntent.ExtraPrompt, "Habla ahora...");

        // Aquí necesitas lanzar la actividad y capturar el resultado
        // con ActivityResultCallback. El texto reconocido se pasa a tcs.SetResult(result);

        return tcs.Task;
    }
}
#endif
