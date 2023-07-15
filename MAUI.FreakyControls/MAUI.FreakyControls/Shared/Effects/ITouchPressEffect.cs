using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls.Shared.TouchPress;

internal interface ITouchPressEffect
{
    ButtonAnimations Animation { get; set; }

    bool IsEnabled { get; set; }

    void ConsumeEvent(EventType gestureType);

    void ExecuteAction();
}