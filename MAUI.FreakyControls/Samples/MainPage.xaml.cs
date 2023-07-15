namespace Samples;

public partial class MainPage : ContentPage
{
    private MainViewModel vm;

    public MainPage()
    {
        BindingContext = vm = new MainViewModel();
        InitializeComponent();
    }

    private async void FreakySvgImageView_Tapped(object sender, System.EventArgs e)
    {
        await this.DisplayAlert("Yo", "Hi from the dotnet bot", "Ok");
    }

    private async void ListView_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
    {
        string route = e.Item.ToString();
        await Shell.Current.GoToAsync(route);
    }

    private async void OnButtonClicked(System.Object sender, System.EventArgs e)
    {
        await this.DisplayAlert("Yo", "I am a freaky button", "Ok");
    }
}