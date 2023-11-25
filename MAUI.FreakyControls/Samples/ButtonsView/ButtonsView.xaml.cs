namespace Samples.ButtonsView;

public partial class ButtonsView : ContentPage
{
    public ButtonsView()
    {
        InitializeComponent();
    }

    void Handle_SlideCompleted(object sender, System.EventArgs e)
    {
        MessageLbl.Text = "Success!!";
    }
}