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
        public ObservableCollection<string> SuggestionItem
        {
            get => _suggestionItem;
            set
            {
                _suggestionItem = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ImageWasTappedCommand = new AsyncRelayCommand(ImageTappedAsync, new AsyncRelayCommandOptions()
            {

            });
            var strSuggestionArr = new string[] {
                        "harshad@mobmaxime.com",
                        "sagar.p@mobmaxime.com",
                        "kapil@mobmaxime.com",
                        "harry@test.com",
                        "xamarin@test.com",
                        "xamarinteam@mob.com"
                        };

            SuggestionItem = new ObservableCollection<string>(strSuggestionArr.ToList());
        }

        private async Task ImageTappedAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            Application.Current.MainPage.DisplayAlert("Title", "The image was clicked on that FreakyEntry", "Ok"));
        }
    }
}

