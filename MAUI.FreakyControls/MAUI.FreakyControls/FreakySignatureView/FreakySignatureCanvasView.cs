﻿using Maui.FreakyControls.Enums;
using Maui.FreakyControls.Extensions;
using System.Windows.Input;

namespace Maui.FreakyControls;

public class FreakySignatureCanvasView : View
{
    #region Events

    public event EventHandler StrokeCompleted;

    public event EventHandler Cleared;

    public event EventHandler<ImageStreamRequestedEventArgs> ImageStreamRequested;

    public event EventHandler<IsBlankRequestedEventArgs> IsBlankRequested;

    public event EventHandler<PointsEventArgs> PointsRequested;

    public event EventHandler<PointsEventArgs> PointsSpecified;

    public event EventHandler<StrokesEventArgs> StrokesRequested;

    public event EventHandler<StrokesEventArgs> StrokesSpecified;

    public event EventHandler ClearRequested;

    #endregion Events

    #region Properties& BindableProperties

    public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(
        nameof(StrokeColor),
        typeof(Color),
        typeof(FreakySignatureCanvasView),
        ImageConstructionSettings.DefaultStrokeColor);

    public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
        nameof(StrokeWidth),
        typeof(float),
        typeof(FreakySignatureCanvasView),
        ImageConstructionSettings.DefaultStrokeWidth);

    public static readonly BindableProperty ClearedCommandProperty = BindableProperty.Create(
        nameof(ClearedCommand),
        typeof(ICommand),
        typeof(FreakySignatureCanvasView),
        default(ICommand));

    public static readonly BindableProperty StrokeCompletedCommandProperty = BindableProperty.Create(
        nameof(StrokeCompletedCommand),
        typeof(ICommand),
        typeof(FreakySignatureCanvasView),
        default(ICommand));

    internal static readonly BindablePropertyKey IsBlankPropertyKey = BindableProperty.CreateReadOnly(
        nameof(IsBlank),
        typeof(bool),
        typeof(FreakySignatureCanvasView),
        true);

    public static readonly BindableProperty IsBlankProperty = IsBlankPropertyKey.BindableProperty;

    public bool IsBlank
    {
        get => RequestIsBlank();
    }

    /// <summary>
    /// Gets or sets the width of the signature strokes.
    /// </summary>
    public float StrokeWidth
    {
        get => (float)GetValue(StrokeWidthProperty);
        set => SetValue(StrokeWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the signature strokes.
    /// </summary>
    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    public IEnumerable<Point> Points
    {
        get => GetSignaturePoints();
        set => SetSignaturePoints(value);
    }

    public IEnumerable<IEnumerable<Point>> Strokes
    {
        get => GetSignatureStrokes();
        set => SetSignatureStrokes(value);
    }

    /// <summary>
    /// Gets or sets the command to be fired on clear button click
    /// </summary>
    public ICommand ClearedCommand
    {
        get => (ICommand)GetValue(ClearedCommandProperty);
        set => SetValue(ClearedCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command to be fired on stroke completed
    /// </summary>
    public ICommand StrokeCompletedCommand
    {
        get => (ICommand)GetValue(StrokeCompletedCommandProperty);
        set => SetValue(StrokeCompletedCommandProperty, value);
    }

    #endregion Properties& BindableProperties

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(1f, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified size.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Size size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(size, SizeOrScaleType.Size, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified scale.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(scale, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(1f, SizeOrScaleType.Scale, keepAspectRatio),
            StrokeColor = strokeColor
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified size with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, Size size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            DesiredSizeOrScale = new SizeOrScale(size, SizeOrScaleType.Size, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified scale with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            DesiredSizeOrScale = new SizeOrScale(scale, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, Color fillColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(1f, SizeOrScaleType.Scale, keepAspectRatio),
            StrokeColor = strokeColor,
            BackgroundColor = fillColor
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified size with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, Color fillColor, Size size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            BackgroundColor = fillColor,
            DesiredSizeOrScale = new SizeOrScale(size, SizeOrScaleType.Size, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified scale with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, Color fillColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            BackgroundColor = fillColor,
            DesiredSizeOrScale = new SizeOrScale(scale, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature using the specified settings.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat imageFormat, ImageConstructionSettings settings)
    {
        var args = new ImageStreamRequestedEventArgs(imageFormat, settings);
        ImageStreamRequested?.Invoke(this, args);
        return args.ImageStreamTask;
    }

    public void Clear()
    {
        ClearRequested?.Invoke(this, null);
    }

    private IEnumerable<Point> GetSignaturePoints()
    {
        var args = new PointsEventArgs();
        PointsRequested?.Invoke(this, args);
        return args.Points;
    }

    private void SetSignaturePoints(IEnumerable<Point> points)
    {
        PointsSpecified?.Invoke(this, new PointsEventArgs { Points = points });
    }

    private IEnumerable<IEnumerable<Point>> GetSignatureStrokes()
    {
        var args = new StrokesEventArgs();
        StrokesRequested?.Invoke(this, args);
        return args.Strokes;
    }

    private void SetSignatureStrokes(IEnumerable<IEnumerable<Point>> strokes)
    {
        StrokesSpecified?.Invoke(this, new StrokesEventArgs { Strokes = strokes });
    }

    private bool RequestIsBlank()
    {
        var args = new IsBlankRequestedEventArgs();
        IsBlankRequested?.Invoke(this, args);
        return args.IsBlank;
    }

    internal void OnStrokeCompleted()
    {
        UpdateBindableProperties();

        StrokeCompleted?.Invoke(this, EventArgs.Empty);
        StrokeCompletedCommand.ExecuteWhenAvailable();
    }

    internal void OnCleared()
    {
        UpdateBindableProperties();

        Cleared?.Invoke(this, EventArgs.Empty);
        ClearedCommand.ExecuteWhenAvailable();
    }

    private void UpdateBindableProperties()
    {
        SetValue(IsBlankPropertyKey, IsBlank);
    }
}