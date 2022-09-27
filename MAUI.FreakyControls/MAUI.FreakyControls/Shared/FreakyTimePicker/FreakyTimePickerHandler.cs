using System;
using Maui.FreakyControls.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

public partial class FreakyTimePickerHandler : TimePickerHandler
{
    public FreakyTimePickerHandler()
    {
        Mapper.AppendToMapping("FreakyTimePickerCustomization", MapTimePicker);
    }

    private void MapTimePicker(ITimePickerHandler timePickerHandler, ITimePicker timePicker)
    {
        if (timePicker is FreakyTimePicker freakyTimePicker &&
                    timePickerHandler is FreakyTimePickerHandler freakyTimePickerHandler)
        {
            if (freakyTimePicker.ImageSource != default(ImageSource))
            {
                freakyTimePickerHandler.HandleAndAlignImageSourceAsync(freakyTimePicker).RunConcurrently();
            }
        }
    }
}

