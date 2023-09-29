namespace Samples.InputViews;

public partial class InputViews : ContentPage
{
    public InputViews()
    {
        InitializeComponent();
        this.BindingContext = new InputViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    void FreakyCodeView_CodeEntryCompleted(object sender, Maui.FreakyControls.FreakyCodeCompletedEventArgs e)
    {
    }
}