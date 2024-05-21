namespace Samples.ImageViews;

public partial class ImagesPage : ContentPage
{
    public ImagesPage()
    {
        InitializeComponent();
        BindingContext = new ImagesViewModel();
    }
}