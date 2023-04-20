using System.Windows.Input;
using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Shared.Enums;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Maui.FreakyControls;

public partial class FreakyColorPicker : ContentView
{
    public FreakyColorPicker()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Occurs when the Picked Color changes
    /// </summary>
    public event EventHandler<FreakyColorPickerEventArgs> PickedColorChanged;

    public static readonly BindableProperty PickedColorChangedCommandProperty
       = BindableProperty.Create(
           nameof(PickedColorChangedCommand),
           typeof(ICommand),
           typeof(FreakyColorPicker));

    /// <summary>
    /// Get the current Picked Color
    /// </summary>
    public ICommand PickedColorChangedCommand
    {
        get { return (ICommand)GetValue(PickedColorChangedCommandProperty); }
        set { SetValue(PickedColorChangedCommandProperty, value); }
    }

    public static readonly BindableProperty PickedColorProperty
        = BindableProperty.Create(
            nameof(PickedColor),
            typeof(Color),
            typeof(FreakyColorPicker));

    /// <summary>
    /// Get the current Picked Color
    /// </summary>
    public Color PickedColor
    {
        get { return (Color)GetValue(PickedColorProperty); }
        private set { SetValue(PickedColorProperty, value); }
    }

    public static readonly BindableProperty PointerColorProperty
        = BindableProperty.Create(
            nameof(PointerColor),
            typeof(Color),
            typeof(FreakyColorPicker),
            Colors.White);

    /// <summary>
    /// Get the current Picked Color
    /// </summary>
    public Color PointerColor
    {
        get { return (Color)GetValue(PointerColorProperty); }
        set { SetValue(PointerColorProperty, value); }
    }

    public static readonly BindableProperty GradientColorStyleProperty
        = BindableProperty.Create(
            nameof(GradientColorStyle),
            typeof(GradientColorStyle),
            typeof(FreakyColorPicker),
            GradientColorStyle.ColorsToDarkStyle,
            BindingMode.OneTime, null);

    /// <summary>
    /// Set the Color Spectrum Gradient Style
    /// </summary>
    public GradientColorStyle GradientColorStyle
    {
        get { return (GradientColorStyle)GetValue(GradientColorStyleProperty); }
        set { SetValue(GradientColorStyleProperty, value); }
    }


    public static readonly BindableProperty ColorListProperty
        = BindableProperty.Create(
            nameof(ColorList),
            typeof(string[]),
            typeof(FreakyColorPicker),
            new string[]
            {
                    new Color(255, 0, 0).ToHex(), // Red
					new Color(255, 255, 0).ToHex(), // Yellow
					new Color(0, 255, 0).ToHex(), // Lime Green
					new Color(0, 255, 255).ToHex(), // Aqua
					new Color(0, 0, 255).ToHex(), // Blue
					new Color(255, 0, 255).ToHex(), // Fuchsia
					new Color(255, 0, 0).ToHex(), // Red
            },
            BindingMode.OneTime, null);

    /// <summary>
    /// Sets the Color List
    /// </summary>
    public string[] ColorList
    {
        get { return (string[])GetValue(ColorListProperty); }
        set { SetValue(ColorListProperty, value); }
    }


    public static readonly BindableProperty ColorListDirectionProperty
        = BindableProperty.Create(
            nameof(ColorDirection),
            typeof(ColorDirection),
            typeof(FreakyColorPicker),
            ColorDirection.Horizontal,
            BindingMode.OneTime);

    /// <summary>
    /// Sets the Color List flow Direction
    /// </summary>
    public ColorDirection ColorDirection
    {
        get { return (ColorDirection)GetValue(ColorListDirectionProperty); }
        set { SetValue(ColorListDirectionProperty, value); }
    }


    public static readonly BindableProperty PointerCircleDiameterUnitsProperty
         = BindableProperty.Create(
             nameof(PointerCircleDiameterUnits),
             typeof(double),
             typeof(FreakyColorPicker),
             0.6,
             BindingMode.OneTime);

    /// <summary>
    /// Sets the Picker Pointer Size
    /// Value must be between 0-1
    /// Calculated against the View Canvas size
    /// </summary>
    public double PointerCircleDiameterUnits
    {
        get { return (double)GetValue(PointerCircleDiameterUnitsProperty); }
        set { SetValue(PointerCircleDiameterUnitsProperty, value); }
    }


    public static readonly BindableProperty PointerCircleBorderUnitsProperty
        = BindableProperty.Create(
            nameof(PointerCircleBorderUnits),
            typeof(double),
            typeof(FreakyColorPicker),
            0.3,
            BindingMode.OneTime);

