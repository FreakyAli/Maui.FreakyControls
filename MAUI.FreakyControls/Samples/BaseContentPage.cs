using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace Samples;

public class BaseContentPage : ContentPage
{
    public BaseContentPage()
    {
#if IOS && !MACCATALYST
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Black,
            StatusBarStyle = StatusBarStyle.LightContent
        });
#endif
    }
}