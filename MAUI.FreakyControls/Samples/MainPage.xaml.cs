using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Samples;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        BindingContext = new MainViewModel();
        InitializeComponent();
    }

    async void FreakySvgImageView_Tapped(object sender, System.EventArgs e)
    {
        await this.DisplayAlert("Yo", "Hi from the dotnet bot", "Ok");
    }

    void FreakyTextInputLayout_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        yolo.Text = e.NewTextValue;
    }

    async void OnButtonClicked(System.Object sender, System.EventArgs e)
    {
        await this.DisplayAlert("Yo", "I am a freaky button", "Ok");
    }

    void FreakyButton_LongPressed(System.Object sender, Maui.FreakyControls.LongPressedEventArgs e)
    {

    }
}


