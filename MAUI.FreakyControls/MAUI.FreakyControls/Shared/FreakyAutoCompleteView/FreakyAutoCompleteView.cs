using System;
using System.Windows.Input;

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

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

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

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(FreakyAutoCompleteView),
            Colors.Black);

    public IList<string> ItemsSource
    {
        get => (IList<string>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IList<string>),
            typeof(FreakyAutoCompleteView),
            null);

    public ICommand ReturnCommand
    {
        get => (ICommand)GetValue(ReturnCommandProperty);
        set => SetValue(ReturnCommandProperty, value);
    }

    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
            nameof(ReturnCommand),
            typeof(ICommand),
            typeof(FreakyAutoCompleteView),
            null);

    public object ReturnCommandParameter
    {
        get => GetValue(ReturnCommandParameterProperty);
        set => SetValue(ReturnCommandParameterProperty, value);
    }

    public static readonly BindableProperty ReturnCommandParameterProperty = BindableProperty.Create(
            nameof(ReturnCommandParameter),
            typeof(object),
            typeof(FreakyAutoCompleteView),
            null);
}