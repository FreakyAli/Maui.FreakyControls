namespace Samples.ScratchView;

public partial class ScratchView : ContentPage
{
	public ScratchView()
	{
		InitializeComponent();
	}
	
	private void OnResetClicked(object sender, EventArgs e)
	{
		scratchView.Reset();
	}

	private void ScratchView_ScratchCompleted(object sender, EventArgs e)
	{
    }
}