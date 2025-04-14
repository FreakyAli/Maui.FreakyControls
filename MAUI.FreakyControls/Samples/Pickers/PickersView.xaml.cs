namespace Samples.Pickers;

public partial class PickersView : ContentPage
{
    public PickersView()
    {
        InitializeComponent();
        BindingContext = new PickersViewModel();
    }
}