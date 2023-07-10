namespace Samples.RadioButtons;

public partial class RadioButtonsView : ContentPage
{
    public RadioButtonsView()
    {
        InitializeComponent();
        BindingContext = new RadioButtonsViewModel();
    }

    private void FreakyRadioGroup_SelectedRadioButtonChanged(System.Object sender, Maui.FreakyControls.FreakyRadioButtonEventArgs e)
    {
    }
}