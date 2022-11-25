namespace Samples.InputViews;

public partial class InputViews : ContentPage
{
	public InputViews()
	{
		InitializeComponent();
		this.BindingContext = new InputViewModel();
	}
}