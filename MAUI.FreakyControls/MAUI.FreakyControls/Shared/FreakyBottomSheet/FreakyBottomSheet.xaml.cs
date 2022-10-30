using System.Windows.Input;
using Maui.FreakyControls.Extensions;
namespace Maui.FreakyControls;

public partial class FreakyBottomSheet : ContentView
{
    private const string defaultClearIcon = "Maui.FreakyControls.clear_icon.svg";
    private const uint shortDuration = 250u;
    private const uint regularDuration = shortDuration * 2u;
    private View bottomSheetContent;

    public FreakyBottomSheet()
    {
        InitializeComponent();
        this.InputTransparent = true;
        this.VerticalOptions = LayoutOptions.Fill;
        this.HorizontalOptions = LayoutOptions.Fill;
        CloseBottomSheetButton.ResourceId = defaultClearIcon;
        CloseBottomSheetButton.SvgAssembly = typeof(FreakyBottomSheet).Assembly;
    }

    #region Properties & Bindable Properties

    public View BottomSheetContent
    {
        get => bottomSheetContent;
        set
        {
            bottomSheetContent = value;
            OnPropertyChanged();
        }
    }

    public static readonly BindableProperty SheetHeightProperty = BindableProperty.Create(
        nameof(SheetHeight),
        typeof(double),
        typeof(FreakyBottomSheet),
        300d,
        BindingMode.OneWay,
        validateValue: (_, value) => value != null);

    public double SheetHeight
    {
        get => (double)GetValue(SheetHeightProperty);
        set => SetValue(SheetHeightProperty, value);
    }

    public static readonly BindableProperty SheetBackgroundColorProperty = BindableProperty.Create(
    nameof(SheetBackgroundColor),
    typeof(Color),
    typeof(FreakyBottomSheet),
    Colors.White);

    public Color SheetBackgroundColor
    {
        get => (Color)GetValue(SheetBackgroundColorProperty);
        set => SetValue(SheetBackgroundColorProperty, value);
    }

    public static readonly BindableProperty FadeBackgroundColorProperty = BindableProperty.Create(
    nameof(FadeBackgroundColor),
    typeof(Color),
    typeof(FreakyBottomSheet),
    Colors.LightGrey);

    public Color FadeBackgroundColor
    {
        get => (Color)GetValue(FadeBackgroundColorProperty);
        set => SetValue(FadeBackgroundColorProperty, value);
    }

    public static readonly BindableProperty CloseIconColorProperty = BindableProperty.Create(
    nameof(CloseIconColor),
    typeof(Color),
    typeof(FreakyBottomSheet),
    Colors.Black);

    public Color CloseIconColor
    {
        get => (Color)GetValue(CloseIconColorProperty);
        set => SetValue(CloseIconColorProperty, value);
    }

    public static readonly BindableProperty FadeOpacityProperty = BindableProperty.Create(
    nameof(FadeOpacity),
    typeof(double),
    typeof(FreakyBottomSheet),
    0d);

    public double FadeOpacity
    {
        get => (double)GetValue(FadeOpacityProperty);
        set => SetValue(FadeOpacityProperty, value);
    }

    public static readonly BindableProperty CloseCommmandProperty = BindableProperty.Create(
    nameof(CloseCommmand),
    typeof(ICommand),
    typeof(FreakyBottomSheet),
    default(ICommand));

    public ICommand CloseCommmand
    {
        get =>(ICommand) GetValue(CloseCommmandProperty);
        set => SetValue(CloseCommmandProperty, value);
    }

    public static readonly BindableProperty CloseCommmandParameterProperty = BindableProperty.Create(
    nameof(CloseCommmandParameter),
    typeof(object),
    typeof(FreakyBottomSheet),
    default(object));

    public object CloseCommmandParameter
    {
        get => (object)GetValue(CloseCommmandParameterProperty);
        set => SetValue(CloseCommmandParameterProperty, value);
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

    async void CloseBottomSheetButton_Tapped(System.Object sender, System.EventArgs e)
    {
        CloseCommmand?.ExecuteCommandIfAvailable(CloseCommmandParameter);
        await CloseAsync();
    }

    async void BackgroundFade_Tapped(System.Object sender, System.EventArgs e)
    {
        await CloseAsync();
    }
}