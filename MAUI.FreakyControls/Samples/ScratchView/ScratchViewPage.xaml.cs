namespace Samples.ScratchView;

public partial class ScratchViewPage : ContentPage
{
    public ScratchViewPage()
    {
        try
        {
            InitializeComponent();
            BindingContext = new ScratchViewModel();
        }
        catch (Exception ex)
        {


        }
    }
}
