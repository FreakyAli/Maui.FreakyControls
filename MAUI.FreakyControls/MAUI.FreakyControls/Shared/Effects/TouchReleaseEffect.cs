namespace Maui.FreakyControls.TouchPress;

internal class TouchReleaseEffect : RoutingEffect
{
    public Action OnRelease { get; set; }

    public TouchReleaseEffect(Action onRelease) 
    {
        OnRelease = onRelease;
    }
}