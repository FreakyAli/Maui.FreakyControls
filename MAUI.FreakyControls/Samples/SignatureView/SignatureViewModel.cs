using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Samples.SignatureView
{
    public class SignatureViewModel : MainViewModel
    {
        public ICommand ConversionCommand { get; }

        public Stream ImageStream { get; set; }

        public SignatureViewModel()
        {
            ConversionCommand = new AsyncRelayCommand(ExecuteCommandAsync);
        }

        private async Task ExecuteCommandAsync()
        {
            await Shell.Current.Navigation.PushAsync(new ImageDisplay(ImageStream));
        }
    }
}