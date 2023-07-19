namespace Maui.FreakyControls.Shared.TouchPress;

internal class TouchReleaseRoutingEffect : RoutingEffect
{
    public TouchReleaseRoutingEffect(Action onRelease)
    {
        OnRelease = onRelease;
    }

    public Action OnRelease { get; set; }
}