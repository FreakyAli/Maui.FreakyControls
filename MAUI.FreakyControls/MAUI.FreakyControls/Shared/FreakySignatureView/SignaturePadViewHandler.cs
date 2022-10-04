using System;
using Microsoft.Maui.Handlers;

#if ANDROID
using NativeView = Maui.FreakyControls.Platforms.Android.SignaturePadView;
#elif IOS
using NativeView = Maui.FreakyControls.Platforms.iOS.SignaturePadView;
#elif W
#endif

namespace Maui.FreakyControls;

#if ANDROID || IOS
public partial class SignaturePadViewHandler : ViewHandler<SignaturePadView, NativeView>
{
    public static PropertyMapper<SignaturePadView, SignaturePadViewHandler> customMapper =
            new(ViewHandler.ViewMapper)
            {
               
            };



    public static CommandMapper<SignaturePadCanvasView, SignaturePadCanvasViewHandler> customCommandMapper =
        new(ViewHandler.ViewCommandMapper)
        {

        };

    public SignaturePadViewHandler(PropertyMapper mapper = null) : base(mapper ?? customMapper)
    {
    }

    public SignaturePadViewHandler() : base(customMapper, customCommandMapper)
    {
    }
}

#else

public partial class SignaturePadViewHandler 
{

}

#endif