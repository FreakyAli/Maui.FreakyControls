using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Nager.Country;
using static System.Net.Mime.MediaTypeNames;
using Maui.FreakyControls;

namespace Samples.InputViews
{
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
            var countries = countryProvider.GetCountries().Select(x=>x.OfficialName);
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
}

