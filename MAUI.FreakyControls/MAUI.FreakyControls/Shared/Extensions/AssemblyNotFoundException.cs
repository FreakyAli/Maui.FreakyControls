#if ANDROID
#endif
#if IOS
using Maui.FreakyControls.Platforms.iOS;
using NativeImage = UIKit.UIImage;
using NativeColor = UIKit.UIColor;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#endif

namespace Maui.FreakyControls.Extensions
{
    public class AssemblyNotFoundException : Exception
    {
        public AssemblyNotFoundException(string message) : base(message)
        {
        }
    }
}

