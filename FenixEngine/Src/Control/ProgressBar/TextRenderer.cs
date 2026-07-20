using Microsoft.Maui.Graphics;

namespace FenixEngine.Src.Control
{
    public static class TextRenderer
    {
        public static void DrawCentered(ICanvas canvas, string text, RectF rect, Color color)
        {
            if (string.IsNullOrEmpty(text)) return;

            canvas.FontColor = color;

#if NET8_0_OR_GREATER
            // Si está disponible la sobrecarga con rectángulo
            canvas.DrawString(text, rect, HorizontalAlignment.Center, VerticalAlignment.Center);
#else
            // Fallback simple
            canvas.DrawString(text, rect.Center.X, rect.Center.Y, HorizontalAlignment.Center);
#endif
        }

        public static void DrawBottomCentered(ICanvas canvas, string text, RectF rect, Color color, float offsetY = 20)
        {
            if (string.IsNullOrEmpty(text)) return;

            canvas.FontColor = color;
            canvas.DrawString(text, rect.Center.X, rect.Bottom + offsetY, HorizontalAlignment.Center);
        }
    }
}
