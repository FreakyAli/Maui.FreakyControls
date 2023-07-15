namespace Maui.FreakyControls.TouchPress;

internal interface ITouchPressEffect
{
    void ConsumeEvent(EventType gestureType);
    bool IsEnabled { get; set; }
    AnimationTypes Animation { get; set; }
    void ExecuteAction();
}