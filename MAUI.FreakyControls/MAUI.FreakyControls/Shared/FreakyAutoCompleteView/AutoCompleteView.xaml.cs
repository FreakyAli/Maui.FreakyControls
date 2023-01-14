using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows.Input;
using Maui.FreakyControls.Extensions;
using Grid = Microsoft.Maui.Controls.Grid;
using StackLayout = Microsoft.Maui.Controls.StackLayout;

namespace Maui.FreakyControls;

public partial class AutoCompleteView : ContentView
{
    private const int RowHeight = 60;

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(AutoCompleteView),
        defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(AutoCompleteView),
        Colors.Black);

    public static readonly BindableProperty MaximumVisibleElementsProperty = BindableProperty.Create(
        nameof(MaximumVisibleElements),
        typeof(int),
        typeof(AutoCompleteView),
        4);

    public static readonly BindableProperty MinimumPrefixCharacterProperty = BindableProperty.Create(
        nameof(MinimumPrefixCharacter),
        typeof(int),
        typeof(AutoCompleteView),
        1);

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder),
        typeof(string),
        typeof(AutoCompleteView),
        string.Empty);

    public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
        nameof(PlaceholderColor),
        typeof(Color),
        typeof(AutoCompleteView),
        Colors.DarkGray);

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource),
        typeof(IEnumerable),
        typeof(AutoCompleteView),
        null);


    private readonly IEnumerable _originSuggestions = Array.Empty<object>();

    public AutoCompleteView()
    {
        InitializeComponent();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public int MaximumVisibleElements
    {
        get => (int)GetValue(MaximumVisibleElementsProperty);
        set => SetValue(MaximumVisibleElementsProperty, value);
    }

    public int MinimumPrefixCharacter
    {
        get => (int)GetValue(MinimumPrefixCharacterProperty);
        set => SetValue(MinimumPrefixCharacterProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public Color PlaceholderColor
    {
        get => (Color)GetValue(PlaceholderColorProperty);
        set => SetValue(PlaceholderColorProperty, value);
    }

    void FreakyEntry_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
    }
}