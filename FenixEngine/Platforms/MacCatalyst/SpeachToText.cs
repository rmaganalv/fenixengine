
using Speech;
using AVFoundation;
using FenixEngine.Src.Control.Speaches;
using Foundation;

public class SpeechToTextService : ISpeechToTextService
{
public async Task<string> RecognizeAsync()
{
    var authStatus = await RequestAuthorizationAsync();
    var tcs = new TaskCompletionSource<string>();
    try{

    if (authStatus != SFSpeechRecognizerAuthorizationStatus.Authorized)
        throw new Exception("Permiso de reconocimiento de voz denegado.");

    var session = AVAudioSession.SharedInstance();
    session.SetCategory(AVAudioSessionCategory.PlayAndRecord, AVAudioSessionCategoryOptions.DefaultToSpeaker);
    session.SetActive(true, out var sessionError);

    var recognizer = new SFSpeechRecognizer(new NSLocale("es-MX"));
    var request = new SFSpeechAudioBufferRecognitionRequest();
    var audioEngine = new AVAudioEngine();

    var node = audioEngine.InputNode;
    var format = node.GetBusOutputFormat(0);

    node.InstallTapOnBus(0, 1024, format, (buffer, when) =>
    {
        request.Append(buffer);
    });

    audioEngine.Prepare();
    audioEngine.StartAndReturnError(out var error);
    if (error != null)
        throw new Exception(error.LocalizedDescription);

    
    recognizer.GetRecognitionTask(request, (result, err) =>
    {
        if (result != null)
        {
            //Console.WriteLine("Reconocido: " + result.BestTranscription.FormattedString);
            //onTextRecognized?.Invoke(result.BestTranscription.FormattedString);

            if (result.Final)
            {
                tcs.TrySetResult(result.BestTranscription.FormattedString);
                audioEngine.Stop();
                request.EndAudio();
                node.RemoveTapOnBus(0);
            }
        }
    });

    }
    catch(Exception e)
    { 
        Console.WriteLine(e.Message.ToString());
    }
    return await tcs.Task;
}


    private Task<SFSpeechRecognizerAuthorizationStatus> RequestAuthorizationAsync()
    {
        var tcs = new TaskCompletionSource<SFSpeechRecognizerAuthorizationStatus>();
        SFSpeechRecognizer.RequestAuthorization(status =>
        {
            tcs.SetResult(status);
        });
        return tcs.Task;
    }
}

