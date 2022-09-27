using System.Windows.Input;

namespace Maui.FreakyControls;

public class FreakyButton : Button
{
    public ICommand LongPressedCommand
    {
        get => (ICommand)GetValue(LongPressedCommandProperty);
        set => SetValue(LongPressedCommandProperty, value);
    }

    public static readonly BindableProperty LongPressedCommandProperty = BindableProperty.Create(
        nameof(LongPressedCommand),
        typeof(ICommand),
        typeof(FreakyButton),
        default(ICommand)
        );

    public object LongPressedCommandParameter
    {
        get => GetValue(LongPressedCommandParameterProperty);
        set => SetValue(LongPressedCommandParameterProperty, value);
    }

    public static readonly BindableProperty LongPressedCommandParameterProperty = BindableProperty.Create(
        nameof(LongPressedCommandParameter),
        typeof(object),
        typeof(FreakyButton),
        default(object)
        );

    public event EventHandler<LongPressedEventArgs> LongPressed;

    internal void RaiseLongPressed()
    {
        LongPressed?.Invoke(this, new LongPressedEventArgs() { });
    }

}

