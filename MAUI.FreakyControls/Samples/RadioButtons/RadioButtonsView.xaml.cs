using Maui.FreakyControls;

namespace Samples.RadioButtons;

public partial class RadioButtonsView : ContentPage
{
    public RadioButtonsView()
    {
        InitializeComponent();
        BindingContext = new RadioButtonsViewModel();
    }

    private void FreakyRadioGroup_SelectedRadioButtonChanged(object sender, FreakyRadioButtonEventArgs e)
    {
    }
}