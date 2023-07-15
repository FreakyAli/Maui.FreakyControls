using Nager.Country;
using System.Windows.Input;

namespace Samples.InputViews;

public class InputViewModel : MainViewModel
{
    private bool _customSearchFunctionSwitchIsToggled;
    private string _searchCountry = string.Empty;
    private bool isBusy;

    public InputViewModel()
    {
        var countryProvider = new CountryProvider();
        var countries = countryProvider.GetCountries().Select(x => x.OfficialName);
        Countries = new List<string>(countries);
    }

    public List<string> Countries { get; }

    public bool CustomSearchFunctionSwitchIsToggled
    {
        get => _customSearchFunctionSwitchIsToggled;
        set
        {
            _customSearchFunctionSwitchIsToggled = value;
            OnPropertyChanged();
        }
    }

    public bool IsBusy
    {
        get => isBusy;
        set => SetProperty(ref isBusy, value);
    }

    public ICommand OnButtonClickedCommand
    {
        get => new Command(() => IsBusy = !IsBusy);
    }

    public string SearchCountry
    {
        get => _searchCountry;
        set
        {
            _searchCountry = value;
            OnPropertyChanged();
        }
    }
}