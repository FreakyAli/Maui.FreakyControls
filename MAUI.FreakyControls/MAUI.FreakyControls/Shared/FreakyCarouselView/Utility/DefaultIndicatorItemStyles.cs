namespace Maui.FreakyControls.Utility;

public static class DefaultIndicatorItemStyles
{
    private static Style _defaultSelectedIndicatorItemStyle;
    private static Style _defaultUnselectedIndicatorItemStyle;

    static DefaultIndicatorItemStyles()
    {
    }

    public static Style DefaultSelectedIndicatorItemStyle
        => _defaultSelectedIndicatorItemStyle ??= new Style(typeof(Frame))
        {
            Setters = {
                new Setter { Property = VisualElement.BackgroundColorProperty, Value = Colors.White.MultiplyAlpha(.8f) }
            }
        };

    public static Style DefaultUnselectedIndicatorItemStyle
        => _defaultUnselectedIndicatorItemStyle ??= new Style(typeof(Frame))
        {
            Setters = {
                new Setter { Property = VisualElement.BackgroundColorProperty, Value = Colors.Transparent },
                new Setter { Property = Frame.BorderColorProperty, Value = Colors.White.MultiplyAlpha(.8f) }
            }
        };
}