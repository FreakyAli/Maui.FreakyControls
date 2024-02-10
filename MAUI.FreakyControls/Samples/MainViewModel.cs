using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Application = Microsoft.Maui.Controls.Application;

namespace Samples
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<string> items;

        public List<string> Names => names;

        protected List<string> names = new List<string>
        {
            "Ava", "Amelia", "Adam", "Aaron", "Abigail", "Addison", "Alexandra", "Alice", "Ashley", "Aiden",
            "Benjamin", "Bella", "Brandon", "Brooke", "Blake", "Brian", "Brooklyn", "Bailey", "Bradley", "Brianna",
            "Charlotte", "Caleb", "Chloe", "Carter", "Christopher", "Claire", "Cameron", "Cassidy", "Cooper", "Caroline",
            "Daniel", "David", "Dylan", "Destiny", "Delilah", "Dominic", "Dakota", "Daisy", "Devin", "Derek",
            "Emma", "Ethan", "Elizabeth", "Elijah", "Emily", "Eric", "Evelyn", "Ellie", "Evan", "Elena",
            "Finn", "Fiona", "Faith", "Francesca", "Felicity", "Felix", "Freya", "Ford", "Flora", "Franklin",
            "Grace", "Gabriel", "Genesis", "Gemma", "George", "Giselle", "Gavin", "Giana", "Greyson", "Gwendolyn",
            "Hannah", "Henry", "Hazel", "Harper", "Hayden", "Hope", "Harrison", "Hailey", "Hudson", "Holly",
            "Isabella", "Isaac", "Ivy", "Ian", "Isla", "Irene", "Ingrid", "Ibrahim", "Indigo", "Iris",
            "Jacob", "James", "Jasmine", "Julian", "Jocelyn", "Jackson", "Jessica", "Joanna", "Jude", "Jenna",
            "Katherine", "Kylie", "Kevin", "Kennedy", "Kai", "Kara", "Kaden", "Kelsey", "Kyle", "Kaitlyn",
            "Liam", "Lily", "Lucas", "Leah", "Logan", "Lillian", "Leo", "Lydia", "Luna", "Lincoln",
            "Mia", "Mason", "Madison", "Mila", "Matthew", "Maya", "Michael", "Molly", "Mason", "Morgan",
            "Nora", "Nathan", "Natalie", "Noah", "Naomi", "Nicholas", "Nina", "Nolan", "Nevaeh", "Nash",
            "Olivia", "Owen", "Oliver", "Olga", "Oscar", "Opal", "Olive", "Onyx", "Octavia", "Omar",
            "Piper", "Peyton", "Preston", "Paige", "Peter", "Phoebe", "Patrick", "Pearl", "Parker", "Penelope",
            "Quinn", "Quincy", "Quintin", "Queenie", "Quincey", "Quiana", "Quinlan", "Quetzal", "Quillan", "Quest",
            "Rachel", "Ryan", "Rebecca", "Robert", "Riley", "Reagan", "Richard", "Rose", "Remington", "Ryder",
            "Sophia", "Samuel", "Samantha", "Sebastian", "Scarlett", "Stella", "Sarah", "Savannah", "Simon", "Sydney",
            "Taylor", "Thomas", "Tessa", "Theodore", "Trinity", "Tyler", "Talia", "Trevor", "Tabitha", "Thaddeus",
            "Ulysses", "Ursula", "Uriah", "Unity", "Umar", "Una", "Ulrich", "Ulla", "Urbain", "Ugo",
            "Victoria", "William", "Violet", "Valentina", "Vincent", "Vanessa", "Vivian", "Vera", "Victor", "Veronica",
            "Walter", "Willow", "Winston", "Whitney", "Wayne", "Winter", "Wendy", "Weston", "Willa", "Wren",
            "Xander", "Ximena", "Xavier", "Xanthe", "Xena", "Xia", "Xylia", "Xyla", "Xerxes", "Xylona",
            "Yasmine", "Yael", "Yara", "Yaretzi", "Yvonne", "Yvette", "Yvaine", "Yelena", "Yara", "Yasmine",
            "Zachary", "Zoe", "Zachariah", "Zara", "Zayden", "Zuri", "Zane", "Zelda", "Zeke", "Zena"
        };

        public MainViewModel()
        {
            ImageWasTappedCommand = new AsyncRelayCommand<object>(ImageTappedAsync, new AsyncRelayCommandOptions());
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
                AppShell.jumpList,
                AppShell.pinView
            };
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

        private async Task ImageTappedAsync(object obj)
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