using Maui.FreakyControls.Extensions;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Samples.InputViews;

public class InputViewModel : MainViewModel
{
    private ObservableCollection<string> namesCollection;
    private ObservableCollection<AutoCompleteModel> namesCollectionModel;
    private string pin;

    public List<AutoCompleteModel> NamesModel { get; }

    public ICommand EntryCompleteCommand { get; set; }

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
        NamesCollection = Names.ToObservable();

        NamesModel = names.Select(x => new AutoCompleteModel { Name = x }).ToList();
        NamesCollectionModel = NamesModel.ToObservable();

        EntryCompleteCommand = new Command(ExecuteEntryCompleteCommand);
    }

    private void ExecuteEntryCompleteCommand(object obj)
    {
    }
}