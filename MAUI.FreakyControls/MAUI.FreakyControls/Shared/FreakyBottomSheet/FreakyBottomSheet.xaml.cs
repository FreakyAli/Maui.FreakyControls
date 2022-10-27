namespace Maui.FreakyControls;

public partial class FreakyBottomSheet : ContentView
{
    private const string defaultClearIcon = "Maui.FreakyControls.clear_icon.svg";

    public FreakyBottomSheet()
    {
        InitializeComponent();
        CloseBottomSheetButton.ResourceId = defaultClearIcon;
        CloseBottomSheetButton.SvgAssembly = typeof(FreakyBottomSheet).Assembly;
    }

    private const uint shortDuration = 250u;
    private const uint regularDuration = shortDuration * 2u;

    public IList<Microsoft.Maui.IView> BottomSheetContent => BottomSheetContentGrid.Children;

    #region Bindable Properties

    public static readonly BindableProperty SheetHeightProperty = BindableProperty.Create(
        nameof(SheetHeight),
        typeof(double),
        typeof(FreakyBottomSheet),
        360d,
        BindingMode.OneWay,
        validateValue: (_, value) => value != null);

    public double SheetHeight
    {
        get => (double)GetValue(SheetHeightProperty);
        set => SetValue(SheetHeightProperty, value);
    }

    public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(
        nameof(HeaderText),
        typeof(string),
        typeof(FreakyBottomSheet),
        string.Empty,
        BindingMode.OneWay,
        validateValue: (_, value) => value != null);

    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public static readonly BindableProperty HeaderStyleProperty = BindableProperty.Create(
        nameof(HeaderStyle),
        typeof(Style),
        typeof(FreakyBottomSheet),
        new Style(typeof(Label))
        {
            Setters =
            {
                new Setter
                {
                    Property = Label.FontSizeProperty,
                    Value = 24
                }
            }
        },
        BindingMode.OneWay,
        validateValue: (_, value) => value != null);

    public Style HeaderStyle
    {
        get => (Style)GetValue(HeaderStyleProperty);
        set => SetValue(HeaderStyleProperty, value);
    }


    #endregion

    public async Task OpenAsync()
    {
        this.InputTransparent = false;
        BackgroundFader.IsVisible = true;
        CloseBottomSheetButton.IsVisible = true;

        _ = BackgroundFader.FadeTo(1, shortDuration, Easing.SinInOut);
        await MainContent.TranslateTo(0, 0, regularDuration, Easing.SinInOut);
        _ = CloseBottomSheetButton.FadeTo(1, regularDuration, Easing.SinInOut);
    }

    public async Task CloseAsync()
    {
        await CloseBottomSheetButton.FadeTo(0, shortDuration, Easing.SinInOut);
        _ = MainContent.TranslateTo(0, SheetHeight, shortDuration, Easing.SinInOut);
        await BackgroundFader.FadeTo(0, shortDuration, Easing.SinInOut);

        BackgroundFader.IsVisible = true;
        CloseBottomSheetButton.IsVisible = true;
        this.InputTransparent = true;
    }

    async void CloseBottomSheetButton_Tapped(System.Object sender, System.EventArgs e) =>
        await CloseAsync();
}