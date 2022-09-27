using System;
using Maui.FreakyControls.Extensions;
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

public partial class FreakyButtonHandler : ButtonHandler
{
    public FreakyButtonHandler()
    {
        Mapper.AppendToMapping("FreakyButtonCustomization", MapFreakyButton);

    }

    private void MapFreakyButton(IButtonHandler buttonHandler, IButton button)
    {
        if (button is FreakyButton fbutton && buttonHandler is FreakyButtonHandler freakyButtonHandler)
        {
            HandleLongPressed();
        }
    }

    private void ExecuteLongPressedHandlers()
    {
        var freakyButton = (VirtualView as FreakyButton);
        freakyButton?.LongPressedCommand?.ExecuteCommandIfAvailable(freakyButton?.LongPressedCommandParameter);
        freakyButton.RaiseLongPressed();
    }
}

