namespace Maui.FreakyControls.TouchPress;

internal class TouchReleaseEffect : RoutingEffect
{
    public TouchReleaseEffect(Action onRelease)
    {
        OnRelease = onRelease;
    }

    public Action OnRelease { get; set; }
}