    /// <summary>
    /// Sets the Picker Pointer Border Size
    /// Value must be between 0-1
    /// Calculated against pixel size of Picker Pointer
    /// </summary>
    public double PointerCircleBorderUnits
    {
        get { return (double)GetValue(PointerCircleBorderUnitsProperty); }
        set { SetValue(PointerCircleBorderUnitsProperty, value); }
    }


    public static readonly BindableProperty PointerPositionXProperty
       = BindableProperty.Create(
           nameof(PointerPositionX),
           typeof(double),
           typeof(FreakyColorPicker),
           0.0,
           propertyChanged: PointerPositionChanged
           );

    public double PointerPositionX
    {
        get { return (double)GetValue(PointerPositionXProperty); }
        set { SetValue(PointerPositionXProperty, value); }
    }

    public static readonly BindableProperty PointerPositionYProperty
        = BindableProperty.Create(
           nameof(PointerPositionY),
           typeof(double),
           typeof(FreakyColorPicker),
           0.0,
           propertyChanged: PointerPositionChanged
           );

    public double PointerPositionY
    {
        get { return (double)GetValue(PointerPositionYProperty); }
        set { SetValue(PointerPositionYProperty, value); }
    }

    private static void PointerPositionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var freakyColorPicker = (FreakyColorPicker)bindable;
        freakyColorPicker.SetPointer(freakyColorPicker.PointerPositionX, freakyColorPicker.PointerPositionY);
    }

    private void SetPointer(double xPositionUnits, double yPositionUnits)
    {
        // Calculate actual X Position
        var xPosition = SkCanvasView.CanvasSize.Width
                                 * xPositionUnits;
        // Calculate actual Y Position
        var yPosition = SkCanvasView.CanvasSize.Height
                                 * yPositionUnits;

        // Update as last touch Position on Canvas
        _lastTouchPoint = new SKPoint(Convert.ToSingle(xPosition), Convert.ToSingle(yPosition));
        SkCanvasView.InvalidateSurface();
    }

    private SKPoint _lastTouchPoint = new SKPoint();
    private bool _checkPointerInitPositionDone = false;

    private void SkCanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var skImageInfo = e.Info;
        var skSurface = e.Surface;
        var skCanvas = skSurface.Canvas;

        var skCanvasWidth = skImageInfo.Width;
        var skCanvasHeight = skImageInfo.Height;

        skCanvas.Clear(SKColors.White);

        // Draw gradient rainbow Color spectrum
        using (var paint = new SKPaint())
        {
            paint.IsAntialias = true;

            System.Collections.Generic.List<SKColor> colors = new System.Collections.Generic.List<SKColor>();
            foreach (var color in ColorList)
            {
                var skColor = Color.FromArgb(color).ToSKColor();
                colors.Add(skColor);
            }

            // create the gradient shader between Colors
            using (var shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                ColorDirection == ColorDirection.Horizontal ?
                    new SKPoint(skCanvasWidth, 0) : new SKPoint(0, skCanvasHeight),
                colors.ToArray(),
                null,
                SKShaderTileMode.Clamp))
            {
                paint.Shader = shader;
                skCanvas.DrawPaint(paint);
            }
        }

        // Draw darker gradient spectrum
        using (var paint = new SKPaint())
        {
            paint.IsAntialias = true;

            // Initiate the darkened primary color list
            var colors = GetGradientOrder();

            // create the gradient shader 
            using (var shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                ColorDirection == ColorDirection.Horizontal ?
                    new SKPoint(0, skCanvasHeight) : new SKPoint(skCanvasWidth, 0),
                colors,
                null,
                SKShaderTileMode.Clamp))
            {
                paint.Shader = shader;
                skCanvas.DrawPaint(paint);
            }
        }

        if (!_checkPointerInitPositionDone)
        {
            var x = ((float)skCanvasWidth * (float)PointerPositionX);
            var y = ((float)skCanvasHeight * (float)PointerPositionY);

            _lastTouchPoint = new SKPoint(x, y);

            _checkPointerInitPositionDone = true;
        }

        // Picking the Pixel Color values on the Touch Point

        // Represent the color of the current Touch point
        SKColor touchPointColor;

        // Efficient and fast
        // https://forums.xamarin.com/discussion/92899/read-a-pixel-info-from-a-canvas
        // create the 1x1 bitmap (auto allocates the pixel buffer)
        using (SKBitmap bitmap = new SKBitmap(skImageInfo))
        {
            // get the pixel buffer for the bitmap
            IntPtr dstpixels = bitmap.GetPixels();

            // read the surface into the bitmap
            skSurface.ReadPixels(skImageInfo,
                dstpixels,
                skImageInfo.RowBytes,
                (int)_lastTouchPoint.X, (int)_lastTouchPoint.Y);

            // access the color
            touchPointColor = bitmap.GetPixel(0, 0);
        }

        // Painting the Touch point
        using (SKPaint paintTouchPoint = new SKPaint())
        {
            paintTouchPoint.Style = SKPaintStyle.Fill;
            paintTouchPoint.Color = PointerColor.ToSKColor();
            paintTouchPoint.IsAntialias = true;

            var valueToCalcAgainst = (skCanvasWidth > skCanvasHeight) ? skCanvasWidth : skCanvasHeight;

            var pointerCircleDiameterUnits = PointerCircleDiameterUnits; // 0.6 (Default)
            pointerCircleDiameterUnits = (float)pointerCircleDiameterUnits / 10f; //  calculate 1/10th of that value
            var pointerCircleDiameter = (float)(valueToCalcAgainst * pointerCircleDiameterUnits);

            // Outer circle of the Pointer (Ring)
            skCanvas.DrawCircle(
                _lastTouchPoint.X,
                _lastTouchPoint.Y,
                pointerCircleDiameter / 2, paintTouchPoint);

            // Draw another circle with picked color
            paintTouchPoint.Color = touchPointColor;

            var pointerCircleBorderWidthUnits = PointerCircleBorderUnits; // 0.3 (Default)
            var pointerCircleBorderWidth = (float)pointerCircleDiameter *
                                                    (float)pointerCircleBorderWidthUnits; // Calculate against Pointer Circle

            // Inner circle of the Pointer (Ring)
            skCanvas.DrawCircle(
                _lastTouchPoint.X,
                _lastTouchPoint.Y,
                ((pointerCircleDiameter - pointerCircleBorderWidth) / 2), paintTouchPoint);
        }

        // Set selected color
        PickedColor = touchPointColor.ToMauiColor();
        var eventArgs = new FreakyColorPickerEventArgs { Color = this.PickedColor, PointerX = this.PointerPositionX, PointerY = this.PointerPositionY };
        PickedColorChanged?.Invoke(this, eventArgs);
        PickedColorChangedCommand?.ExecuteCommandIfAvailable(eventArgs);
    }

    private void SkCanvasView_OnTouch(object sender, SKTouchEventArgs e)
    {
        _lastTouchPoint = e.Location;

        var canvasSize = SkCanvasView.CanvasSize;

        // Check for each touch point XY position to be inside Canvas
        // Ignore any Touch event ocurred outside the Canvas region 
        if ((e.Location.X > 0 && e.Location.X < canvasSize.Width) &&
            (e.Location.Y > 0 && e.Location.Y < canvasSize.Height))
        {
            e.Handled = true;

            // update the Canvas as you wish
            SkCanvasView.InvalidateSurface();
        }
    }

    private SKColor[] GetGradientOrder()
    {
        if (GradientColorStyle == GradientColorStyle.ColorsOnlyStyle)
        {
            return new SKColor[]
            {
                SKColors.Transparent
            };
        }
        else if (GradientColorStyle == GradientColorStyle.ColorsToDarkStyle)
        {
            return new SKColor[]
            {
                SKColors.Transparent,
                SKColors.Black
            };
        }
        else if (GradientColorStyle == GradientColorStyle.DarkToColorsStyle)
        {
            return new SKColor[]
            {
                SKColors.Black,
                SKColors.Transparent
            };
        }
        else if (GradientColorStyle == GradientColorStyle.ColorsToLightStyle)
        {
            return new SKColor[]
            {
                SKColors.Transparent,
                SKColors.White
            };
        }
        else if (GradientColorStyle == GradientColorStyle.LightToColorsStyle)
        {
            return new SKColor[]
            {
                SKColors.White,
                SKColors.Transparent
            };
        }
        else if (GradientColorStyle == GradientColorStyle.LightToColorsToDarkStyle)
        {
            return new SKColor[]
            {
                SKColors.White,
                SKColors.Transparent,
                SKColors.Black
            };
        }
        else if (GradientColorStyle == GradientColorStyle.DarkToColorsToLightStyle)
        {
            return new SKColor[]
            {
                SKColors.Black,
                SKColors.Transparent,
                SKColors.White
            };
        }
        else
        {
            return new SKColor[]
            {
                SKColors.Transparent,
                SKColors.Black
            };
        }
    }
}