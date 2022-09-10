using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Samples
{
    public class MainViewModel
    {
        public ICommand ImageWasTappedCommand { get; set; }

        public MainViewModel()
        {
            ImageWasTappedCommand = new AsyncRelayCommand(ImageTappedAsync);
        }

        private async Task ImageTappedAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            Application.Current.MainPage.DisplayAlert("Title", "The image was clicked on that FreakyEntry", "Ok"));
        }
    }
}

