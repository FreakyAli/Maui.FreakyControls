using Foundation;
using UIKit;

namespace Maui.FreakyControls.Platforms.iOS;

public abstract class AutoCompleteViewSource : UITableViewSource
{
    public ICollection<string> Suggestions { get; set; } = new List<string>();

    public NativeAutoCompleteView AutoCompleteTextField { get; set; }

    public abstract void UpdateSuggestions(ICollection<string> suggestions);

    public abstract override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath);

    public override nint RowsInSection(UITableView tableview, nint section)
    {
        return Suggestions.Count;
    }

    public event EventHandler<SelectedItemChangedEventArgs> Selected;

    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
    {
        AutoCompleteTextField.AutoCompleteTableView.Hidden = true;
        if (indexPath.Row < Suggestions.Count)
            AutoCompleteTextField.Text = Suggestions.ElementAt(indexPath.Row);
        AutoCompleteTextField.ResignFirstResponder();
        var item = Suggestions.ToList()[(int)indexPath.Item];
        Selected?.Invoke(tableView, new SelectedItemChangedEventArgs(item, -1));
        // don't call base.RowSelected
    }
}


