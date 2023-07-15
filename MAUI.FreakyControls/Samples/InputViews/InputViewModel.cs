using System.Windows.Input;
using Nager.Country;

namespace Samples.InputViews;

public class InputViewModel : MainViewModel
{
    private string _searchCountry = string.Empty;
    private bool _customSearchFunctionSwitchIsToggled;
    private bool isBusy;

    public string SearchCountry
    {
        get => _searchCountry;
        set
        {
            _searchCountry = value;
            OnPropertyChanged();
        }
    }

    public bool IsBusy
    {
        get => isBusy;
        set => SetProperty(ref isBusy, value);
    }

    public List<string> Countries { get; }

    public ICommand OnButtonClickedCommand
    {
        get => new Command(()=>IsBusy = !IsBusy);
    }

    public InputViewModel()
    {
        var countryProvider = new CountryProvider();
        var countries = countryProvider.GetCountries().Select(x => x.OfficialName);
        Countries = new List<string>(countries);
    }

    public bool CustomSearchFunctionSwitchIsToggled
    {
        get => _customSearchFunctionSwitchIsToggled;
        set
        {
            _customSearchFunctionSwitchIsToggled = value;
            OnPropertyChanged();
        }
    }
}