namespace Samples.SignatureView;

public partial class SignatureView : ContentPage
{
    public SignatureView()
    {
        InitializeComponent();
        this.BindingContext = new SignatureViewModel();
    }
}
