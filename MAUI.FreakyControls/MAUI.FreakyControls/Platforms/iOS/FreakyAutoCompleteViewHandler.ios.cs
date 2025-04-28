using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.iOS;
using Maui.FreakyControls.Platforms.iOS.NativeControls;
using Maui.FreakyControls.Enums;
using Microsoft.Maui.Handlers;
using System.Drawing;
using UIKit;

namespace Maui.FreakyControls;

public partial class FreakyAutoCompleteViewHandler : ViewHandler<IFreakyAutoCompleteView, FreakyNativeAutoCompleteView>
{
    /// <inheritdoc />
    protected override FreakyNativeAutoCompleteView CreatePlatformView() => new FreakyNativeAutoCompleteView();

    protected override void ConnectHandler(FreakyNativeAutoCompleteView platformView)
    {
        base.ConnectHandler(platformView);
        PlatformView.Text = VirtualView.Text ?? string.Empty;
        PlatformView.Frame = new RectangleF(0, 20, 320, 50);
        UpdateTextColor(platformView);
        UpdatePlaceholder(platformView);
        UpdatePlaceholderColor(platformView);
        UpdateDisplayMemberPath(platformView);
        UpdateIsEnabled(platformView);
        UpdateFont(platformView);
        UpdateTextAlignment(platformView);
        UpdateTextTransform(platformView);
        platformView.UpdateTextOnSelect = VirtualView.UpdateTextOnSelect;
        platformView.IsSuggestionListOpen = VirtualView.IsSuggestionListOpen;
        UpdateItemsSource(platformView);
        platformView.SuggestionChosen += OnPlatformViewSuggestionChosen;
        platformView.TextChanged += OnPlatformViewTextChanged;
        platformView.QuerySubmitted += OnPlatformViewQuerySubmitted;
        PlatformView.EditingDidBegin += Control_EditingDidBegin;
        PlatformView.EditingDidEnd += Control_EditingDidEnd;
    }

    protected override void DisconnectHandler(FreakyNativeAutoCompleteView platformView)
    {
        base.DisconnectHandler(platformView);
        platformView.SuggestionChosen -= OnPlatformViewSuggestionChosen;
        platformView.TextChanged -= OnPlatformViewTextChanged;
        platformView.QuerySubmitted -= OnPlatformViewQuerySubmitted;
        platformView.EditingDidBegin -= Control_EditingDidBegin;
        platformView.EditingDidEnd -= Control_EditingDidEnd;
        platformView.Dispose();
    }

    private void OnPlatformViewSuggestionChosen(object? sender, FreakyAutoCompleteViewSuggestionChosenEventArgs e)
    {
        VirtualView?.RaiseSuggestionChosen(e);
    }

    private void OnPlatformViewTextChanged(object? sender, FreakyAutoCompleteViewTextChangedEventArgs e)
    {
        VirtualView?.NativeControlTextChanged(e);
    }

    private void OnPlatformViewQuerySubmitted(object? sender, FreakyAutoCompleteViewQuerySubmittedEventArgs e)
    {
        VirtualView?.RaiseQuerySubmitted(e);
    }

    static readonly int baseHeight = 20;

    /// <inheritdoc />
    public override Microsoft.Maui.Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
    {
        var baseResult = base.GetDesiredSize(widthConstraint, heightConstraint);
        var testString = new Foundation.NSString("Tj");
        var testSize = testString.GetSizeUsingAttributes(new UIStringAttributes { Font = PlatformView.Font });
        double height = baseHeight + testSize.Height;
        height = Math.Round(height);
        if (double.IsInfinity(widthConstraint) || double.IsInfinity(heightConstraint))
        {
            // If we drop an infinite value into base.GetDesiredSize for the Editor, we'll
            // get an exception; it doesn't know what do to with it. So instead we'll size
            // it to fit its current contents and use those values to replace infinite constraints

            PlatformView.SizeToFit();
            var sz = new Microsoft.Maui.Graphics.Size(PlatformView.Frame.Width, PlatformView.Frame.Height);
            return sz;
        }

        return base.GetDesiredSize(baseResult.Width, height);
    }

    void Control_EditingDidBegin(object sender, EventArgs e)
    {
        VirtualView.IsFocused = true;
    }

    void Control_EditingDidEnd(object sender, EventArgs e)
    {
        VirtualView.IsFocused = false;
    }

    public static void MapText(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        if (handler.PlatformView.Text != view.Text)
            handler.PlatformView.Text = view.Text;
    }

    public static void MapTextColor(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.SetTextColor(view.TextColor);
    }

    public static void MapPlaceholder(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.Placeholder = view.Placeholder;
    }

    public static void MapThreshold(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.Threshold = view.Threshold;
    }

    public static async void MapImageSource(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        var entry = handler.VirtualView;
        var uiImage = await entry.ImageSource?.ToNativeImageSourceAsync();
        if (uiImage is not null)
        {
            var uiView = uiImage.UiImageToUiView(entry.ImageHeight, entry.ImageWidth, entry.ImagePadding);
            uiView.UserInteractionEnabled = true;
            var tapGesture = new UITapGestureRecognizer(() =>
            {
                entry?.ImageCommand?.ExecuteCommandIfAvailable(entry.ImageCommandParameter);
            });
            uiView.AddGestureRecognizer(tapGesture);
            switch (entry.ImageAlignment)
            {
                case ImageAlignment.Left:
                    handler.PlatformView.InputTextField.LeftViewMode = UITextFieldViewMode.Always;
                    handler.PlatformView.InputTextField.LeftView = uiView;
                    break;

                case ImageAlignment.Right:
                    handler.PlatformView.InputTextField.RightViewMode = UITextFieldViewMode.Always;
                    handler.PlatformView.InputTextField.RightView = uiView;
                    break;
            }
        }
    }

