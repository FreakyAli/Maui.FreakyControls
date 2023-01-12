using System;
using Microsoft.Maui.Handlers;
#if ANDROID
using AutoCompleteTextField = AndroidX.AppCompat.Widget.AppCompatAutoCompleteTextView;
#elif IOS
using AutoCompleteTextField = Maui.FreakyControls.AutoCompleteTextField;
#endif

namespace Maui.FreakyControls;

#if IOS || ANDROID
public partial class FreakyAutoCompleteViewHandler : ViewHandler<FreakyAutoCompleteView, AutoCompleteTextField>
{
    public static IPropertyMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler> Mapper
       => new PropertyMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler>(ViewHandler.ViewMapper)
       {
           [nameof(FreakyAutoCompleteView.Text)] = MapText,
           [nameof(FreakyAutoCompleteView.ItemsSource)] = MapItemsSource,
       };

    public static CommandMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler> CommandMapper =
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

