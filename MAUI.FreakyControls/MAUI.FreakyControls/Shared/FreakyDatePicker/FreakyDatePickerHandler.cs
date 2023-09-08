using System;
using Maui.FreakyControls.Extensions;
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

#if ANDROID || IOS
public partial class FreakyDatePickerHandler : DatePickerHandler
{
    public FreakyDatePickerHandler()
    {
        Mapper.AppendToMapping("FreakyDatePickerCustomization", MapDatePicker);
    }

    // Todo: Remove try-catch added as a quickfix for https://github.com/FreakyAli/Maui.FreakyControls/issues/76
    private void MapDatePicker(IDatePickerHandler datePickerHandler, IDatePicker datePicker)
    {
        try
        {
            if (datePicker is FreakyDatePicker freakyDatePicker &&
                     datePickerHandler is FreakyDatePickerHandler freakyDatePickerHandler)
            {
                if (PlatformView != null && VirtualView != null)
                {
                    if (freakyDatePicker.ImageSource != default(ImageSource))
                    {
                        freakyDatePickerHandler.HandleAndAlignImageSourceAsync(freakyDatePicker).RunConcurrently();
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
public partial class FreakyDatePickerHandler : DatePickerHandler
{
}
#endif
