namespace Samples.InputViews;

public partial class InputViews : ContentPage
{
    public InputViews()
    {
        InitializeComponent();
        BindingContext = new InputViewModel();
    }
}