using Nager.Country;

namespace Samples.InputViews;

public class InputViewModel : MainViewModel
{
    private string _searchCountry = string.Empty;
    private bool _customSearchFunctionSwitchIsToggled;

    public string SearchCountry
    {
        get => _searchCountry;
        set
        {
            _searchCountry = value;
            OnPropertyChanged();
        }
    }

    public List<string> Countries { get; }

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