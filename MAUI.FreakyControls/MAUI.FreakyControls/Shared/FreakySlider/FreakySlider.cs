namespace Maui.FreakyControls;

public class FreakySlider : Slider
{
    public static readonly BindableProperty ActiveTrackColorProperty =
        BindableProperty.Create(nameof(ActiveTrackColor), typeof(Color), typeof(FreakySlider), Colors.Black);

    public static readonly BindableProperty InactiveTrackColorProperty =
        BindableProperty.Create(nameof(InactiveTrackColor), typeof(Color), typeof(FreakySlider), Colors.Gray);

    public new static readonly BindableProperty ThumbColorProperty =
        BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(FreakySlider), Colors.Transparent);

    public static readonly BindableProperty TrackHeightProperty =
        BindableProperty.Create(nameof(TrackHeight), typeof(int), typeof(FreakySlider), 6);

    public static readonly BindableProperty TrackCornerRadiusProperty =
        BindableProperty.Create(nameof(TrackCornerRadius), typeof(int), typeof(FreakySlider), 3);

    public Color ActiveTrackColor
    {
        get => (Color)GetValue(ActiveTrackColorProperty);
        set => SetValue(ActiveTrackColorProperty, value);
    }

    public Color InactiveTrackColor
    {
        get => (Color)GetValue(InactiveTrackColorProperty);
        set => SetValue(InactiveTrackColorProperty, value);
    }

    public new Color ThumbColor
    {
        get => (Color)GetValue(ThumbColorProperty);
        set => SetValue(ThumbColorProperty, value);
    }

    public int TrackHeight
    {
        get => (int)GetValue(TrackHeightProperty);
        set => SetValue(TrackHeightProperty, value);
    }

    public int TrackCornerRadius
    {
        get => (int)GetValue(TrackCornerRadiusProperty);
        set => SetValue(TrackCornerRadiusProperty, value);
    }
}