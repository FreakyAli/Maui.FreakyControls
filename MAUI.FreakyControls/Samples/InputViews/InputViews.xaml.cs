namespace Samples.InputViews;

public partial class InputViews : ContentPage
{
    public InputViews()
    {
        InitializeComponent();
        this.BindingContext = new InputViewModel();
    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
    }
}