namespace Samples.SwipeCardView;

public partial class SwipeCardView : ContentPage
{
	public SwipeCardView()
	{
		InitializeComponent();
		this.BindingContext = new SwipeCardViewModel();
    }
}