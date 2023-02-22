using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace Maui.FreakyControls;

public class FreakyAutoCompleteView : View
{
    public FreakyAutoCompleteView()
    {
        ItemsSource = new List<string>();
    }

    public event EventHandler<TextChangedEventArgs> TextChanged;    

    public event EventHandler Completed;

    internal void TriggerCompleted()
    {
        Completed?.Invoke(this, EventArgs.Empty);

        if (ReturnCommand?.CanExecute(ReturnCommandParameter) ?? false)
        {
            ReturnCommand?.Execute(ReturnCommandParameter);
        }
    }

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(FreakyAutoCompleteView),
        string.Empty,
        BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is FreakyAutoCompleteView view)
            {
                view.TextChanged?.Invoke(view, new TextChangedEventArgs((string)oldValue, (string)newValue));
            }
        });

    public string SelectedText { get => (string)GetValue(SelectedTextProperty); set => SetValue(SelectedTextProperty, value); }

    public static readonly BindableProperty SelectedTextProperty = BindableProperty.Create(
        nameof(SelectedText),
        typeof(string),
        typeof(FreakyAutoCompleteView),
        string.Empty,
        BindingMode.TwoWay);

    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(FreakyAutoCompleteView),
        Colors.DarkGray);

    public IList<string> ItemsSource { get => (IList<string>)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IList<string>),
            typeof(FreakyAutoCompleteView),
            null);

    public ICommand ReturnCommand { get => (ICommand)GetValue(ReturnCommandProperty); set => SetValue(ReturnCommandProperty, value); }

    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(nameof(ReturnCommand),
            typeof(ICommand),
            typeof(FreakyAutoCompleteView),
            null);

    public object ReturnCommandParameter { get => GetValue(ReturnCommandParameterProperty); set => SetValue(ReturnCommandParameterProperty, value); }

    public static readonly BindableProperty ReturnCommandParameterProperty = BindableProperty.Create(nameof(ReturnCommandParameter),
            typeof(object),
            typeof(FreakyAutoCompleteView),
            null);
}


