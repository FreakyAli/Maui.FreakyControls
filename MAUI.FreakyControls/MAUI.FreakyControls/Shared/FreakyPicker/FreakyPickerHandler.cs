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
        if (picker is FreakyPicker freakyTimePicker &&
            pickerHandler is FreakyPickerHandler freakyTimePickerHandler)
            if (freakyTimePicker.ImageSource != default(ImageSource))
                freakyTimePickerHandler.HandleAndAlignImageSourceAsync(freakyTimePicker).RunConcurrently();
    }
}
#else
public partial class FreakyPickerHandler : PickerHandler
{
}
#endif