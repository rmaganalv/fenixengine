
namespace FenixEngine.Src.Control;

public class SpectrumProgressBar : GraphicsView
{
    public static readonly BindableProperty ProgressProperty =
        BindableProperty.Create(nameof(Progress), typeof(double), typeof(SpectrumProgressBar), 0.0,
            propertyChanged: OnProgressChanged);

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public event EventHandler<double> OnValueChanged;

    private static void OnProgressChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var bar = (SpectrumProgressBar)bindable;
        double start = (double)oldValue;
        double end = (double)newValue;

        var animation = new Animation(v =>
        {
            bar._animatedProgress = v;
            bar.Invalidate();
        }, start, end);

        animation.Commit(bar, "ProgressAnimation", length: 400, easing: Easing.CubicInOut);

        bar.OnValueChanged?.Invoke(bar, end);
    }

    private double _animatedProgress = 0;
    internal double AnimatedProgress => _animatedProgress;

    public int SpectrumLength { get; set; } = 20;
    public Color FillColor { get; set; } = Colors.Green;
    public Color EmptyColor { get; set; } = Colors.Gray;
    public string CenterLabel { get; set; } = "";

    private readonly Random _rand = new();
    private string _animatedSpectrum = "";

    public SpectrumProgressBar()
    {
        Drawable = new SpectrumProgressBarDrawable(this);

        // TrackBar interactividad
        this.StartInteraction += OnTouch;
        this.DragInteraction += OnTouch;

        // Timer para espectro pulsante
        var timer = Application.Current?.Dispatcher.CreateTimer();
        timer?.Interval = TimeSpan.FromMilliseconds(120);
        timer?.Tick += (s, e) =>
        {
            _animatedSpectrum = BuildAnimatedSpectrum();
            Invalidate();
        };
        timer?.Start();
    }

    private void OnTouch(object sender, TouchEventArgs e)
    {
        var x = e.Touches[0].X;
        Progress = Math.Clamp(x / Width, 0, 1);
    }

    internal string GetSpectrum() => _animatedSpectrum;

    private string BuildAnimatedSpectrum()
    {
        int filled = (int)(AnimatedProgress * SpectrumLength);
        char[] chars = new char[SpectrumLength];
        return new string(chars);
    }
}

