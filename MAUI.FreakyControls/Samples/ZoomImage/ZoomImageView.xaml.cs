namespace Samples.ZoomImage;

public partial class ZoomImageView
{
	public ZoomImageView()
	{
		InitializeComponent();
		this.BindingContext = new ZoomImageViewModel();
	}
}
