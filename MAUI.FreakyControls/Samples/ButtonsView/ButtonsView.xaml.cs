namespace Samples.ButtonsView;

public partial class ButtonsView : ContentPage
{
    public ButtonsView()
    {
        InitializeComponent();
        BindingContext = new ButtonsViewModel();
    }
}