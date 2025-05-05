using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.Core.View;
using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.Android.NativeControls;
using Maui.FreakyControls.Enums;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using static Microsoft.Maui.ApplicationModel.Platform;
using Android.Views;
using Android.Util;

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
        UpdateText(platformView);
        UpdateTextColor(platformView);
        UpdatePlaceholder(platformView);
        UpdatePlaceholderColor(platformView);
        UpdateDisplayMemberPath(platformView);
        UpdateIsEnabled(platformView);
        UpdateFont(platformView);
        UpdateTextAlignment(platformView);
        UpdateItemsSource(platformView);
        platformView.SuggestionChosen += OnPlatformViewSuggestionChosen;
        platformView.TextChanged += OnPlatformViewTextChanged;
        platformView.QuerySubmitted += OnPlatformViewQuerySubmitted;
        platformView.SetTextColor(VirtualView?.TextColor.ToPlatform() ?? VirtualView.TextColor.ToPlatform());
        UpdateTextColor(platformView);
        UpdatePlaceholder(platformView);
    }

    protected override void DisconnectHandler(FreakyNativeAutoCompleteView platformView)
    {
        base.DisconnectHandler(platformView);

        platformView.SuggestionChosen -= OnPlatformViewSuggestionChosen;
        platformView.TextChanged -= OnPlatformViewTextChanged;
        platformView.QuerySubmitted -= OnPlatformViewQuerySubmitted;

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

    public static void MapText(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        if (handler.PlatformView?.Text != view.Text)
            handler.PlatformView.Text = view.Text;
    }

    public static void MapTextColor(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        if (handler.PlatformView is null)
            return;

        handler.PlatformView.SetTextColor(view.TextColor.ToPlatform());
    }

    public static void MapPlaceholder(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        if (handler.PlatformView is null)
            return;

        handler.PlatformView.Hint = view.Placeholder;
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
        if (imageBitmap is not null)
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
    public static void MapDisplayMemberPath(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    public static void MapIsSuggestionListOpen(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.IsSuggestionListOpen = view.IsSuggestionListOpen;
    }

    public static void MapTextMemberPath(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.SetItems(view.ItemsSource?.OfType<object>(), (o) => FormatType(o, view.DisplayMemberPath), (o) => FormatType(o, view.TextMemberPath));
    }
    
    public static void MapPlaceholderColor(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        if (handler.PlatformView == null || view.PlaceholderColor == null)
            return;

        handler.PlatformView.SetHintTextColor(view.PlaceholderColor.ToPlatform());
    }

    public static void MapUpdateTextOnSelect(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.UpdateTextOnSelect = view.UpdateTextOnSelect;
    }

    public static void MapIsEnabled(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        if (handler.PlatformView == null)
            return;

        handler.PlatformView.Enabled = view.IsEnabled;
    }

    public static void MapItemsSource(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.SetItems(view.ItemsSource?.OfType<object>(), o => FormatType(o, view.DisplayMemberPath), o => FormatType(o, view.TextMemberPath));
    }

    private void UpdateSuggestionListWidth(FreakyNativeAutoCompleteView platformView)
    {
        if (VirtualView == null || platformView == null)
            return;
        if (VirtualView.SuggestionListWidth <= 0)
        {
            platformView.DropDownWidth = ViewGroup.LayoutParams.WrapContent;
        }
        else
        {
            platformView.DropDownWidth = (int)VirtualView.SuggestionListWidth.DipsToPixels();
        }
    }

    private void UpdateSuggestionListHeight(FreakyNativeAutoCompleteView platformView)
    {
        if (VirtualView == null || platformView == null)
            return;
        // Only override if the user provided a specific height
        if (VirtualView.SuggestionListHeight > 0)
        {
            platformView.DropDownHeight = (int)VirtualView.SuggestionListHeight.DipsToPixels();
        }
        else
        {
            // Use WRAP_CONTENT equivalent to let it size dynamically
            platformView.DropDownHeight = ViewGroup.LayoutParams.WrapContent;
        }
    }

    private void UpdateText(FreakyNativeAutoCompleteView platformView)
    {
        platformView.Text = VirtualView?.Text ?? string.Empty;
    }

    private void UpdateTextColor(FreakyNativeAutoCompleteView platformView)
    {
        if (VirtualView?.TextColor != null)
            platformView.SetTextColor(VirtualView.TextColor.ToPlatform());
    }

    private void UpdatePlaceholder(FreakyNativeAutoCompleteView platformView)
    {
        platformView.Hint = VirtualView?.Placeholder;
    }

    private void UpdatePlaceholderColor(FreakyNativeAutoCompleteView platformView)
    {
        if (VirtualView?.PlaceholderColor != null)
            platformView.SetHintTextColor(VirtualView.PlaceholderColor.ToPlatform());
    }

    private void UpdateDisplayMemberPath(FreakyNativeAutoCompleteView platformView)
    {
        platformView.SetItems(VirtualView.ItemsSource?.OfType<object>(), o => FormatType(o, VirtualView.DisplayMemberPath), o => FormatType(o, VirtualView.TextMemberPath));
    }

    private void UpdateIsEnabled(FreakyNativeAutoCompleteView platformView)
    {
        platformView.Enabled = VirtualView?.IsEnabled ?? true;
    }

    private void UpdateItemsSource(FreakyNativeAutoCompleteView platformView)
    {
        platformView.SetItems(VirtualView?.ItemsSource?.OfType<object>(), o => FormatType(o, VirtualView.DisplayMemberPath), o => FormatType(o, VirtualView.TextMemberPath));
    }

    private void UpdateFont(FreakyNativeAutoCompleteView platformView)
    {
        if (VirtualView == null)
            return;

        Typeface? typeface = null;

        if (!string.IsNullOrEmpty(VirtualView.FontFamily))
        {
            try
            {
                typeface = Typeface.Create(VirtualView.FontFamily, TypefaceStyle.Normal);
            }
            catch
            {
                // Fallback in case font family not found
                typeface = Typeface.Default;
            }
        }
        else
        {
            typeface = VirtualView.FontAttributes switch
            {
                FontAttributes.Bold => Typeface.DefaultBold,
                _ => Typeface.Default,
            };
        }

        platformView.Typeface = typeface;

        if (VirtualView.FontSize > 0)
        {
            platformView.SetTextSize(ComplexUnitType.Sp, (float)VirtualView.FontSize);
        }
    }

    private void UpdateTextAlignment(FreakyNativeAutoCompleteView platformView)
    {
        if (VirtualView == null)
            return;

        platformView.Gravity = (VirtualView.VerticalTextAlignment, VirtualView.HorizontalTextAlignment) switch
        {
            (Microsoft.Maui.TextAlignment.Start, Microsoft.Maui.TextAlignment.Start) => GravityFlags.Top | GravityFlags.Left,
            (Microsoft.Maui.TextAlignment.Start, Microsoft.Maui.TextAlignment.Center) => GravityFlags.Top | GravityFlags.CenterHorizontal,
            (Microsoft.Maui.TextAlignment.Start, Microsoft.Maui.TextAlignment.End) => GravityFlags.Top | GravityFlags.Right,
            
            (Microsoft.Maui.TextAlignment.Center, Microsoft.Maui.TextAlignment.Start) => GravityFlags.CenterVertical | GravityFlags.Left,
            (Microsoft.Maui.TextAlignment.Center, Microsoft.Maui.TextAlignment.Center) => GravityFlags.Center,
            (Microsoft.Maui.TextAlignment.Center, Microsoft.Maui.TextAlignment.End) => GravityFlags.CenterVertical | GravityFlags.Right,
            
            (Microsoft.Maui.TextAlignment.End, Microsoft.Maui.TextAlignment.Start) => GravityFlags.Bottom | GravityFlags.Left,
            (Microsoft.Maui.TextAlignment.End, Microsoft.Maui.TextAlignment.Center) => GravityFlags.Bottom | GravityFlags.CenterHorizontal,
            (Microsoft.Maui.TextAlignment.End, Microsoft.Maui.TextAlignment.End) => GravityFlags.Bottom | GravityFlags.Right,
            
            _ => GravityFlags.Center,
        };
    }

    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
        else
            return instance?.ToString() ?? "";
    }
}