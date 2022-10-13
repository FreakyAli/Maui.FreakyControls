namespace Samples.Pickers;

public partial class PickersView : ContentPage
{
	public PickersView()
	{
		InitializeComponent();
		this.BindingContext = new PickersViewModel();
	}
}
