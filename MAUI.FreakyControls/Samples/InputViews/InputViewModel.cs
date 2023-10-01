using Maui.FreakyControls.Extensions;
using System.Collections.ObjectModel;

namespace Samples.InputViews;

public class InputViewModel : MainViewModel
{
    private ObservableCollection<string> namesCollection;
    private ObservableCollection<AutoCompleteModel> namesCollectionModel;
    private string pin;

    public List<string> Names { get; }
    public List<AutoCompleteModel> NamesModel { get; }


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

    public ObservableCollection<AutoCompleteModel> NamesCollectionModel
    {
        get => namesCollectionModel;
        set => SetProperty(ref namesCollectionModel, value);
    }

    public InputViewModel()
    {
        Names = names.OrderBy(x => x).ToList();
        NamesCollection = Names.ToObservable();
        NamesModel = names.Select(x => new AutoCompleteModel { Name=x}).ToList();
        NamesCollectionModel= NamesModel.ToObservable();
    }
}

public class AutoCompleteModel
{
    public string Name { get; set; }
}