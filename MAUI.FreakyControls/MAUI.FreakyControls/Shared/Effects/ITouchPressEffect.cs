namespace Maui.FreakyControls.TouchPress;

public interface ITouchAndPressEffectConsumer
{
    void ConsumeEvent(EventType gestureType);

    bool IsEnabled { get; set; }
    AnimationTypes Animation { get; set; }

    void ExecuteAction();
}