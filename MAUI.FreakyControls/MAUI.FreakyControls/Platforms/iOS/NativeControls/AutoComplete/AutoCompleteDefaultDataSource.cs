using Foundation;
using UIKit;

namespace Maui.FreakyControls.Platforms.iOS;

public class AutoCompleteDefaultDataSource : AutoCompleteViewSource
{
    private const string _cellIdentifier = "DefaultIdentifier";

    public override void UpdateSuggestions(ICollection<string> suggestions)
    {
        Suggestions = suggestions;
    }

    public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
    {
        var cell = tableView.DequeueReusableCell(_cellIdentifier);
        var item = Suggestions.ElementAt(indexPath.Row);

        if (cell == null)
            cell = new UITableViewCell(UITableViewCellStyle.Default, _cellIdentifier);

        cell.BackgroundColor = UIColor.Clear;
        cell.TextLabel.Text = item;

        return cell;
    }
}


