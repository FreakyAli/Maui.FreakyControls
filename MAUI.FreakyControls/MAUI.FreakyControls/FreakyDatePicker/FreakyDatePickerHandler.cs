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

    private void MapDatePicker(IDatePickerHandler datePickerHandler, IDatePicker datePicker)
    {
        try
        {
            if (datePicker is FreakyDatePicker freakyDatePicker &&
                     datePickerHandler is FreakyDatePickerHandler freakyDatePickerHandler)
            {
                if (PlatformView is not null && VirtualView is not null)
                {
                    if (freakyDatePicker.ImageSource != default(ImageSource))
                    {
                        freakyDatePickerHandler.HandleAndAlignImageSourceAsync(freakyDatePicker).RunConcurrently();
                    }
                }
            }
        }
        catch (Exception) { }
    }
}

#else
public partial class FreakyDatePickerHandler : DatePickerHandler
{
}
#endif
