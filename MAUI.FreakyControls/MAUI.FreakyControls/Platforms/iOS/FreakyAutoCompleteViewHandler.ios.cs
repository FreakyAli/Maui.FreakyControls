using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.iOS;
using Maui.FreakyControls.Platforms.iOS.NativeControls;
using Maui.FreakyControls.Enums;
using Microsoft.Maui.Handlers;
using System.Drawing;
using UIKit;
using Foundation;

namespace Maui.FreakyControls;

public partial class FreakyAutoCompleteViewHandler : ViewHandler<IFreakyAutoCompleteView, FreakyNativeAutoCompleteView>
{
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
        platformView.UpdateTextOnSelect = VirtualView.UpdateTextOnSelect;
        platformView.IsSuggestionListOpen = VirtualView.IsSuggestionListOpen;
        UpdateItemsSource(platformView);
        UpdateSuggestionListWidth(platformView);
        UpdateSuggestionListHeight(platformView);
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

    static readonly int baseHeight = 20;

    public override Microsoft.Maui.Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
    {
        var baseResult = base.GetDesiredSize(widthConstraint, heightConstraint);
        var testString = new NSString("Tj");
        var testSize = testString.GetSizeUsingAttributes(new UIStringAttributes { Font = PlatformView.Font });
        double height = baseHeight + testSize.Height;
        height = Math.Round(height);

        if (double.IsInfinity(widthConstraint) || double.IsInfinity(heightConstraint))
        {
            PlatformView.SizeToFit();
            return new Microsoft.Maui.Graphics.Size(PlatformView.Frame.Width, PlatformView.Frame.Height);
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
                entry?.ImageCommand?.ExecuteWhenAvailable(entry.ImageCommandParameter);
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

    public static void MapTextAlignment(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.UpdateTextAlignment(handler?.PlatformView);
    }

    public static void MapFont(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.UpdateFont(handler?.PlatformView);
    }

    public static void MapSuggestionListWidth(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.UpdateSuggestionListWidth(handler.PlatformView);
    }

    public static void MapSuggestionListHeight(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.UpdateSuggestionListHeight(handler.PlatformView);
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
        handler.PlatformView?.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view.DisplayMemberPath), (o) => FormatType(o, view.TextMemberPath));
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

    private void UpdateTextColor(FreakyNativeAutoCompleteView platformView)
    {
        platformView.SetTextColor(VirtualView?.TextColor);
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

    public void UpdateSuggestionListWidth(FreakyNativeAutoCompleteView platformView)
    {
        if(platformView == null || VirtualView == null)
        return;
        platformView.SuggestionListWidth =(float)VirtualView.SuggestionListWidth;
    }

    public void UpdateSuggestionListHeight(FreakyNativeAutoCompleteView platformView)
    {
        if(platformView == null || VirtualView == null)
        return;
        platformView.SuggestionListHeight =(float)VirtualView.SuggestionListHeight;
    }

    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
        else
            return instance?.ToString() ?? "";
    }

    private void UpdateFont(FreakyNativeAutoCompleteView platformView)
    {
        if(platformView == null)
            return;
            
        var fontSize = (nfloat)(VirtualView?.FontSize ?? 14);
        if (!string.IsNullOrEmpty(VirtualView?.FontFamily))
        {
            platformView.Font = UIFont.FromName(VirtualView.FontFamily, fontSize);
        }
        else
        {
            platformView.Font = VirtualView.FontAttributes switch
            {
                FontAttributes.Bold => UIFont.BoldSystemFontOfSize(fontSize),
                FontAttributes.Italic => UIFont.ItalicSystemFontOfSize(fontSize),
                _ => UIFont.SystemFontOfSize(fontSize),
            };
        }
    }

    private void UpdateTextAlignment(FreakyNativeAutoCompleteView platformView)
    {
        if (VirtualView == null)
            return;

        switch (VirtualView.HorizontalTextAlignment)
        {
            case TextAlignment.Center:
                platformView.InputTextField.TextAlignment = UITextAlignment.Center;
                break;
            case TextAlignment.End:
                platformView.InputTextField.TextAlignment = UITextAlignment.Right;
                break;
            case TextAlignment.Start:
            default:
                platformView.InputTextField.TextAlignment = UITextAlignment.Left;
                break;
        }

        switch (VirtualView.VerticalTextAlignment)
        {
            case Microsoft.Maui.TextAlignment.Start:
                platformView.InputTextField.VerticalAlignment = UIControlContentVerticalAlignment.Top;
                break;
            case Microsoft.Maui.TextAlignment.Center:
                platformView.InputTextField.VerticalAlignment = UIControlContentVerticalAlignment.Center;
                break;
            case Microsoft.Maui.TextAlignment.End:
                platformView.InputTextField.VerticalAlignment    = UIControlContentVerticalAlignment.Bottom;
                break;
        }
    }
}
