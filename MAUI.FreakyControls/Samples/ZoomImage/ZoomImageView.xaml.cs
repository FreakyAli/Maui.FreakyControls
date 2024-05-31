namespace Samples.ZoomImage;

public partial class ZoomImageView : ContentPage
{
	public ZoomImageView()
	{
		InitializeComponent();
		this.BindingContext = new ZoomImageViewModel();
	}
}
