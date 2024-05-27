using Android.Widget;
using Maui.FreakyControls.Platforms.Android.NativeControls;

namespace Maui.FreakyControls;

public partial class ZoomImageHandler
{
    protected override ImageView CreatePlatformView()
    {
        var zoomView = new ZoomageView(this.Context);
        return zoomView;
    }
}