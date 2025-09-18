using CommunityToolkit.Mvvm.ComponentModel;
using FreakyKit.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Samples.InputViews;

public partial class InputViewModel : MainViewModel
{
    private ObservableCollection<string> namesCollection;
    private ObservableCollection<AutoCompleteModel> namesCollectionModel;
    private string pin;

    public List<AutoCompleteModel> NamesModel { get; }

    public ICommand EntryCompleteCommand { get; set; }
    public ICommand SwitchImageCommand { get; set; }

    [ObservableProperty]
    private string imageSource;


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
        ImageSource = "calendar";
        NamesModel = [.. names.Select(x => new AutoCompleteModel { Name = x })];
        NamesCollectionModel = NamesModel.ToObservable();

        EntryCompleteCommand = new Command(ExecuteEntryCompleteCommand);
        SwitchImageCommand = new Command(ExecuteSwitchImageCommand);
    }

    private void ExecuteSwitchImageCommand()
    {
        this.ImageSource = this.ImageSource == "backspace" ?  "calendar" : "backspace";
    }

    private void ExecuteEntryCompleteCommand(object obj)
    {
    }
}