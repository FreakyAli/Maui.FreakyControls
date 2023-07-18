#if ANDROID
#endif
#if IOS
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