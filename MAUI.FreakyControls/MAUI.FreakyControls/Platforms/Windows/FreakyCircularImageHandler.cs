#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
#endif

namespace Maui.FreakyControls
{
    public sealed partial class FreakyCircularImageHandler
    {
#if WINDOWS
        protected override void ConnectHandler(Image platformView)
        {
            base.ConnectHandler(platformView);
            platformView.SizeChanged += OnSizeChanged;
        }

        protected override void DisconnectHandler(Image platformView)
        {
            platformView.SizeChanged -= OnSizeChanged;
            base.DisconnectHandler(platformView);
        }

        private void OnSizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
        {
            if (sender is Image image)
            {
                var radius = Math.Min(e.NewSize.Width, e.NewSize.Height) / 2;
                image.Clip = new EllipseGeometry
                {
                    Center = new Point(e.NewSize.Width / 2, e.NewSize.Height / 2),
                    RadiusX = radius,
                    RadiusY = radius
                };
            }
        }
#endif
    }
}
