using Microsoft.Maui.Handlers;

namespace Samples.Platforms.Android;

public class CustomNavigationPageRenderer : NavigationViewHandler
{
    public CustomNavigationPageRenderer()
    {
    }

    protected override void ConnectHandler(global::Android.Views.View platformView)
    {
        base.ConnectHandler(platformView);
    }
}