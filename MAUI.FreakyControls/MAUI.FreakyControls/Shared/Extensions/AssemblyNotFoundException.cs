#if ANDROID
#endif
#if IOS
using Maui.FreakyControls.Platforms.iOS;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using NativeColor = UIKit.UIColor;
using NativeImage = UIKit.UIImage;
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