    public static void MapAllowCopyPaste(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.InputTextField.AllowCopyPaste = view.AllowCopyPaste;
    }

    public static void MapPlaceholderColor(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.SetPlaceholderColor(view.PlaceholderColor);
    }

    public static void MapTextMemberPath(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.SetItems(view.ItemsSource?.OfType<object>(), (o) => FormatType(o, view.DisplayMemberPath), (o) => FormatType(o, view.TextMemberPath));
    }

    public static void MapDisplayMemberPath(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view.DisplayMemberPath), (o) => FormatType(o, view.TextMemberPath));
    }

    public static void MapIsSuggestionListOpen(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.IsSuggestionListOpen = view.IsSuggestionListOpen;
    }

    public static void MapUpdateTextOnSelect(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.UpdateTextOnSelect = view.UpdateTextOnSelect;
    }

    public static void MapIsEnabled(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.UserInteractionEnabled = view.IsEnabled;
    }

    public static void MapItemsSource(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    public static void MapHorizontalTextAlignment(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.InputTextField.TextAlignment = view.HorizontalTextAlignment.ToPlatform();
    }

    public static void MapVerticalTextAlignment(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.InputTextField.VerticalAlignment = view.VerticalTextAlignment.ToVPlatform();
    }

   public static void MapFontAttributes(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
   {
        // If the FontAttributes is Bold
        if (view.FontAttributes == FontAttributes.Bold)
        {
            handler.PlatformView.Font = UIFont.BoldSystemFontOfSize((nfloat)view.FontSize);
        }
        // If the FontAttributes is Italic
        else if (view.FontAttributes == FontAttributes.Italic)
        {
            handler.PlatformView.Font = UIFont.ItalicSystemFontOfSize((nfloat)view.FontSize);
        }
        // If it's neither Bold nor Italic, set to normal font
        else
        {
            handler.PlatformView.Font = UIFont.SystemFontOfSize((nfloat)view.FontSize);
        }
    }

    public static void MapFontFamily(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        var font = UIFont.FromName(view.FontFamily, (float)view.FontSize);
        handler.PlatformView.Font = font;
    }

    public static void MapFontSize(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.Font = UIFont.SystemFontOfSize((float)view.FontSize);
    }

    public static void MapTextTransform(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        if (view.TextTransform == TextTransform.Uppercase)
            handler.PlatformView.Text = handler.PlatformView.Text.ToUpper();
        else if (view.TextTransform == TextTransform.Lowercase)
            handler.PlatformView.Text = handler.PlatformView.Text.ToLower();
    }

    // Helper to convert FontAttributes to iOS font traits
    private static UIFontDescriptorSymbolicTraits UIFontDescriptorFromAttributes(FontAttributes fontAttributes)
    {
        UIFontDescriptorSymbolicTraits traits = 0;

        if (fontAttributes.HasFlag(FontAttributes.Bold))
            traits |= UIFontDescriptorSymbolicTraits.Bold;

        if (fontAttributes.HasFlag(FontAttributes.Italic))
            traits |= UIFontDescriptorSymbolicTraits.Italic;

        return traits;
    }

    private void UpdateTextColor(FreakyNativeAutoCompleteView platformView)
    {
        platformView.SetTextColor(VirtualView?.TextColor);
    }

    private void UpdateFont(FreakyNativeAutoCompleteView platformView)
    {
        //platformView.font(VirtualView?.TextColor);
    }

    private void UpdateTextAlignment(FreakyNativeAutoCompleteView platformView)
    {
    }

    private void UpdateTextTransform(FreakyNativeAutoCompleteView platformView)
    {
        
    }

    private void UpdateDisplayMemberPath(FreakyNativeAutoCompleteView platformView)
    {
        platformView.SetItems(VirtualView.ItemsSource?.OfType<object>(), (o) => FormatType(o, VirtualView.DisplayMemberPath), (o) => FormatType(o, VirtualView.TextMemberPath));
    }

    private void UpdatePlaceholderColor(FreakyNativeAutoCompleteView platformView)
    {
        platformView.SetPlaceholderColor(VirtualView?.PlaceholderColor);
    }

    private void UpdatePlaceholder(FreakyNativeAutoCompleteView platformView)
    {
        platformView.Placeholder = VirtualView?.Placeholder;
    }

    private void UpdateIsEnabled(FreakyNativeAutoCompleteView platformView)
    {
        platformView.UserInteractionEnabled = (bool)(VirtualView.IsEnabled);
    }

    private void UpdateItemsSource(FreakyNativeAutoCompleteView platformView)
    {
        platformView.SetItems(VirtualView?.ItemsSource?.OfType<object>(), (o) => FormatType(o, VirtualView?.DisplayMemberPath), (o) => FormatType(o, VirtualView?.TextMemberPath));
    }

    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
        else
            return instance?.ToString() ?? "";
    }
}