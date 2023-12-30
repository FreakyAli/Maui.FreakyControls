using Microsoft.Maui.Handlers;
using UIKit;

namespace Maui.FreakyControls;

public partial class FreakySignatureCanvasViewHandler : ViewHandler<View, UIView>
{
    public FreakySignatureCanvasViewHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) : base(mapper, commandMapper)
    {
    }

    protected override UIView CreatePlatformView()
    {
        return new UIView();
    }
}

public partial class FreakyCropViewHandler : ViewHandler<View, UIView>
{
    public FreakyCropViewHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) : base(mapper, commandMapper)
    {
    }

    protected override UIView CreatePlatformView()
    {
        return new UIView();
    }
}