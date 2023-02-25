using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Application = Microsoft.Maui.Controls.Application;

namespace Samples
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand ImageWasTappedCommand
        {
            get; set;
        }

        ObservableCollection<string> _suggestionItem;
        private ObservableCollection<string> items;

        public ObservableCollection<string> SuggestionItem
        {
            get => _suggestionItem;
            set
            {
                _suggestionItem = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged();
            }
        }

        public ICommand FreakyLongPressedCommand { get; set; }

        public MainViewModel()
        {
            ImageWasTappedCommand = new AsyncRelayCommand(ImageTappedAsync, new AsyncRelayCommandOptions());
            FreakyLongPressedCommand = new AsyncRelayCommand<object>(LongPressedAsync);

            Items = new ObservableCollection<string>
            {
                AppShell.pickers,
                AppShell.textInputLayout,
                AppShell.inputViews,
                AppShell.imageViews,
                AppShell.signatureView,
                AppShell.checkboxes,
                AppShell.radioButtons
            };

            var strSuggestionArr = new string[] {
                        "hello",
                        "how are you",
                        "some other question",
                        "Yuhu",
                        "Yello",
                        "Majin buuuuuuuuuu"
                        };
            SuggestionItem = new ObservableCollection<string>(strSuggestionArr.ToList());
        }

        private async Task LongPressedAsync(object commandParam)
        {
            await Application.Current.MainPage.DisplayAlert(commandParam?.ToString(), "Long pressed yo :D", "Ok");
        }

        private async Task ImageTappedAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            Application.Current.MainPage.DisplayAlert("Title", "The image was clicked on that FreakyEntry", "Ok"));
        }
    }
}