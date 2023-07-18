using Nager.Country;

namespace Samples.InputViews;

public class InputViewModel : MainViewModel
{
    private bool _customSearchFunctionSwitchIsToggled;
    private string _searchCountry = string.Empty;

    public InputViewModel()
    {
        var countryProvider = new CountryProvider();
        var countries = countryProvider.GetCountries().Select(x => x.OfficialName);
        Countries = new List<string>(countries);
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
}