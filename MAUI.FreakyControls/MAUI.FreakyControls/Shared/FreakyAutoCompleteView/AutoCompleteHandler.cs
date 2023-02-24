using System;
using Microsoft.Maui.Handlers;
#if ANDROID
using NativeAutoComplete = Maui.FreakyControls.Platforms.Android.NativeControls.AndroidAutoSuggestBox;
#elif IOS
using NativeAutoComplete = Maui.FreakyControls.Platforms.iOS.NativeControls.iOSAutoSuggestBox;
#endif

namespace Maui.FreakyControls;

#if ANDROID||IOS
public partial class AutoCompleteHandler : ViewHandler<AutoCompleteView, NativeAutoComplete>
{
    public AutoCompleteHandler() : base(Mapper)
    {
    }

    public AutoCompleteHandler(IPropertyMapper? mapper)
        : base(mapper ?? Mapper, CommandMapper)
    {
    }

    public AutoCompleteHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }

    public static CommandMapper<AutoCompleteView, AutoCompleteHandler> CommandMapper =
        new(ViewHandler.ViewCommandMapper)
        {
        };

    public static PropertyMapper<AutoCompleteView, AutoCompleteHandler> Mapper =
             new(ViewHandler.ViewMapper)
             {
                 //[nameof(AutoCompleteView.TextColor)] = MapTextColor,
                 [nameof(AutoCompleteView.Text)] = MapText,
                 //[nameof(AutoCompleteView.PlaceholderText)] = MapPlaceHolderText,
                 //[nameof(AutoCompleteView.PlaceholderTextColor)] = MapPlaceHolderTextColor,
                 [nameof(AutoCompleteView.ItemsSource)] = MapItemSource,
                 [nameof(AutoCompleteView.DisplayMemberPath)]= MapDisplayMember,
                 //[nameof(AutoCompleteView.IsEnabled)] = MapIsEnabled,
             };

    private static void MapDisplayMember(AutoCompleteHandler handler, AutoCompleteView view)
    {
        handler.UpdateDisplayMemberPath();
    }

    private static void MapItemSource(AutoCompleteHandler handler, AutoCompleteView view)
    {
        handler.UpdateItemsSource();
    }

    private static void MapPlaceHolderTextColor(AutoCompleteHandler handler, AutoCompleteView view)
    {
        handler.UpdatePlaceholderTextColor();
    }

    private static void MapPlaceHolderText(AutoCompleteHandler handler, AutoCompleteView view)
    {
        handler.UpdatePlaceholderText();
    }

    private static void MapText(AutoCompleteHandler handler, AutoCompleteView view)
    {
        handler.UpdateText();
    }

    private static void MapTextColor(AutoCompleteHandler handler, AutoCompleteView view)
    {
        handler.UpdateTextColor();
    }

    private void AutoSuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
    {
        MessagingCenter.Send(VirtualView, "AutoSuggestBox_" + nameof(AutoCompleteView.QuerySubmitted), (e.QueryText, e.ChosenSuggestion));
    }

    private void AutoSuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
    {
        MessagingCenter.Send(VirtualView, "AutoSuggestBox_" + nameof(AutoCompleteView.TextChanged), (PlatformView.Text, (AutoSuggestionBoxTextChangeReason)e.Reason));
    }

    private void AutoSuggestBox_SuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
    {
        MessagingCenter.Send(VirtualView, "AutoSuggestBox_" + nameof(AutoCompleteView.SuggestionChosen), e.SelectedItem);
    }

    private void UpdateText()
    {
        PlatformView.Text = VirtualView?.Text;
    }

    private void UpdateTextColor()
    {
        var color = VirtualView.TextColor;
        PlatformView?.SetTextColor(color);
    }

    private void UpdatePlaceholderTextColor()
    {
        var placeholderColor = VirtualView.PlaceholderTextColor;
        PlatformView?.SetPlaceholderTextColor(placeholderColor);
    }

    private void UpdatePlaceholderText() => PlatformView.PlaceholderText = VirtualView?.PlaceholderText;

    private void UpdateDisplayMemberPath()
    {
        PlatformView.SetItems(VirtualView.ItemsSource?.OfType<object>(), (o) => FormatType(o, VirtualView.DisplayMemberPath), (o) => FormatType(o, VirtualView.TextMemberPath));
    }

    private void UpdateItemsSource()
    {
        PlatformView.SetItems(VirtualView?.ItemsSource?.OfType<object>(), (o) => FormatType(o, VirtualView?.DisplayMemberPath), (o) => FormatType(o, VirtualView?.TextMemberPath));
    }

    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
        else
            return instance?.ToString() ?? "";
    }
}
#endif