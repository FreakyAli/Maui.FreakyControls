namespace Samples.TextInputLayout;

public partial class TextInputLayoutView : ContentPage
{
    public TextInputLayoutView()
    {
        InitializeComponent();
        BindingContext = new TextInputLayoutViewModel();
    }

    private async void Handle_SlideCompleted(object sender, EventArgs e)
    {
        await this.DisplayAlert("Swiped", "Swipe button was slide successfully!", "Ok");
    }
}