namespace Maui.FreakyControls;

public class FreakySwitch : Switch
{
    public static readonly BindableProperty OutlineColorProperty =
   BindableProperty.Create(
       nameof(OutlineColor),
       typeof(Color),
       typeof(FreakySwitch),
       Colors.Black);

    /// <summary>
    /// Gets or sets the color of the switch's outline(Android only).
    /// </summary>
    /// <value>Color value of the outline</value>
    public Color OutlineColor
    {
        get { return (Color)GetValue(OutlineColorProperty); }
        set { SetValue(OutlineColorProperty, value); }
    }
}