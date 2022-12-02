namespace Samples.InputViews;

public partial class InputViews : ContentPage
{
	public InputViews()
	{
		InitializeComponent();
		this.BindingContext = new InputViewModel();
	}

    void AutoCompleteView_TextChanged(System.Object sender, Maui.FreakyControls.AutoSuggestBoxTextChangedEventArgs e)
    {
    }

    void AutoCompleteView_QuerySubmitted(System.Object sender, Maui.FreakyControls.AutoSuggestBoxQuerySubmittedEventArgs e)
    {
    }
}