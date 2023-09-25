using System.Collections.ObjectModel;
using Maui.FreakyControls.Extensions;

namespace Samples.InputViews;

public class InputViewModel : MainViewModel
{
    private ObservableCollection<string> namesCollection;

    public List<string> Names { get; }


    public ObservableCollection<string> NamesCollection
    {
        get => namesCollection;
        set =>SetProperty(ref namesCollection , value);
    }

    public InputViewModel()
    {
        Names = names.OrderBy(x => x).ToList();
        NamesCollection = Names.ToObservable();
    }
}