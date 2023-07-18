using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Samples.SignatureView;

public class SignatureViewModel : MainViewModel
{
    public SignatureViewModel()
    {
        ConversionCommand = new AsyncRelayCommand(ExecuteCommandAsync);
    }

    public ICommand ConversionCommand { get; }

    public Stream ImageStream { get; set; }

    private async Task ExecuteCommandAsync()
    {
        await Shell.Current.Navigation.PushAsync(new ImageDisplay(ImageStream));
    }
}