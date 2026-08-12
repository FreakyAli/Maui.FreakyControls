#nullable enable
namespace Samples.SignatureView;

public class ImageDisplay : ContentPage
{
    public ImageDisplay(Stream? stream)
    {
        var imageView = new Image
        {
            Aspect = Aspect.AspectFit
        };
        Content = imageView;
        if (stream is not null)
            imageView.Source = ImageSource.FromStream(() => stream);
    }
}