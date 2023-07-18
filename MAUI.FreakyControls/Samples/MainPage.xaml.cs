namespace Samples;

public partial class MainPage : ContentPage
{
    private MainViewModel vm;

    public MainPage()
    {
        BindingContext = vm = new MainViewModel();
        InitializeComponent();
    }

    private async void FreakySvgImageView_Tapped(object sender, EventArgs e)
    {
        await DisplayAlert("Yo", "Hi from the dotnet bot", "Ok");
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Yo", "I am a freaky button", "Ok");
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var route = e.Item.ToString();
        await Shell.Current.GoToAsync(route);
    }
}