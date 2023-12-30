using Maui.FreakyControls;

namespace Samples.SignatureView;

public class ImageDisplay : ContentPage
{
    public ImageDisplay(Stream stream)
    {
        var imageView = new Image
        {
            Aspect = Aspect.AspectFit
        };
#pragma warning disable Risky
        Content = new FreakyZoomableImage() { Content = imageView };
#pragma warning restore Risky
        var imageSource = ImageSource.FromStream(() => stream);
        imageView.Source = imageSource;
    }
}