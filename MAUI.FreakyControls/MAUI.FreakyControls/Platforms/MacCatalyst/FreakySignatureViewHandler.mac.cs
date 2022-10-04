using System;
using Microsoft.Maui.Handlers;
using UIKit;

namespace Maui.FreakyControls;

public partial class FreakySignatureViewHandler : ViewHandler<View, UIView>
{
    public FreakySignatureViewHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) : base(mapper, commandMapper)
    {
    }

    protected override UIView CreatePlatformView()
    {
        return new UIView();
    }
}

