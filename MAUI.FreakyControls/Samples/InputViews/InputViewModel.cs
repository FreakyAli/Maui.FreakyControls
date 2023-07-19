
namespace Samples.InputViews;

public class InputViewModel : MainViewModel
{
    private bool _customSearchFunctionSwitchIsToggled;

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