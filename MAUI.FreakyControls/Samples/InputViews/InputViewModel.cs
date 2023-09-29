using Maui.FreakyControls.Extensions;
using System.Collections.ObjectModel;

namespace Samples.InputViews;

public class InputViewModel : MainViewModel
{
    private ObservableCollection<string> namesCollection;
    private string pin;

    public List<string> Names { get; }

    public string Pin
    {
        get => pin;
        set => SetProperty(ref pin, value);
    }

    public ObservableCollection<string> NamesCollection
    {
        get => namesCollection;
        set => SetProperty(ref namesCollection, value);
    }

    public InputViewModel()
    {
        Names = names.OrderBy(x => x).ToList();
        NamesCollection = Names.ToObservable();
    }
}