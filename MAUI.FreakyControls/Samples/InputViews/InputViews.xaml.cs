using Maui.FreakyControls;
using Maui.FreakyControls.Extensions;

namespace Samples.InputViews;

public partial class InputViews : ContentPage
{
    private InputViewModel viewModel;

    public InputViews()
    {
        InitializeComponent();
        BindingContext = viewModel = new InputViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //XfPinView.FocusBox();
    }

    private void FreakyAutoCompleteView_QuerySubmitted(object sender, Maui.FreakyControls.FreakyAutoCompleteViewQuerySubmittedEventArgs e)
    {
    }

    private void FreakyAutoCompleteView_TextChanged(object sender, Maui.FreakyControls.FreakyAutoCompleteViewTextChangedEventArgs e)
    {
        var autoComplete = (FreakyAutoCompleteView)sender;
        viewModel.NamesCollection = viewModel.Names.
            Where(s => s.StartsWith(autoComplete.Text, StringComparison.InvariantCultureIgnoreCase)).
            ToObservable();
    }
}