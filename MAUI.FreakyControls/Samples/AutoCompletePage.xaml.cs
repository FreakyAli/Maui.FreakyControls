namespace Samples;

public partial class AutoCompletePage : ContentPage
{
	public AutoCompletePage()
	{
		InitializeComponent();
		this.BindingContext = new AutoCompleteViewModel();
	}
}
