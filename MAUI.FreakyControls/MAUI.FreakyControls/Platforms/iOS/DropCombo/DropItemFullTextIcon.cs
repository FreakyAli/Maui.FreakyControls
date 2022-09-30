using Foundation;
using Maui.FreakyControls.Shared.Controls;
using Microsoft.Maui.Platform;
using UIKit;

namespace SupportWidgetXF.iOS.Renderers.DropCombo
{

    public partial class DropItemFullTextIcon : UITableViewCell
    {
        public static readonly NSString Key = new NSString("DropItemFullTextIcon");
        public static readonly UINib Nib;

        static DropItemFullTextIcon()
        {
            Nib = UINib.FromName("DropItemFullTextIcon", NSBundle.MainBundle);
        }

        protected DropItemFullTextIcon(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public DropItemFullTextIcon() { }

        private Action ActionClick;

        public void BindDataToCell(IAutoDropItem dropItem, Action action, SupportViewDrop _ConfigStyle, bool _ShowCheckBox = false)
        {
            try
            {
                txtTitle.Text = dropItem.IF_GetTitle();
                txtDescription.Text = dropItem.IF_GetDescription();
                txtSeperator.BackgroundColor = _ConfigStyle.SeperatorColor.ToPlatform();
                NsHeightSeperator.Constant = _ConfigStyle.SeperatorHeight;
                imgIcon.Image = UIImage.FromBundle(dropItem.IF_GetIcon()).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                txtTitle.TextColor = _ConfigStyle.TextColor.ToPlatform();
                txtDescription.TextColor = _ConfigStyle.DescriptionTextColor.ToPlatform();

                if(_ShowCheckBox)
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
