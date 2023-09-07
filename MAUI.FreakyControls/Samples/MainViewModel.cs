using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Application = Microsoft.Maui.Controls.Application;

namespace Samples
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<string> _suggestionItem;

        private ObservableCollection<string> items;

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
                AppShell.radioButtons,
                AppShell.buttons,
                AppShell.jumpList
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

        public ICommand FreakyLongPressedCommand { get; set; }

        public ICommand ImageWasTappedCommand
        {
            get; set;
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

        public ObservableCollection<string> SuggestionItem
        {
            get => _suggestionItem;
            set
            {
                _suggestionItem = value;
                OnPropertyChanged();
            }
        }

        private async Task ImageTappedAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            Application.Current.MainPage.DisplayAlert("Title", "The image was clicked on that FreakyEntry", "Ok"));
        }

        private async Task LongPressedAsync(object commandParam)
        {
            await Application.Current.MainPage.DisplayAlert(commandParam?.ToString(), "Long pressed yo :D", "Ok");
        }
    }
}