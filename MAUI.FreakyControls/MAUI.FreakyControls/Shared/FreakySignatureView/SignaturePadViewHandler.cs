using System;
using Microsoft.Maui.Handlers;

#if ANDROID
using NativeView = Android.Views.View;
#elif IOS
using NativeView = UIKit.UIView;
#elif W
#endif

namespace Maui.FreakyControls;

#if ANDROID || IOS
public partial class SignaturePadViewHandler : ViewHandler<SignaturePadView, NativeView>
{
    public SignaturePadViewHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) : base(mapper, commandMapper)
    {

    }

    protected override NativeView CreatePlatformView()
    {
        throw new NotImplementedException();
    }
}

#else

public partial class FreakySignatureViewHandler 
{

}

#endif