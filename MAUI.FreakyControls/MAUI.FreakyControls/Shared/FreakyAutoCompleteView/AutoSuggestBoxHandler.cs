using System;
#if ANDROID
using PlatformView = Maui.FreakyControls.Platforms.Android.NativeControls.AutoSuggestBoxView;
#elif IOS
using PlatformView = Maui.FreakyControls.Platforms.iOS.NativeControls.AutoSuggestBoxView;
#endif
using Microsoft.Maui.Handlers;
namespace Maui.FreakyControls;

#if ANDROID|| IOS
public partial class AutoSuggestBoxHandler
{
    public static IPropertyMapper<IAutoSuggestBox, AutoSuggestBoxHandler> PropertyMapper =
                new PropertyMapper<IAutoSuggestBox, AutoSuggestBoxHandler>(ViewMapper)
                {
                    [nameof(IAutoSuggestBox.Text)] = MapText,
                    [nameof(IAutoSuggestBox.TextColor)] = MapTextColor,
                    [nameof(IAutoSuggestBox.PlaceholderText)] = MapPlaceholderText,
                    [nameof(IAutoSuggestBox.PlaceholderTextColor)] = MapPlaceholderTextColor,
                    [nameof(IAutoSuggestBox.TextMemberPath)] = MapTextMemberPath,
                    [nameof(IAutoSuggestBox.DisplayMemberPath)] = MapDisplayMemberPath,
                    [nameof(IAutoSuggestBox.IsEnabled)] = MapIsEnabled,
                    [nameof(IAutoSuggestBox.ItemsSource)] = MapItemsSource,
                    [nameof(IAutoSuggestBox.UpdateTextOnSelect)] = MapUpdateTextOnSelect,
                    [nameof(IAutoSuggestBox.IsSuggestionListOpen)] = MapIsSuggestionListOpen,
                };

    /// <summary>
    /// <see cref ="CommandMapper"/> for AutoSuggestBox Control.
    /// </summary>
    public static CommandMapper<IAutoSuggestBox, AutoSuggestBoxHandler> CommandMapper = new(ViewCommandMapper);

    /// <summary>
    /// Initialize new instance of <see cref="AutoSuggestBoxHandler"/>.
    /// </summary>
    /// <param name="mapper">Custom instance of <see cref="PropertyMapper"/>, if it's null the <see cref="CommandMapper"/> will be used</param>
    /// <param name="commandMapper">Custom instance of <see cref="CommandMapper"/></param>
    public AutoSuggestBoxHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? mapper, commandMapper ?? CommandMapper)
    { }

    /// <summary>
    /// Initialize new instance of <see cref="AutoSuggestBoxHandler"/>.
    /// </summary>
    public AutoSuggestBoxHandler() : base(PropertyMapper, CommandMapper)
    {
    }
}

#endif