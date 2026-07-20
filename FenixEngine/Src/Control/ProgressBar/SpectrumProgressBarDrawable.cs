using FenixEngine.Src.Control;
using Microsoft.Maui.Graphics;

namespace FenixEngine.Src.Control;

public class SpectrumProgressBarDrawable : IDrawable
{
    private readonly SpectrumProgressBar _bar;
    public SpectrumProgressBarDrawable(SpectrumProgressBar bar) => _bar = bar;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float radius = 10f;
        float progressWidth = (float)(dirtyRect.Width * _bar.AnimatedProgress);

        // Fondo vacío
        canvas.FillColor = _bar.EmptyColor;
        canvas.FillRoundedRectangle(dirtyRect, radius);

        // Relleno animado
        var fillRect = new RectF(dirtyRect.X, dirtyRect.Y, progressWidth, dirtyRect.Height);
        canvas.FillColor = _bar.FillColor;
        canvas.FillRoundedRectangle(fillRect, radius);

        // Etiqueta central (encima del relleno)
        TextRenderer.DrawCentered(canvas, _bar.CenterLabel, dirtyRect, Colors.White);

        // Etiqueta espectro (debajo de la barra)
        string spectrum = _bar.GetSpectrum();
        canvas.FontColor = Colors.BlanchedAlmond;
        TextRenderer.DrawBottomCentered(canvas, spectrum, dirtyRect, Colors.BlanchedAlmond, 20);
    }

}