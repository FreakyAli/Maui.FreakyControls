using System;
#if ANDROID
using PlatformView = Maui.FreakyControls.Platforms.Android.NativeControls.FreakyNativeAutoCompleteView;
#elif IOS
using PlatformView = Maui.FreakyControls.Platforms.iOS.NativeControls.FreakyNativeAutoCompleteView;
#endif
using Microsoft.Maui.Handlers;
namespace Maui.FreakyControls;

#if ANDROID|| IOS
public partial class FreakyAutoCompleteViewHandler
{
    public static IPropertyMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler> PropertyMapper =
        new PropertyMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler>(ViewMapper)
        {
            [nameof(IFreakyAutoCompleteView.Text)] = MapText,
            [nameof(IFreakyAutoCompleteView.TextColor)] = MapTextColor,
            [nameof(IFreakyAutoCompleteView.Placeholder)] = MapPlaceholder,
            [nameof(IFreakyAutoCompleteView.PlaceholderColor)] = MapPlaceholderColor,
            [nameof(IFreakyAutoCompleteView.TextMemberPath)] = MapTextMemberPath,
            [nameof(IFreakyAutoCompleteView.DisplayMemberPath)] = MapDisplayMemberPath,
            [nameof(IFreakyAutoCompleteView.IsEnabled)] = MapIsEnabled,
            [nameof(IFreakyAutoCompleteView.ItemsSource)] = MapItemsSource,
            [nameof(IFreakyAutoCompleteView.UpdateTextOnSelect)] = MapUpdateTextOnSelect,
            [nameof(IFreakyAutoCompleteView.IsSuggestionListOpen)] = MapIsSuggestionListOpen,
            [nameof(IFreakyAutoCompleteView.Threshold)] = MapThreshold,
            [nameof(IFreakyAutoCompleteView.ImageAlignment)] = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImageCommand)] = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImageCommandParameter)] = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImageHeight)] = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImagePadding)] = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImageSource)] = MapImageSource,
            [nameof(IFreakyAutoCompleteView.ImageWidth)] = MapImageSource,

        };

    /// <summary>
    /// <see cref ="CommandMapper"/> for FreakyAutoCompleteView Control.
    /// </summary>
    public static CommandMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler> CommandMapper = new(ViewCommandMapper);

    /// <summary>
    /// Initialize new instance of <see cref="FreakyAutoCompleteViewHandler"/>.
    /// </summary>
    /// <param name="mapper">Custom instance of <see cref="PropertyMapper"/>, if it's null the <see cref="CommandMapper"/> will be used</param>
    /// <param name="commandMapper">Custom instance of <see cref="CommandMapper"/></param>
    public FreakyAutoCompleteViewHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? mapper, commandMapper ?? CommandMapper)
    { }

    /// <summary>
    /// Initialize new instance of <see cref="FreakyAutoCompleteViewHandler"/>.
    /// </summary>
    public FreakyAutoCompleteViewHandler() : base(PropertyMapper, CommandMapper)
    {
    }
}

#endif