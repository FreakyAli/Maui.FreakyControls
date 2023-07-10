namespace Samples.Checkboxes;

public partial class CheckboxesView : ContentPage
{
    public CheckboxesView()
    {
        InitializeComponent();
        BindingContext = new CheckboxesViewModel();
    }

    private void FreakyCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
    }
}