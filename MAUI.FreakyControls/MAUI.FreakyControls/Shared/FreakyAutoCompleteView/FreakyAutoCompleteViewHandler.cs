using Microsoft.Maui.Handlers;
#if ANDROID
using NativeView = AndroidX.AppCompat.Widget.AppCompatAutoCompleteTextView;
#elif IOS
using NativeView = Maui.FreakyControls.Platforms.iOS.NativeAutoCompleteView;
#endif

namespace Maui.FreakyControls;

#if IOS || ANDROID
public partial class FreakyAutoCompleteViewHandler : ViewHandler<FreakyAutoCompleteView, NativeView>
{
    public static PropertyMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler> Mapper =
            new(ViewHandler.ViewMapper)
            {
                [nameof(FreakyAutoCompleteView.Text)] = MapText,
                [nameof(FreakyAutoCompleteView.ItemsSource)] = MapItemsSource,
            };

    public static CommandMapper<FreakySignatureCanvasView, FreakySignatureCanvasViewHandler> CommandMapper =
        new(ViewHandler.ViewCommandMapper)
        {
        };

    public FreakyAutoCompleteViewHandler() : base(Mapper)
    {
    }

    public FreakyAutoCompleteViewHandler(IPropertyMapper? mapper)
        : base(mapper ?? Mapper, CommandMapper)
    {
    }

    public FreakyAutoCompleteViewHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }
}
#endif

