using System;
using Maui.FreakyControls.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

#if ANDROID || IOS
public partial class FreakyPickerHandler : PickerHandler
{
    public FreakyPickerHandler()
    {
        Mapper.AppendToMapping("FreakyPickerCustomization", MapPicker);
    }

    // Todo: Remove try-catch added as a quickfix for https://github.com/FreakyAli/Maui.FreakyControls/issues/76
    private void MapPicker(IPickerHandler pickerHandler, IPicker picker)
    {
        try
        {
            if (picker is FreakyPicker freakyTimePicker &&
                          pickerHandler is FreakyPickerHandler freakyTimePickerHandler)
            {
                if (PlatformView is not null && VirtualView is not null)
                {
                    if (freakyTimePicker.ImageSource != default(ImageSource))
                    {
                        freakyTimePickerHandler.HandleAndAlignImageSourceAsync(freakyTimePicker).RunConcurrently();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.TraceException();
        }
    }
}
#else
public partial class FreakyPickerHandler : PickerHandler
{
}
#endif
