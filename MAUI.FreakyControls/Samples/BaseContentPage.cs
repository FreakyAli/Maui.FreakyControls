using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace Samples;

public class BaseContentPage : ContentPage
{
    public BaseContentPage()
    {
#if IOS && !MACCATALYST
#pragma warning disable CA1416
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Black,
            StatusBarStyle = StatusBarStyle.LightContent
        });
#pragma warning restore CA1416
#endif
    }
}