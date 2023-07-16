namespace Maui.FreakyControls.Shared.TouchPress;

internal class ToouchReleaseRoutingEffect : RoutingEffect
{
    public ToouchReleaseRoutingEffect(Action onRelease)
    {
        OnRelease = onRelease;
    }

    public Action OnRelease { get; set; }
}