using Maui.FreakyControls.Extensions;
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

#if ANDROID || IOS
public partial class FreakyPickerHandler : PickerHandler
{
    public FreakyPickerHandler()
    {
        Mapper.AppendToMapping("FreakyPickerCustomization", MapPicker);
    }

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
        catch (InvalidOperationException ex) { }
    }
}
#else
public partial class FreakyPickerHandler : PickerHandler
{
}
#endif
