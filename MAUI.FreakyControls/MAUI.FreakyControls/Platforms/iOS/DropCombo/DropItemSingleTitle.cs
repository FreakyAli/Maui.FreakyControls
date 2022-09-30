using Foundation;
using Maui.FreakyControls.Shared.Controls;
using Microsoft.Maui.Platform;
using UIKit;


namespace SupportWidgetXF.iOS.Renderers.DropCombo
{
    public partial class DropItemSingleTitle : UITableViewCell
    {
        public static readonly NSString Key = new NSString("DropItemSingleTitle");
        public static readonly UINib Nib;

        static DropItemSingleTitle()
        {
            Nib = UINib.FromName("DropItemSingleTitle", NSBundle.MainBundle);
        }

        protected DropItemSingleTitle(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public DropItemSingleTitle() { }

        private Action ActionClick;

        public void BindDataToCell(IAutoDropItem dropItem,  Action action, SupportViewDrop _ConfigStyle, bool _ShowCheckBox = false)
        {
            try
            {
                txtTitle.Text = dropItem.IF_GetTitle();
                txtSeperator.BackgroundColor = _ConfigStyle.SeperatorColor.ToPlatform();
                NsHeightSeperator.Constant = _ConfigStyle.SeperatorHeight;
                txtTitle.TextColor = _ConfigStyle.TextColor.ToPlatform();

                if (_ShowCheckBox)
                {
                    NSSpaceBetWeen.Constant = 5;
                    NSSizeOfCheckbox.Constant = 25;
                    cbxCheckBox.Checked = dropItem.IF_GetChecked();
                    cbxCheckBox.Hidden = false;
                }
                else
                {
                    NSSpaceBetWeen.Constant = 0;
                    NSSizeOfCheckbox.Constant = 0;
                    cbxCheckBox.Hidden = true;
                }

                if (ActionClick == null)
                {
                    ActionClick = action;
                    bttClick.TouchUpInside += (sender, e) =>
                    {
                        ActionClick();
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}