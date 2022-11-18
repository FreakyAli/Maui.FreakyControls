using System;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Windows.Input;
using SkiaSharp.Views.Maui.Controls;

namespace Maui.FreakyControls;

public class Checkbox : ContentView, IDisposable
{
    #region Fields

    bool _isAnimating;
    SKCanvasView _skiaView;
    ICommand _toggleCommand;

    #endregion

    #region Constructor

    public Checkbox()
    {
        InitializeCanvas();
        WidthRequest = HeightRequest = DEFAULT_SIZE;
        HorizontalOptions = VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false);
        Content = _skiaView;
        GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = _toggleCommand
        });
    }

    #endregion

    #region Defaults

    private static Design design => Design.Unified;

    private static Shape shape => DeviceInfo.Platform == DevicePlatform.iOS ?
                Shape.Circle : Shape.Rectangle;

    private static float outlineWidth => DeviceInfo.Platform == DevicePlatform.iOS ?
                   4.0f : 6.0f;

    private static double DEFAULT_SIZE => 24.0;

    #endregion

    #region Initialize Canvas

    void InitializeCanvas()
    {
        _toggleCommand = new Command(OnTappedCommand);

        _skiaView = new SKCanvasView();
        _skiaView.PaintSurface += Handle_PaintSurface;
        _skiaView.WidthRequest = _skiaView.HeightRequest = DEFAULT_SIZE;
    }

    void OnTappedCommand(object obj)
    {
        if (_isAnimating)
            return;

        IsChecked = !IsChecked;
    }

    async Task AnimateToggle()
    {
        _isAnimating = true;
        await _skiaView.ScaleTo(0.85, 100);
        _skiaView.InvalidateSurface();
        await _skiaView.ScaleTo(1, 100, Easing.BounceOut);
        _isAnimating = false;
    }
    #endregion

    #region Checkmark Paint Surface

    void Handle_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        e?.Surface?.Canvas?.Clear();

        DrawOutline(e);

        if (IsChecked)
            DrawCheckFilled(e);
    }

    void DrawCheckFilled(SKPaintSurfaceEventArgs e)
    {
        var imageInfo = e.Info;
        var canvas = e?.Surface?.Canvas;

        using (var checkfill = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = FillColor.ToSKColor(),
            StrokeJoin = SKStrokeJoin.Round,
            IsAntialias = true
        })
        {
            var shape = Design == Design.Unified ? Shape : Checkbox.shape;
            if (shape == Shape.Circle)
                canvas.DrawCircle(imageInfo.Width / 2, imageInfo.Height / 2, (imageInfo.Width / 2) - (OutlineWidth / 2), checkfill);
            else
            {
                var cornerRadius = OutlineWidth;
                canvas.DrawRoundRect(OutlineWidth, OutlineWidth, imageInfo.Width - (OutlineWidth * 2), imageInfo.Height - (OutlineWidth * 2), cornerRadius, cornerRadius, checkfill);
            }
        }

        using (var checkPath = new SKPath())
        {
            if (Design == Design.Unified)
            {
                checkPath.MoveTo(.275f * imageInfo.Width, .5f * imageInfo.Height);
                checkPath.LineTo(.425f * imageInfo.Width, .65f * imageInfo.Height);
                checkPath.LineTo(.725f * imageInfo.Width, .375f * imageInfo.Height);
            }
            else
            {
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    checkPath.MoveTo(.2f * imageInfo.Width, .5f * imageInfo.Height);
                    checkPath.LineTo(.375f * imageInfo.Width, .675f * imageInfo.Height);
                    checkPath.LineTo(.75f * imageInfo.Width, .3f * imageInfo.Height);
                }
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    checkPath.MoveTo(.2f * imageInfo.Width, .5f * imageInfo.Height);
                    checkPath.LineTo(.425f * imageInfo.Width, .7f * imageInfo.Height);
                    checkPath.LineTo(.8f * imageInfo.Width, .275f * imageInfo.Height);
                }
            }

            using (var checkStroke = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = CheckColor.ToSKColor(),
                StrokeWidth = OutlineWidth,
                IsAntialias = true
            })
            {
                checkStroke.StrokeCap = Design == Design.Unified ? SKStrokeCap.Round : SKStrokeCap.Butt;
                canvas.DrawPath(checkPath, checkStroke);
            }
        }
    }

    void DrawOutline(SKPaintSurfaceEventArgs e)
    {

        var imageInfo = e.Info;
        var canvas = e?.Surface?.Canvas;

        using (var outline = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = OutlineColor.ToSKColor(),
            StrokeWidth = OutlineWidth,
            StrokeJoin = SKStrokeJoin.Round,
            IsAntialias = true
        })
        {
            var shape = Design == Design.Unified ? Shape : Checkbox.shape;
            if (shape == Shape.Circle)
                canvas.DrawCircle(imageInfo.Width / 2, imageInfo.Height / 2, (imageInfo.Width / 2) - (OutlineWidth / 2), outline);
            else
            {
                var cornerRadius = OutlineWidth;
                canvas.DrawRoundRect(OutlineWidth, OutlineWidth, imageInfo.Width - (OutlineWidth * 2), imageInfo.Height - (OutlineWidth * 2), cornerRadius, cornerRadius, outline);
            }
        }
    }
    #endregion

    #region Events
    /// <summary>
    /// Raised when IsChecked is changed.
    /// </summary>
    public event EventHandler<TappedEventArgs> IsCheckedChanged;
    #endregion

    #region Bindable Properties
    public static BindableProperty OutlineColorProperty = BindableProperty.Create(nameof(OutlineColor), typeof(Color), typeof(Checkbox), Colors.Blue);  /// <summary>
                                                                                                                                                        /// Gets or sets the color of the outline.
                                                                                                                                                        /// </summary>
                                                                                                                                                        /// <value>Color value of the outline</value>
    public Color OutlineColor
    {
        get { return (Color)GetValue(OutlineColorProperty); }
        set { SetValue(OutlineColorProperty, value); }
    }

    public static BindableProperty FillColorProperty = BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(Checkbox), Colors.Blue);
    /// <summary>
    /// Gets or sets the color of the fill.
    /// </summary>
    /// <value>Color value of the fill.</value>
    public Color FillColor
    {
        get { return (Color)GetValue(FillColorProperty); }
        set { SetValue(FillColorProperty, value); }
    }

    public static BindableProperty CheckColorProperty = BindableProperty.Create(nameof(CheckColor), typeof(Color), typeof(Checkbox), Colors.White);
    /// <summary>
    /// Gets or sets the color of the check.
    /// </summary>
    /// <value>Color of the check.</value>
    public Color CheckColor
    {
        get { return (Color)GetValue(CheckColorProperty); }
        set { SetValue(CheckColorProperty, value); }
    }

    public static BindableProperty OutlineWidthProperty = BindableProperty.Create(nameof(OutlineWidth), typeof(float), typeof(Checkbox), outlineWidth);
    /// <summary>
    /// Gets or sets the width of the outline and check.
    /// </summary>
    /// <value>The width of the outline and check.</value>
    public float OutlineWidth
    {
        get { return (float)GetValue(OutlineWidthProperty); }
        set { SetValue(OutlineWidthProperty, value); }
    }

    public static BindableProperty ShapeProperty = BindableProperty.Create(nameof(Shape), typeof(Shape), typeof(Checkbox), shape);
    /// <summary>
    /// Gets or sets the shape of the <see cref="Checkbox"/>.
    /// </summary>
    public Shape Shape
    {
        get { return (Shape)GetValue(ShapeProperty); }
        set { SetValue(ShapeProperty, value); }
    }

    public static BindableProperty DesignProperty = BindableProperty.Create(nameof(Design), typeof(Design), typeof(Checkbox), design);

    /// <summary>
    /// Gets or sets the shape of the <see cref="Checkbox"/>.
    /// </summary>
    public Design Design
    {
        get { return (Design)GetValue(DesignProperty); }
        set { SetValue(DesignProperty, value); }
    }

    public static new BindableProperty StyleProperty = BindableProperty.Create(nameof(Style), typeof(Style), typeof(Checkbox), propertyChanged: OnStyleChanged);

    /// <summary>
    /// Gets or sets the style for <see cref="Checkbox"/>.
    /// </summary>
    /// <value>The style.</value>
    public new Style Style
    {
        get { return (Style)GetValue(StyleProperty); }
        set { SetValue(StyleProperty, value); }
    }

    static void OnStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is Checkbox checkbox)) return;

        var setters = ((Style)newValue).Setters;
        var dict = new Dictionary<string, Color>();

        foreach (var setter in setters)
        {
            dict.Add(setter.Property.PropertyName, (Color)setter.Value);
        }

        checkbox.OutlineColor = dict[nameof(OutlineColor)];
        checkbox.FillColor = dict[nameof(FillColor)];
        checkbox.CheckColor = dict[nameof(CheckColor)];
    }

    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(Checkbox), false, BindingMode.TwoWay, propertyChanged: OnIsCheckedChanged);
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:IntelliAbb.Xamarin.Controls.Checkbox"/> is checked.
    /// </summary>
    /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
    public bool IsChecked
    {
        get { return (bool)GetValue(IsCheckedProperty); }
        set { SetValue(IsCheckedProperty, value); }
    }

    static async void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is Checkbox checkbox)) return;
        checkbox.IsCheckedChanged?.Invoke(checkbox, new TappedEventArgs((bool)newValue));
        await checkbox.AnimateToggle();
    }
    #endregion

    #region IDisposable

    public void Dispose()
    {
        _skiaView.PaintSurface -= Handle_PaintSurface;
        GestureRecognizers.Clear();
    }

    #endregion
}

public enum Shape
{
    Circle,
    Rectangle
}

public enum Design
{
    Unified,
    Native
}

