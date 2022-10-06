using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Maui.FreakyControls;

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
            await Shell.Current.Navigation.PushAsync(new ImageDisplay(ImageStream)); ;
        }
    }
}

