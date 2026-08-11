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
            [nameof(IFreakyAutoCompleteView.DropDownWidth)]         = MapDropDownWidth,
            [nameof(IFreakyAutoCompleteView.DropDownHeight)]       = MapDropDownHeight,
            [nameof(IFreakyAutoCompleteView.DropDownBorderColor)]  = MapDropDownBorderColor,
            [nameof(IFreakyAutoCompleteView.DropDownBorderWidth)]  = MapDropDownBorderWidth,
            [nameof(IFreakyAutoCompleteView.DropDownCornerRadius)] = MapDropDownCornerRadius,
        };

    public static CommandMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler> CommandMapper = new(ViewCommandMapper);

    public FreakyAutoCompleteViewHandler() : base(PropertyMapper, CommandMapper) { }

    public FreakyAutoCompleteViewHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? PropertyMapper, commandMapper ?? CommandMapper) { }

    private CancellationTokenSource _cts = new();

    protected override FreakyWindowsAutoCompleteView CreatePlatformView() => new();

    protected override void ConnectHandler(FreakyWindowsAutoCompleteView platformView)
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();
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
        _cts.Cancel();
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
        {
            var text = FormatType(args.SelectedItem, VirtualView.TextMemberPath);
            sender.Text = text;
            VirtualView.Text = text;
        }
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

    public static void MapImageSource(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler._cts.Cancel();
        handler._cts.Dispose();
        handler._cts = new CancellationTokenSource();
        _ = handler.MapImageSourceAsync(view, handler._cts.Token);
    }

    private async Task MapImageSourceAsync(IFreakyAutoCompleteView view, CancellationToken token)
    {
        try
        {
            if (view.ImageSource is null)
            {
                PlatformView.SetIcon(null, 0, 0, 0, 0, null);
                return;
            }

            var imageSource = (await view.ImageSource.GetPlatformImageAsync(MauiContext!))?.Value;

            // Throws OperationCanceledException when a newer MapImageSource call has superseded this one
            // or DisconnectHandler has cancelled _cts — caught and swallowed below.
            token.ThrowIfCancellationRequested();

            if (imageSource is null) return;

            var column = view.ImageAlignment == ImageAlignment.Left ? 0 : 2;
            PlatformView.SetIcon(
                imageSource,
                view.ImageWidth,
                view.ImageHeight,
                view.ImagePadding,
                column,
                () => view.ImageCommand?.ExecuteWhenAvailable(view.ImageCommandParameter));
        }
        catch (OperationCanceledException)
        {
            // Superseded by a newer call or the handler was disconnected; discard silently.
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[FreakyAutoCompleteViewHandler] MapImageSourceAsync failed: {ex}");
        }
    }

    public static void MapDropDownWidth(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        // AutoSuggestBox sizes its popup to the control width by default; custom width not exposed natively.
    }

    public static void MapDropDownHeight(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        // WinUI 3 AutoSuggestBox does not expose MaxDropDownHeight; no-op.
    }

    public static void MapDropDownBorderColor(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.UpdateBorderStyle(handler.PlatformView);
    }

    public static void MapDropDownBorderWidth(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.UpdateBorderStyle(handler.PlatformView);
    }

    public static void MapDropDownCornerRadius(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.UpdateBorderStyle(handler.PlatformView);
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

    private void UpdateBorderStyle(FreakyWindowsAutoCompleteView platformView)
    {
        // Note: WinUI AutoSuggestBox dropdown styling is not directly exposed via public APIs.
        // The dropdown popup is internally managed and cannot be easily styled without custom
        // control templates. Border customization is available on iOS, Android, and macOS.
        // This is a platform limitation of WinUI 3.
    }
}
#endif
