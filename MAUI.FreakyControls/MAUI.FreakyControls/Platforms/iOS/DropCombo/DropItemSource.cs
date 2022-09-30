using System.Runtime.InteropServices;
using Foundation;
using Maui.FreakyControls;
using Maui.FreakyControls.Shared.Controls;
using Microsoft.Maui.Platform;
using ObjCRuntime;
using UIKit;


namespace SupportWidgetXF.iOS.Renderers.DropCombo
{
    public class DropItemSource : UITableViewSource
    {
        private List<IAutoDropItem> ItemsList;
        private int HeightOfRow;
        private SupportViewDrop ConfigStyle;
        private IDropItemSelected IDropItemSelected;
        private bool IsShowCheckbox = false;

        public DropItemSource(List<IAutoDropItem> _ItemsList, SupportViewDrop _ConfigStyle, int _HeightOfRow,IDropItemSelected dropItemSelected)
        {
            ItemsList = _ItemsList;
            ConfigStyle = _ConfigStyle;
            HeightOfRow = _HeightOfRow;
            IDropItemSelected = dropItemSelected;
        }

        public DropItemSource(List<IAutoDropItem> _ItemsList, SupportViewDrop _ConfigStyle, int _HeightOfRow, IDropItemSelected dropItemSelected, bool _IsShowCheckbox)
        {
            ItemsList = _ItemsList;
            ConfigStyle = _ConfigStyle;
            HeightOfRow = _HeightOfRow;
            IDropItemSelected = dropItemSelected;
            IsShowCheckbox = _IsShowCheckbox;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = ItemsList[indexPath.Row];
            if(ConfigStyle.DropMode == SupportAutoCompleteDropMode.TitleWithDescription)
            {
                var cellChild = tableView.DequeueReusableCell("DropItemTitleDescriptionID") as DropItemTitleDescription;
                cellChild = new DropItemTitleDescription();
                var viewChild = NSBundle.MainBundle.LoadNib("DropItemTitleDescription", cellChild, null);
                cellChild = Runtime.GetNSObject(viewChild.ValueAt(0)) as DropItemTitleDescription;
                cellChild.BindDataToCell(item, delegate {
                    IDropItemSelected.IF_ItemSelectd(indexPath.Row);
                }, ConfigStyle, IsShowCheckbox);
                return cellChild;
            }
            else if (ConfigStyle.DropMode == SupportAutoCompleteDropMode.IconAndTitle)
            {
                var cellChild = tableView.DequeueReusableCell("DropItemIconTitleID") as DropItemIconTitle;
                cellChild = new DropItemIconTitle();
                var viewChild = NSBundle.MainBundle.LoadNib("DropItemIconTitle", cellChild, null);
                cellChild = Runtime.GetNSObject(viewChild.ValueAt(0)) as DropItemIconTitle;
                cellChild.BindDataToCell(item, delegate {
                    IDropItemSelected.IF_ItemSelectd(indexPath.Row);
                }, ConfigStyle, IsShowCheckbox);
                return cellChild;
            }
            else if (ConfigStyle.DropMode == SupportAutoCompleteDropMode.FullTextAndIcon)
            {
                var cellChild = tableView.DequeueReusableCell("DropItemFullTextIconID") as DropItemFullTextIcon;
                cellChild = new DropItemFullTextIcon();
                var viewChild = NSBundle.MainBundle.LoadNib("DropItemFullTextIcon", cellChild, null);
                cellChild = Runtime.GetNSObject(viewChild.ValueAt(0)) as DropItemFullTextIcon;
                cellChild.BindDataToCell(item, delegate {
                    IDropItemSelected.IF_ItemSelectd(indexPath.Row);
                }, ConfigStyle, IsShowCheckbox);
                return cellChild;
            }
            else
            {
                var cellChild = tableView.DequeueReusableCell("DropItemSingleTitleID") as DropItemSingleTitle;
                cellChild = new DropItemSingleTitle();
                var viewChild = NSBundle.MainBundle.LoadNib("DropItemSingleTitle", cellChild, null);
                cellChild = Runtime.GetNSObject(viewChild.ValueAt(0)) as DropItemSingleTitle;
                cellChild.BindDataToCell(item, delegate {
                    IDropItemSelected.IF_ItemSelectd(indexPath.Row);
                }, ConfigStyle, IsShowCheckbox);
                return cellChild;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ItemsList.Count;
        }

        public override NFloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return HeightOfRow;
        }
    }
}