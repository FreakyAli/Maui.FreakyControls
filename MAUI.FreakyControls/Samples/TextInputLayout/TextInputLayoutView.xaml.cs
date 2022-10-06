namespace Samples.TextInputLayout;

public partial class TextInputLayoutView : ContentPage
{
	public TextInputLayoutView()
	{
		InitializeComponent();
		BindingContext = new TextInputLayoutViewModel();
	}
}
