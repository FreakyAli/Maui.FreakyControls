using Microsoft.Maui.Handlers;
#if WINDOWS
using Maui.FreakyControls.Enums;
using Maui.FreakyControls.Platforms.Windows.NativeControls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI.Text;
using MauiTextAlignment = Microsoft.Maui.TextAlignment;
using WinHorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment;
using WinVerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment;
#endif

namespace Maui.FreakyControls;

#if WINDOWS
public partial class FreakyAutoCompleteViewHandler : ViewHandler<IFreakyAutoCompleteView, FreakyWindowsAutoCompleteView>
{
    public static IPropertyMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler> PropertyMapper =
        new PropertyMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler>(ViewHandler.ViewMapper)
        {
            [nameof(IFreakyAutoCompleteView.Text)]                  = MapText,
            [nameof(IFreakyAutoCompleteView.TextColor)]             = MapTextColor,
            [nameof(IFreakyAutoCompleteView.Placeholder)]           = MapPlaceholder,
            [nameof(IFreakyAutoCompleteView.PlaceholderColor)]      = MapPlaceholderColor,
            [nameof(IFreakyAutoCompleteView.TextMemberPath)]        = MapTextMemberPath,
            [nameof(IFreakyAutoCompleteView.DisplayMemberPath)]     = MapDisplayMemberPath,
            [nameof(IFreakyAutoCompleteView.IsEnabled)]             = MapIsEnabled,
            [nameof(IFreakyAutoCompleteView.ItemsSource)]           = MapItemsSource,
            [nameof(IFreakyAutoCompleteView.UpdateTextOnSelect)]    = MapUpdateTextOnSelect,
            [nameof(IFreakyAutoCompleteView.IsSuggestionListOpen)]  = MapIsSuggestionListOpen,
            [nameof(IFreakyAutoCompleteView.Threshold)]             = MapThreshold,
            [nameof(IFreakyAutoCompleteView.ImageAlignment)]        = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImageCommand)]          = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImageCommandParameter)] = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImageHeight)]           = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImagePadding)]          = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImageSource)]           = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImageWidth)]            = MapImageSource,
            [nameof(IFreakyAutoCompleteView.AllowCopyPaste)]        = MapAllowCopyPaste,
            [nameof(IFreakyAutoCompleteView.HorizontalTextAlignment)] = MapTextAlignment,
            [nameof(IFreakyAutoCompleteView.VerticalTextAlignment)]   = MapTextAlignment,
            [nameof(IFreakyAutoCompleteView.FontFamily)]            = MapFont,
            [nameof(IFreakyAutoCompleteView.FontSize)]              = MapFont,
            [nameof(IFreakyAutoCompleteView.FontAttributes)]        = MapFont,
            [nameof(IFreakyAutoCompleteView.SuggestionListWidth)]   = MapSuggestionListWidth,
            [nameof(IFreakyAutoCompleteView.SuggestionListHeight)]  = MapSuggestionListHeight,
        };

    public static CommandMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler> CommandMapper = new(ViewCommandMapper);

    public FreakyAutoCompleteViewHandler() : base(PropertyMapper, CommandMapper) { }

    public FreakyAutoCompleteViewHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? PropertyMapper, commandMapper ?? CommandMapper) { }

    protected override FreakyWindowsAutoCompleteView CreatePlatformView() => new();

    protected override void ConnectHandler(FreakyWindowsAutoCompleteView platformView)
    {
        base.ConnectHandler(platformView);
        var asb = platformView.AutoSuggestBox;

        asb.Text = VirtualView.Text ?? string.Empty;
        asb.PlaceholderText = VirtualView.Placeholder ?? string.Empty;
        asb.IsSuggestionListOpen = VirtualView.IsSuggestionListOpen;
        asb.IsEnabled = VirtualView.IsEnabled;
        UpdateItemsSource(platformView);
        MapFont(this, VirtualView);
        MapTextAlignment(this, VirtualView);

        asb.TextChanged += OnTextChanged;
        asb.SuggestionChosen += OnSuggestionChosen;
        asb.QuerySubmitted += OnQuerySubmitted;
    }

    protected override void DisconnectHandler(FreakyWindowsAutoCompleteView platformView)
    {
        var asb = platformView.AutoSuggestBox;
        asb.TextChanged -= OnTextChanged;
        asb.SuggestionChosen -= OnSuggestionChosen;
        asb.QuerySubmitted -= OnQuerySubmitted;
        base.DisconnectHandler(platformView);
    }

    // ── Event forwarding ───────────────────────────────────────────────────

    private void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            VirtualView?.NativeControlTextChanged(new FreakyAutoCompleteViewTextChangedEventArgs(sender.Text, TextChangeReason.UserInput));
    }

    private void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (VirtualView?.UpdateTextOnSelect == true)
            sender.Text = FormatType(args.SelectedItem, VirtualView.TextMemberPath);
        VirtualView?.RaiseSuggestionChosen(new FreakyAutoCompleteViewSuggestionChosenEventArgs(args.SelectedItem));
    }

    private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        VirtualView?.RaiseQuerySubmitted(new FreakyAutoCompleteViewQuerySubmittedEventArgs(args.QueryText, args.ChosenSuggestion));
    }

    // ── Map methods ────────────────────────────────────────────────────────

    public static void MapText(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        var asb = handler.PlatformView.AutoSuggestBox;
        if (asb.Text != view.Text)
            asb.Text = view.Text ?? string.Empty;
    }

    public static void MapTextColor(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        // WinUI 3 AutoSuggestBox text color is theme-controlled; direct override requires a control template.
    }

    public static void MapPlaceholder(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.AutoSuggestBox.PlaceholderText = view.Placeholder ?? string.Empty;
    }

    public static void MapPlaceholderColor(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        // WinUI 3 AutoSuggestBox placeholder color is theme-controlled; direct override requires a control template.
    }

    public static void MapThreshold(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        // AutoSuggestBox surfaces suggestions on any input by default; Threshold is not a native concept.
    }

    public static void MapIsEnabled(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.AutoSuggestBox.IsEnabled = view.IsEnabled;
    }

    public static void MapItemsSource(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.UpdateItemsSource(handler.PlatformView);
    }

    public static void MapTextMemberPath(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.UpdateItemsSource(handler.PlatformView);
    }

    public static void MapDisplayMemberPath(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.UpdateItemsSource(handler.PlatformView);
    }

    public static void MapUpdateTextOnSelect(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        // Handled inside OnSuggestionChosen.
    }

    public static void MapIsSuggestionListOpen(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.AutoSuggestBox.IsSuggestionListOpen = view.IsSuggestionListOpen;
    }

    public static void MapAllowCopyPaste(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        var asb = handler.PlatformView.AutoSuggestBox;
        if (!view.AllowCopyPaste)
            asb.ContextFlyout = null;
        else
            asb.ClearValue(AutoSuggestBox.ContextFlyoutProperty);
    }

    public static void MapTextAlignment(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        var asb = handler.PlatformView.AutoSuggestBox;
        asb.HorizontalContentAlignment = view.HorizontalTextAlignment switch
        {
            MauiTextAlignment.Center => WinHorizontalAlignment.Center,
            MauiTextAlignment.End    => WinHorizontalAlignment.Right,
            _                        => WinHorizontalAlignment.Left,
        };
        asb.VerticalContentAlignment = view.VerticalTextAlignment switch
        {
            MauiTextAlignment.Start => WinVerticalAlignment.Top,
            MauiTextAlignment.End   => WinVerticalAlignment.Bottom,
            _                       => WinVerticalAlignment.Center,
        };
    }

    public static void MapFont(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        var asb = handler.PlatformView.AutoSuggestBox;

        if (!string.IsNullOrEmpty(view.FontFamily))
            asb.FontFamily = new Microsoft.UI.Xaml.Media.FontFamily(view.FontFamily);

        if (view.FontSize > 0)
            asb.FontSize = view.FontSize;

        asb.FontStyle = view.FontAttributes.HasFlag(FontAttributes.Italic)
            ? Windows.UI.Text.FontStyle.Italic
            : Windows.UI.Text.FontStyle.Normal;

        asb.FontWeight = view.FontAttributes.HasFlag(FontAttributes.Bold)
            ? Microsoft.UI.Text.FontWeights.Bold
            : Microsoft.UI.Text.FontWeights.Normal;
    }

    public static async void MapImageSource(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        if (view.ImageSource is null)
        {
            handler.PlatformView.SetIcon(null, 0, 0, 0, 0, null);
            return;
        }

        var imageSource = await view.ImageSource.GetPlatformImageAsync(handler.MauiContext!);

        if (imageSource is null) return;

        var column = view.ImageAlignment == ImageAlignment.Left ? 0 : 2;
        handler.PlatformView.SetIcon(
            imageSource,
            view.ImageWidth,
            view.ImageHeight,
            view.ImagePadding,
            column,
            () => view.ImageCommand?.ExecuteWhenAvailable(view.ImageCommandParameter));
    }

    public static void MapSuggestionListWidth(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        // AutoSuggestBox sizes its popup to the control width by default; custom width not exposed natively.
    }

    public static void MapSuggestionListHeight(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        // WinUI 3 AutoSuggestBox does not expose MaxDropDownHeight; no-op.
    }

    // ── Helpers ────────────────────────────────────────────────────────────

    private void UpdateItemsSource(FreakyWindowsAutoCompleteView platformView)
    {
        var asb = platformView.AutoSuggestBox;
        if (VirtualView?.ItemsSource is null)
        {
            asb.ItemsSource = null;
            return;
        }

        asb.ItemsSource = VirtualView.ItemsSource
            .OfType<object>()
            .Select(o => FormatType(o, VirtualView.DisplayMemberPath))
            .ToList();
    }

    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? string.Empty;
        return instance?.ToString() ?? string.Empty;
    }
}
#endif
