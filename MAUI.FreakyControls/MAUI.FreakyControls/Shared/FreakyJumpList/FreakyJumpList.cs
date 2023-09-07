using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Maui.FreakyControls.Extensions;

namespace Maui.FreakyControls;

public class FreakyJumpList : SKCanvasView, IDisposable
{
    private IDictionary<string, SKPoint> charLocationDictionary;
    public event EventHandler<FreakyCharacterChangedEventArgs> SelectedCharacterChanged;

    public static readonly BindableProperty CharacterColorProperty = BindableProperty.Create(
        nameof(CharacterColor),
        typeof(Color),
        typeof(FreakyJumpList),
        Colors.Black
        );

    public static readonly BindableProperty SelectedCharacterColorProperty = BindableProperty.Create(
        nameof(SelectedCharacterColor),
        typeof(Color),
        typeof(FreakyJumpList),
        Colors.Black
        );

    public static readonly BindableProperty SelectedCharacterProperty = BindableProperty.Create(
        nameof(SelectedCharacter),
        typeof(string),
        typeof(FreakyJumpList),
        string.Empty
        );

    public static readonly BindableProperty CharacterSizeProperty = BindableProperty.Create(
        nameof(CharacterSize),
        typeof(float),
        typeof(FreakyJumpList),
        40.0f
        );

    public static readonly BindableProperty HasHaptikFeedbackProperty = BindableProperty.Create(
        nameof(HasHaptikFeedback),
        typeof(bool),
        typeof(FreakyJumpList),
        false
        );

    public static readonly BindableProperty AlphabetProviderProperty = BindableProperty.Create(
       nameof(AlphabetProvider),
       typeof(IAlphabetProvider),
       typeof(FreakyJumpList),
       null
       );

    // <summary>
    // Gets or sets a AlphabetProvider instance
    // </summary>
    public IAlphabetProvider AlphabetProvider
    {
        get => (IAlphabetProvider)GetValue(AlphabetProviderProperty);
        set => SetValue(AlphabetProviderProperty, value);
    }

    // <summary>
    // Gets or sets the height of the characters in pixels
    // </summary>
    public bool HasHaptikFeedback
    {
        get => (bool)GetValue(HasHaptikFeedbackProperty);
        set => SetValue(HasHaptikFeedbackProperty, value);
    }

    // <summary>
    // Gets or sets the height of the characters in pixels
    // </summary>
    public float CharacterSize
    {
        get => (float)GetValue(CharacterSizeProperty);
        set => SetValue(CharacterSizeProperty, value);
    }

    // <summary>
    // string formatted single character that you have touched/swiped to
    // </summary>
    public string SelectedCharacter
    {
        get => (string)GetValue(SelectedCharacterProperty);
        set => SetValue(SelectedCharacterProperty, value);
    }

    // <summary>
    // of type Color, defines the color of the jump list's selected character.
    // </summary>
    public Color SelectedCharacterColor
    {
        get => (Color)GetValue(SelectedCharacterColorProperty);
        set => SetValue(SelectedCharacterColorProperty, value);
    }

    // <summary>
    // of type Color, defines the color of the jump list characters.
    // </summary>
    public Color CharacterColor
    {
        get => (Color)GetValue(CharacterColorProperty);
        set => SetValue(CharacterColorProperty, value);
    }

    public FreakyJumpList()
    {
        EnableTouchEvents = true;
    }

    private KeyValuePair<string, SKPoint> GetClosestPoint(SKTouchEventArgs e)
    {
        var touchedLocation = e.Location;
        var getAllPoints = charLocationDictionary.Select(x =>
        {
            var distance = SKPoint.Distance(touchedLocation, x.Value);
            return new KeyValuePair<string, float>(x.Key, distance);
        }).ToList();
        var getClosestPoint = getAllPoints.OrderBy(x => x.Value).FirstOrDefault();
        return charLocationDictionary.FirstOrDefault(x => x.Key == getClosestPoint.Key);
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);
        e?.Surface?.Canvas?.Clear();
        DrawJumpList(e);
    }

    protected override void OnTouch(SKTouchEventArgs e)
    {
        base.OnTouch(e);
        var closestPoint = GetClosestPoint(e);
        if (HasHaptikFeedback)
        {
            if (SelectedCharacter != closestPoint.Key)
            {
                try
                {
                    var duration = TimeSpan.MinValue;
                    Vibration.Vibrate(duration);
                }
                catch (FeatureNotSupportedException)
                {
                    // Feature not supported on device
                }
                catch (Exception)
                {
                    // Other error has occurred.
                }
            }
        }
        SelectedCharacter = closestPoint.Key;
        SelectedCharacterChanged?.Invoke(this,
            new()
            {
                SelectedCharacter = closestPoint.Key
            });
        e.Handled = true;
        this.InvalidateSurface();
    }

    private void DrawJumpList(SKPaintSurfaceEventArgs e)
    {
        if (e == null)
        {
            throw new ArgumentNullException(nameof(e) + typeof(SKPaintSurfaceEventArgs) + " cannot be null");
        }

        if(this.AlphabetProvider== null || this.AlphabetProvider.GetCount() == 0)
        {
            throw new InvalidDataException($"{nameof(AlphabetProvider)} cannot be null or empty, make sure you create an IAlphabetProvider instance" );
        }

        var info = e.Info;
        var canvas = e.Surface?.Canvas;
        var  provider = this.AlphabetProvider;
        var maxCount = this.AlphabetProvider?.GetCount();
        charLocationDictionary = new Dictionary<string, SKPoint>();
        foreach (var (item, index) in provider.GetAlphabet().WithIndex())
        {
            var currentAlphabet = item.ToString();
            var currentIndex = index + 1;
            using var textPaint = new SKPaint()
            {
                Color = CharacterColor.ToSKColor(),
                TextSize = this.CharacterSize,
                TextAlign = SKTextAlign.Center,
                FakeBoldText=true
            };
            if (currentAlphabet == this.SelectedCharacter)
            {
                textPaint.Color = SelectedCharacterColor.ToSKColor();
            }
            var point = new SKPoint((float)(info.Width / 2.0), (float)(info.Height / maxCount * currentIndex));
            charLocationDictionary.Add(currentAlphabet, point);
            canvas?.DrawText(currentAlphabet, point, textPaint);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~FreakyJumpList()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
}

public interface IAlphabetProvider
{
    IEnumerable<char> GetAlphabet();

    int GetCount();
}