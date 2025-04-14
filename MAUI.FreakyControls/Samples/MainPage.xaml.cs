namespace Samples;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel vm;

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
        string route = e.Item.ToString();
        if (route == AppShell.JumpList)
        {
            var permission = await PermissionHelper.CheckAndRequestPermissionAsync<Permissions.Vibrate>();
            if (permission != PermissionStatus.Granted)
            {
                await DisplayAlert("Error", "Needs vibration permission for haptik feedback", "Ok");
                return;
            }
        }
        await Shell.Current.GoToAsync(route);
    }
}