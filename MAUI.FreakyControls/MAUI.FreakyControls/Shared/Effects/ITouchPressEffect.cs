namespace Maui.FreakyControls.TouchPress;

internal interface ITouchPressEffect
{
    AnimationTypes Animation { get; set; }

    bool IsEnabled { get; set; }

    void ConsumeEvent(EventType gestureType);

    void ExecuteAction();
}