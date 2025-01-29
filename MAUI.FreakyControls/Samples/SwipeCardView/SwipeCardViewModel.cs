using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Samples.SwipeCardView;

public partial class SwipeCardViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<Profile> profiles = new ObservableCollection<Profile>();

    public SwipeCardViewModel()
    {
        InitializeProfiles();
    }

    public ICommand SwipedCommand { get; }

    public ICommand DraggingCommand { get; }

    public ICommand ClearItemsCommand { get; }

    public ICommand AddItemsCommand { get; }

    private void InitializeProfiles()
    {
        Profiles.Add(new Profile { ProfileId = 1, Name = "Laura", Age = 24, Gender = Gender.Female, Photo = ImageUrls.Female1});
        Profiles.Add(new Profile { ProfileId = 2, Name = "Sophia", Age = 21, Gender = Gender.Female, Photo = ImageUrls.Female2 });
        Profiles.Add(new Profile { ProfileId = 3, Name = "Anne", Age = 19, Gender = Gender.Female, Photo = ImageUrls.Female3 });
        Profiles.Add(new Profile { ProfileId = 4, Name = "Yvonne ", Age = 27, Gender = Gender.Female, Photo = ImageUrls.Female4});
        Profiles.Add(new Profile { ProfileId = 5, Name = "Abby", Age = 25, Gender = Gender.Female, Photo = ImageUrls.Female5 });
        Profiles.Add(new Profile { ProfileId = 6, Name = "Andressa", Age = 28, Gender = Gender.Female, Photo = ImageUrls.Female6 });
        Profiles.Add(new Profile { ProfileId = 7, Name = "June", Age = 29, Gender = Gender.Female, Photo = ImageUrls.Female7 });
        Profiles.Add(new Profile { ProfileId = 8, Name = "Kim", Age = 22, Gender = Gender.Female, Photo = ImageUrls.Female8 });
        Profiles.Add(new Profile { ProfileId = 9, Name = "Denesha", Age = 26, Gender = Gender.Female, Photo = ImageUrls.Female9});
        Profiles.Add(new Profile { ProfileId = 10, Name = "Sasha", Age = 23, Gender = Gender.Female, Photo = ImageUrls.Female10});

        Profiles.Add(new Profile { ProfileId = 11, Name = "Austin", Age = 28, Gender = Gender.Male, Photo = ImageUrls.Male1 });
        Profiles.Add(new Profile { ProfileId = 11, Name = "James", Age = 32, Gender = Gender.Male, Photo = ImageUrls.Male2 });
        Profiles.Add(new Profile { ProfileId = 11, Name = "Chris", Age = 27, Gender = Gender.Male, Photo = ImageUrls.Male3 });
        Profiles.Add(new Profile { ProfileId = 11, Name = "Alexander", Age = 30, Gender = Gender.Male, Photo = ImageUrls.Male4 });
        Profiles.Add(new Profile { ProfileId = 11, Name = "Steve", Age = 31, Gender = Gender.Male, Photo = ImageUrls.Male5 });
    }
}
