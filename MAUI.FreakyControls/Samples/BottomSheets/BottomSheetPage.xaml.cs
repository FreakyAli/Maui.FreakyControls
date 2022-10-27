namespace Samples.BottomSheets;

public partial class BottomSheetPage : ContentPage
{
	public BottomSheetPage()
	{
		InitializeComponent();
	}

    async void CloseMe_Clicked(System.Object sender, System.EventArgs e)
    {
        await this.customBottomSheet.CloseAsync();
    }

    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        await this.customBottomSheet.OpenAsync();
    }
}
