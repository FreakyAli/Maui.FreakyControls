using System;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Windows.Input;
using SkiaSharp.Views.Maui.Controls;
using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public class FreakyCheckbox : ContentView, IDisposable
{
    #region Fields

    bool isAnimating;
    SKCanvasView skiaView;
    TapGestureRecognizer tapped = new();
    #endregion

    #region ctor

    public FreakyCheckbox()
    {
        InitializeCanvas();
        WidthRequest = HeightRequest = size;
        HorizontalOptions = VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false);
        Content = skiaView;
        tapped.Tapped += CheckBox_Tapped;
        GestureRecognizers.Add(tapped);
    }

    ~FreakyCheckbox()
    {
        tapped.Tapped -= CheckBox_Tapped;
        GestureRecognizers.Remove(tapped);
    }

    private void CheckBox_Tapped(object sender, EventArgs e)
    {
        if (IsEnabled)
        {
            if (isAnimating)
                return;

            IsChecked = !IsChecked;
        }
    }

    #endregion

    #region Defaults

    private static readonly Design design = Shared.Enums.Design.Unified;

    private static readonly Shape shape = DeviceInfo.Platform == DevicePlatform.iOS ?
                Shared.Enums.Shape.Circle : Shared.Enums.Shape.Rectangle;

    private static readonly float outlineWidth = DeviceInfo.Platform == DevicePlatform.iOS ?
                   4.0f : 6.0f;

    private static readonly double size = 24.0;

    #endregion

    #region Canvas

    void InitializeCanvas()
    {
        skiaView = new SKCanvasView();
        skiaView.PaintSurface += Handle_PaintSurface;
        skiaView.WidthRequest = skiaView.HeightRequest = size;
    }

    async Task AnimateToggle()
    {
        if (HasCheckAnimation)
        {
            isAnimating = true;
            await skiaView.ScaleTo(0.85, 100);
        }
        skiaView.InvalidateSurface();
        if (HasCheckAnimation)
        {
            await skiaView.ScaleTo(1, 100, Easing.BounceOut);
            isAnimating = false;
        }
    }
    #endregion

    #region Skia

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

        using (SKPaint checkfill = new()
        {
            Style = SKPaintStyle.Fill,
            Color = FillColor.ToSKColor(),
            StrokeJoin = SKStrokeJoin.Round,
            IsAntialias = true
        })
        {
            var shape = Design == Design.Unified ? Shape : FreakyCheckbox.shape;
            if (shape == Shared.Enums.Shape.Circle)
            {
                canvas.DrawCircle(imageInfo.Width / 2, imageInfo.Height / 2, (imageInfo.Width / 2) - (OutlineWidth / 2), checkfill);
            }
            else
            {
                var cornerRadius = OutlineWidth;
                canvas.DrawRoundRect(OutlineWidth, OutlineWidth, imageInfo.Width - (OutlineWidth * 2), imageInfo.Height - (OutlineWidth * 2), cornerRadius, cornerRadius, checkfill);
            }
        }

        using var checkPath = new SKPath();
        if (Design == Design.Unified)
        {
            if (CheckType == CheckType.Check)
            {
                checkPath.MoveTo(.275f * imageInfo.Width, .5f * imageInfo.Height);
                checkPath.LineTo(.425f * imageInfo.Width, .65f * imageInfo.Height);
                checkPath.LineTo(.725f * imageInfo.Width, .375f * imageInfo.Height);
            }
            else if (CheckType == CheckType.Cross)
            {
                checkPath.MoveTo(.75f * imageInfo.Width, .25f * imageInfo.Height);
                checkPath.LineTo(.25f * imageInfo.Width, .75f * imageInfo.Height);
                checkPath.MoveTo(.75f * imageInfo.Width, .75f * imageInfo.Height);
                checkPath.LineTo(.25f * imageInfo.Width, .25f * imageInfo.Height);
            }
            else if (CheckType == CheckType.Line)
            {
                checkPath.MoveTo(.2f * imageInfo.Width, .5f * imageInfo.Height);
                checkPath.LineTo(.8f * imageInfo.Width, .5f * imageInfo.Height);
            }
            else
            {
                checkPath.MoveTo(.2f * imageInfo.Width, .8f * imageInfo.Height);
                checkPath.LineTo(.8f * imageInfo.Width, .8f * imageInfo.Height);
                checkPath.LineTo(.8f * imageInfo.Width, .2f * imageInfo.Height);
                checkPath.LineTo(.2f * imageInfo.Width, .2f * imageInfo.Height);
                checkPath.LineTo(.2f * imageInfo.Width, .8f * imageInfo.Height);
                checkPath.Close();
            }
        }
        else
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                checkPath.MoveTo(.2f * imageInfo.Width, .5f * imageInfo.Height);
                checkPath.LineTo(.375f * imageInfo.Width, .675f * imageInfo.Height);
                checkPath.LineTo(.75f * imageInfo.Width, .3f * imageInfo.Height);
            }
            else
            {
                checkPath.MoveTo(.2f * imageInfo.Width, .5f * imageInfo.Height);
                checkPath.LineTo(.425f * imageInfo.Width, .7f * imageInfo.Height);
                checkPath.LineTo(.8f * imageInfo.Width, .275f * imageInfo.Height);
            }
        }

        using var checkStroke = new SKPaint
        {
            Style = Design == Design.Unified && CheckType == CheckType.Box ? SKPaintStyle.Fill : SKPaintStyle.Stroke,
            Color = CheckColor.ToSKColor(),
            StrokeWidth = CheckWidth,
            IsAntialias = true,
            StrokeCap = Design == Design.Unified ? SKStrokeCap.Round : SKStrokeCap.Butt
        };

        canvas.DrawPath(checkPath, checkStroke);
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
            var shape = Design == Design.Unified ? Shape : FreakyCheckbox.shape;
            if (shape == Shared.Enums.Shape.Circle)
            {
                canvas.DrawCircle(
                    imageInfo.Width / 2,
                    imageInfo.Height / 2,
                    (imageInfo.Width / 2) - (OutlineWidth / 2),
                    outline);
            }
            else
            {
                var cornerRadius = OutlineWidth;
                canvas.DrawRoundRect(
                    OutlineWidth,
                    OutlineWidth,
                    imageInfo.Width - (OutlineWidth * 2),
                    imageInfo.Height - (OutlineWidth * 2),
                    cornerRadius,
                    cornerRadius,
                    outline);
            }
        }
    }
    #endregion

    #region Events
    /// <summary>
    /// Raised when IsChecked is changed.
    /// </summary>
    public event EventHandler<CheckedChangedEventArgs> CheckedChanged;
    #endregion

    #region Bindable Properties

    public static readonly BindableProperty HasCheckAnimationProperty =
    BindableProperty.Create(
        nameof(HasCheckAnimation),
        typeof(bool),
        typeof(FreakyCheckbox),
        true);

    /// <summary>
    /// Gets or sets the color of the outline.
    /// </summary>
    /// <value>Color value of the outline</value>
    public bool HasCheckAnimation
    {
        get => (bool)GetValue(HasCheckAnimationProperty);
        set => SetValue(HasCheckAnimationProperty, value);
    }

    public static readonly BindableProperty OutlineColorProperty =
    BindableProperty.Create(
        nameof(OutlineColor),
        typeof(Color),
        typeof(FreakyCheckbox),
        Colors.Black);

    /// <summary>
    /// Gets or sets the color of the outline.
    /// </summary>
    /// <value>Color value of the outline</value>
    public Color OutlineColor
    {
        get { return (Color)GetValue(OutlineColorProperty); }
        set { SetValue(OutlineColorProperty, value); }
    }

    public static readonly BindableProperty FillColorProperty =
    BindableProperty.Create(
        nameof(FillColor),
        typeof(Color),
        typeof(FreakyCheckbox),
        Colors.Black);

    /// <summary>
    /// Gets or sets the color of the fill.
    /// </summary>
    /// <value>Color value of the fill.</value>
    public Color FillColor
    {
        get { return (Color)GetValue(FillColorProperty); }
        set { SetValue(FillColorProperty, value); }
    }

    public static readonly BindableProperty CheckColorProperty =
    BindableProperty.Create(
        nameof(CheckColor),
        typeof(Color),
        typeof(FreakyCheckbox),
        Colors.White);

    /// <summary>
    /// Gets or sets the color of the check.
    /// </summary>
    /// <value>Color of the check.</value>
    public Color CheckColor
    {
        get { return (Color)GetValue(CheckColorProperty); }
        set { SetValue(CheckColorProperty, value); }
    }

    public static readonly BindableProperty OutlineWidthProperty =
    BindableProperty.Create(
        nameof(OutlineWidth),
        typeof(float),
        typeof(FreakyCheckbox),
        outlineWidth);

    /// <summary>
    /// Gets or sets the width of the outline.
    /// </summary>
    /// <value>The width of the outline and check.</value>
    public float OutlineWidth
    {
        get { return (float)GetValue(OutlineWidthProperty); }
        set { SetValue(OutlineWidthProperty, value); }
    }

    public static readonly BindableProperty CheckWidthProperty =
  BindableProperty.Create(
      nameof(CheckWidth),
      typeof(float),
      typeof(FreakyCheckbox),
      outlineWidth);

    /// <summary>
    /// Gets or sets the width of the check.
    /// </summary>
    /// <value>The width of the outline and check.</value>
    public float CheckWidth
    {
        get { return (float)GetValue(CheckWidthProperty); }
        set { SetValue(CheckWidthProperty, value); }
    }

    public static readonly BindableProperty ShapeProperty =
    BindableProperty.Create(
        nameof(Shape),
        typeof(Shape),
        typeof(FreakyCheckbox),
        shape);

    /// <summary>
    /// Gets or sets the shape of the <see cref="FreakyCheckbox"/>.
    /// </summary>
    public Shape Shape
    {
        get { return (Shape)GetValue(ShapeProperty); }
        set { SetValue(ShapeProperty, value); }
    }

    public static readonly BindableProperty CheckTypeProperty =
   BindableProperty.Create(
       nameof(CheckType),
       typeof(CheckType),
       typeof(FreakyCheckbox),
       CheckType.Check);

    /// <summary>
    /// Gets or sets the type of the check on <see cref="FreakyCheckbox"/>.
    /// </summary>
    public CheckType CheckType
    {
        get { return (CheckType)GetValue(CheckTypeProperty); }
        set { SetValue(CheckTypeProperty, value); }
    }


    public static readonly BindableProperty DesignProperty =
    BindableProperty.Create(
        nameof(Design),
        typeof(Design),
        typeof(FreakyCheckbox),
        design);

    /// <summary>
    /// Gets or sets the shape of the <see cref="FreakyCheckbox"/>.
    /// </summary>
    public Design Design
    {
        get { return (Design)GetValue(DesignProperty); }
        set { SetValue(DesignProperty, value); }
    }

    public static readonly new BindableProperty StyleProperty =
    BindableProperty.Create(
        nameof(Style),
        typeof(Style),
        typeof(FreakyCheckbox),
        propertyChanged: OnStyleChanged);

    /// <summary>
    /// Gets or sets the style for <see cref="FreakyCheckbox"/>.
    /// </summary>
    /// <value>The style.</value>
    public new Style Style
    {
        get { return (Style)GetValue(StyleProperty); }
        set { SetValue(StyleProperty, value); }
    }

    static void OnStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not FreakyCheckbox FreakyCheckbox) return;

        var setters = ((Style)newValue).Setters;
        var dict = new Dictionary<string, Color>();

        foreach (var setter in setters)
        {
            dict.Add(setter.Property.PropertyName, (Color)setter.Value);
        }

        FreakyCheckbox.OutlineColor = dict[nameof(OutlineColor)];
        FreakyCheckbox.FillColor = dict[nameof(FillColor)];
        FreakyCheckbox.CheckColor = dict[nameof(CheckColor)];
    }

    public static readonly BindableProperty CheckedChangedCommandProperty =
    BindableProperty.Create(
        nameof(CheckedChangedCommand),
        typeof(ICommand),
        typeof(FreakyCheckbox));

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:IntelliAbb.Xamarin.Controls.FreakyCheckbox"/> is checked.
    /// </summary>
    /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
    public ICommand CheckedChangedCommand
    {
        get { return (ICommand)GetValue(CheckedChangedCommandProperty); }
        set { SetValue(CheckedChangedCommandProperty, value); }
    }

    public static readonly BindableProperty IsCheckedProperty =
    BindableProperty.Create(
        nameof(IsChecked),
        typeof(bool),
        typeof(FreakyCheckbox),
        false,
        BindingMode.TwoWay,
        propertyChanged: OnCheckedChanged);

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:IntelliAbb.Xamarin.Controls.FreakyCheckbox"/> is checked.
    /// </summary>
    /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
    public bool IsChecked
    {
        get { return (bool)GetValue(IsCheckedProperty); }
        set { SetValue(IsCheckedProperty, value); }
    }

    static async void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is FreakyCheckbox checkbox)) return;
        checkbox.CheckedChanged?.Invoke(checkbox, new CheckedChangedEventArgs((bool)newValue));
        checkbox.CheckedChangedCommand?.ExecuteCommandIfAvailable(newValue);
        await checkbox.AnimateToggle();
    }

    public static readonly BindableProperty SizeRequestProperty =
   BindableProperty.Create(
       nameof(SizeRequest),
       typeof(double),
       typeof(FreakyCheckbox),
       size,
       propertyChanged: SizeRequestChanged);

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:IntelliAbb.Xamarin.Controls.FreakyCheckbox"/> is checked.
    /// </summary>
    /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
    public double SizeRequest
    {
        get { return (double)GetValue(SizeRequestProperty); }
        set { SetValue(SizeRequestProperty, value); }
    }

    static void SizeRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is FreakyCheckbox checkbox)) return;
        checkbox.WidthRequest = checkbox.HeightRequest = (double)(newValue);
        checkbox.skiaView.WidthRequest = checkbox.skiaView.HeightRequest = (double)(newValue);
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        skiaView.PaintSurface -= Handle_PaintSurface;
        GestureRecognizers.Clear();
    }

    #endregion
}
