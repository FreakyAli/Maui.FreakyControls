namespace Samples.TextInputLayout;

public partial class TextInputLayoutView : ContentPage
{
    public TextInputLayoutView()
    {
        InitializeComponent();
        BindingContext = new TextInputLayoutViewModel();
    }

    void Handle_SlideCompleted(object sender, System.EventArgs e)
    {
        MessageLbl.Text = "Success!!";
    }
}