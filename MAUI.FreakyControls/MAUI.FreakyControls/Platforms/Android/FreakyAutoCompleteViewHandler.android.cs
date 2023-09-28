using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.Core.View;
using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.Android.NativeControls;
using Maui.FreakyControls.Shared.Enums;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using static Microsoft.Maui.ApplicationModel.Platform;

namespace Maui.FreakyControls;

public partial class FreakyAutoCompleteViewHandler : ViewHandler<IFreakyAutoCompleteView, FreakyNativeAutoCompleteView>
{
    protected override FreakyNativeAutoCompleteView CreatePlatformView()
    {
        var _nativeView = new FreakyNativeAutoCompleteView(this.Context);
        var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        ViewCompat.SetBackgroundTintList(_nativeView, colorStateList);
        return _nativeView;
    }

    protected override void ConnectHandler(FreakyNativeAutoCompleteView platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SuggestionChosen += OnPlatformViewSuggestionChosen;
        platformView.TextChanged += OnPlatformViewTextChanged;
        platformView.QuerySubmitted += OnPlatformViewQuerySubmitted;
        platformView.SetTextColor(VirtualView?.TextColor.ToPlatform() ?? VirtualView.TextColor.ToPlatform());
        UpdateTextColor(platformView);
        UpdatePlaceholder(platformView);
    }

    protected override void DisconnectHandler(FreakyNativeAutoCompleteView platformView)
    {
        platformView.SuggestionChosen -= OnPlatformViewSuggestionChosen;
        platformView.TextChanged -= OnPlatformViewTextChanged;
        platformView.QuerySubmitted -= OnPlatformViewQuerySubmitted;

        platformView.Dispose();
        base.DisconnectHandler(platformView);
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
        handler.PlatformView?.SetTextColor(view.TextColor.ToPlatform());
    }

    public static void MapPlaceholder(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.Placeholder = view.Placeholder;
    }

    public static void MapThreshold(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.Threshold = view.Threshold;
    }

    public static void MapAllowCopyPaste(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        if (view.AllowCopyPaste)
        {
            handler.PlatformView.CustomInsertionActionModeCallback = null;
            handler.PlatformView.CustomSelectionActionModeCallback = null;
        }
        else
        {
            handler.PlatformView.CustomInsertionActionModeCallback = new CustomInsertionActionModeCallback();
            handler.PlatformView.CustomSelectionActionModeCallback = new CustomSelectionActionModeCallback();
        }
    }

    public static async void MapImageSource(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        var entry = handler.VirtualView;
        var imageBitmap = await entry.ImageSource?.ToNativeImageSourceAsync();
        if (imageBitmap != null)
        {
            var bitmapDrawable = new BitmapDrawable(CurrentActivity.Resources,
                Bitmap.CreateScaledBitmap(imageBitmap, entry.ImageWidth * 2, entry.ImageHeight * 2, true));
            var freakyEditText = (handler.PlatformView as FreakyNativeAutoCompleteView);
            freakyEditText.SetDrawableClickListener(new DrawableHandlerCallback(entry));
            switch (entry.ImageAlignment)
            {
                case ImageAlignment.Left:
                    freakyEditText.SetCompoundDrawablesWithIntrinsicBounds(bitmapDrawable, null, null, null);
                    break;

                case ImageAlignment.Right:
                    freakyEditText.SetCompoundDrawablesWithIntrinsicBounds(null, null, bitmapDrawable, null);
                    break;
            }
        }
        handler.PlatformView.CompoundDrawablePadding = entry.ImagePadding;
    }

    public static void MapKeyboard(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.UpdateKeyboard(view);
    }

    public static void MapCharacterSpacing(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.UpdateCharacterSpacing(view);
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
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
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
        handler.PlatformView.Enabled = view.IsEnabled;
    }

    public static void MapItemsSource(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    private void UpdateTextColor(FreakyNativeAutoCompleteView platformView)
    {
        var color = VirtualView?.TextColor;
        platformView.SetTextColor(color.ToPlatform());
    }

    private void UpdateDisplayMemberPath(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    private void UpdatePlaceholderColor(FreakyNativeAutoCompleteView platformView)
    {
        var placeholderColor = VirtualView?.PlaceholderColor;
        platformView.SetPlaceholderColor(placeholderColor);
    }

    private void UpdatePlaceholder(FreakyNativeAutoCompleteView platformView) => platformView.Placeholder = VirtualView?.Placeholder;

    private void UpdateIsEnabled(FreakyNativeAutoCompleteView platformView)
    {
        platformView.Enabled = (bool)(VirtualView?.IsEnabled);
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