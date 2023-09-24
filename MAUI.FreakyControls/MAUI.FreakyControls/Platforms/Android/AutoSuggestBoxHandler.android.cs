using Android.Content.Res;
using AndroidX.Core.View;
using Maui.FreakyControls.Platforms.Android.NativeControls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Maui.FreakyControls;

public partial class FreakyAutoCompleteViewHandler : ViewHandler<IFreakyAutoCompleteView, FreakyAutoCompleteViewView>
{
    /// <inheritdoc />
    //protected override FreakyAutoCompleteViewView CreatePlatformView() => new FreakyAutoCompleteViewView(Context);
    protected override FreakyAutoCompleteViewView CreatePlatformView()
    {
        var _nativeView = new FreakyAutoCompleteViewView(this.Context);
        var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        ViewCompat.SetBackgroundTintList(_nativeView, colorStateList);
        return _nativeView;
    }

    protected override void ConnectHandler(FreakyAutoCompleteViewView platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SuggestionChosen += OnPlatformViewSuggestionChosen;
        platformView.TextChanged += OnPlatformViewTextChanged;
        platformView.QuerySubmitted += OnPlatformViewQuerySubmitted;
        platformView.SetTextColor(VirtualView?.TextColor.ToPlatform() ?? VirtualView.TextColor.ToPlatform());
        UpdateTextColor(platformView);
        UpdatePlaceholderText(platformView);
    }

    protected override void DisconnectHandler(FreakyAutoCompleteViewView platformView)
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

    public static void MapPlaceholderText(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.PlaceholderText = view.PlaceholderText;
    }

    public static void MapPlaceholderTextColor(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.SetPlaceholderTextColor(view.PlaceholderTextColor);
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

    private void UpdateTextColor(FreakyAutoCompleteViewView platformView)
    {
        var color = VirtualView?.TextColor;
        platformView.SetTextColor(color.ToPlatform());
    }

    private void UpdateDisplayMemberPath(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    private void UpdatePlaceholderTextColor(FreakyAutoCompleteViewView platformView)
    {
        var placeholderColor = VirtualView?.PlaceholderTextColor;
        platformView.SetPlaceholderTextColor(placeholderColor);
    }

    private void UpdatePlaceholderText(FreakyAutoCompleteViewView platformView) => platformView.PlaceholderText = VirtualView?.PlaceholderText;

    private void UpdateIsEnabled(FreakyAutoCompleteViewView platformView)
    {
        platformView.Enabled = (bool)(VirtualView?.IsEnabled);
    }

    private void UpdateItemsSource(FreakyAutoCompleteViewView platformView)
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