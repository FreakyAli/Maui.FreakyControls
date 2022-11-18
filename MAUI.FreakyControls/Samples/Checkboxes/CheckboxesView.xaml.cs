namespace Samples.Checkboxes;

public partial class CheckboxesView : ContentPage
{
	public CheckboxesView()
	{
		InitializeComponent();
		BindingContext = new CheckboxesViewModel();
	}
}
