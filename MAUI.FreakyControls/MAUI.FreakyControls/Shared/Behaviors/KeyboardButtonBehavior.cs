using System.Windows.Input;
using Maui.FreakyControls.Extensions;

namespace Maui.FreakyControls.Shared.Behaviors;

public class KeyboardButtonBehavior : BehaviorBase<Button>
{
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(KeyboardButtonBehavior),
            defaultValue: null);

    protected override void OnAttachedTo(Button bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.Clicked += Bindable_Clicked;
    }

    private void Bindable_Clicked(object sender, EventArgs e)
    {
        Command?.ExecuteCommandIfAvailable(((Button)sender).Text);
    }

    protected override void OnDetachingFrom(Button bindable)
    {
        base.OnDetachingFrom(bindable);
        bindable.Clicked -= Bindable_Clicked;
    }
}