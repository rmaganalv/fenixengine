using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Media;

namespace FenixEngine.Src.Control;

public class NativeTtsService
{
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Lee un texto usando la síntesis de voz nativa del dispositivo.
    /// </summary>
    public async Task SpeakAsync(string text, float volume = 1.0f, float pitch = 1.0f)
    {
        try
        {
            // Cancelar cualquier lectura que esté en curso
            CancelSpeech();

            _cts = new CancellationTokenSource();

            // Buscar la voz preferida en español (si existe en el SO)
            IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();
            Locale? spanishLocale = locales.FirstOrDefault(l => l.Language.StartsWith("es", StringComparison.OrdinalIgnoreCase));

            var options = new SpeechOptions
            {
                Volume = volume, // Rango: 0.0 a 1.0
                Pitch = pitch,   // Rango: 0.0 a 2.0 (1.0 es el tono normal)
                Locale = spanishLocale // Asigna la voz en español si la encuentra
            };

            // Ejecuta la reproducción nativa
            await TextToSpeech.Default.SpeakAsync(text, options, cancelToken: _cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Ocurre de forma normal si el usuario detiene la lectura manualmente
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en TTS Nativo: {ex.Message}");
        }
    }

    /// <summary>
    /// Detiene la lectura de inmediato.
    /// </summary>
    public void CancelSpeech()
    {
        if (_cts != null && !_cts.IsCancellationRequested)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}