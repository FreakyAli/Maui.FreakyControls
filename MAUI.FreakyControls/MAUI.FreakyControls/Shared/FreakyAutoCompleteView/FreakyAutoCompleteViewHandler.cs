using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

#if IOS || ANDROID

public partial class FreakyAutoCompleteViewHandler
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

