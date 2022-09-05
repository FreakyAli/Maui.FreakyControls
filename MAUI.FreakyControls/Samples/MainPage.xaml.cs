using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Samples;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}


