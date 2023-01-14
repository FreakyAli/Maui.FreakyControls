namespace Samples.ImageViews;

public partial class ImagesPage : ContentPage
{
	public ImagesPage()
	{
		InitializeComponent();
		BindingContext = new ImagesViewModel();
	}

    void FreakySvgImageView_Tapped(System.Object sender, System.EventArgs e)
    {		
    }
}
