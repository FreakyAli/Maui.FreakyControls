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
        Content = new FreakyZoomableImage() { Content = imageView };
        var imageSource = ImageSource.FromStream(() => stream);
        imageView.Source = imageSource;
    }
}