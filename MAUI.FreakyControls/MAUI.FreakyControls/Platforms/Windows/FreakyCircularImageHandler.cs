namespace Maui.FreakyControls
{
    public sealed partial class FreakyCircularImageHandler
    {
#if WINDOWS
        protected override void ConnectHandler(Microsoft.UI.Xaml.Controls.Image platformView)
        {
            base.ConnectHandler(platformView);
            platformView.SizeChanged += OnSizeChanged;
        }

        protected override void DisconnectHandler(Microsoft.UI.Xaml.Controls.Image platformView)
        {
            platformView.SizeChanged -= OnSizeChanged;
            base.DisconnectHandler(platformView);
        }

        private void OnSizeChanged(object? sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
        {
            if (sender is Microsoft.UI.Xaml.Controls.Image image)
            {
                var visual = Microsoft.UI.Xaml.Hosting.ElementCompositionPreview.GetElementVisual(image);
                var compositor = visual.Compositor;
                var ellipse = compositor.CreateEllipseGeometry();
                var radius = (float)Math.Min(e.NewSize.Width, e.NewSize.Height) / 2f;
                ellipse.Center = new System.Numerics.Vector2((float)(e.NewSize.Width / 2), (float)(e.NewSize.Height / 2));
                ellipse.Radius = new System.Numerics.Vector2(radius, radius);
                visual.Clip = compositor.CreateGeometricClip(ellipse);
            }
        }
#endif
    }
}
