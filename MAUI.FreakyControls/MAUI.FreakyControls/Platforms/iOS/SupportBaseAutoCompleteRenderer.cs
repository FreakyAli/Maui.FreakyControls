using System;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using UIKit;
using Maui.FreakyControls.Shared.Controls;
using Microsoft.Maui.Controls.Platform;
using System.Runtime.InteropServices;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using ObjCRuntime;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using SupportWidgetXF.iOS.Renderers.DropCombo;

namespace Maui.FreakyControls.Platforms.iOS;

public class SupportDropRenderer<TSupport> : Microsoft.Maui.Controls.Handlers.Compatibility.ViewRenderer<TSupport, UIView>, IDropItemSelected where TSupport : SupportViewDrop
{
    protected TSupport SupportView;
    protected int HeightOfRow = 40;

    protected UITableView tableView;
    protected UITextField textField;
    protected bool FlagShow = false;

    protected List<IAutoDropItem> SupportItemList = new List<IAutoDropItem>();
    protected DropItemSource dropSource;

    public virtual void SyncItemSource()
    {
        SupportItemList.Clear();
        if (SupportView.ItemsSource != null)
        {
            SupportItemList.AddRange(SupportView.ItemsSource.ToList());
        }
        NotifyAdapterChanged();
    }

    public virtual void NotifyAdapterChanged()
    {
        if (tableView != null)
            tableView.ReloadData();
    }

    public virtual void OnInitialize()
    {

    }

    public virtual void IF_ItemSelectd(int position)
    {
        ShowData();
        if (position >= 0 && position < SupportItemList.Count)
        {
            var text = SupportItemList[position].IF_GetTitle();
            SupportView.Text = text;
        }
    }

    public virtual void OnInitializeTextField()
    {
        textField = new UITextField();
        textField.Frame = this.Frame;
        textField.Layer.CornerRadius = (float)SupportView.CornerRadius;
        textField.Layer.BorderWidth = (float)SupportView.CornerWidth;

        textField.Layer.BackgroundColor = SupportView.BackgroundColor.ToPlatform().CGColor;
        SupportView.BackgroundColor = Colors.Transparent;

        textField.Layer.BorderColor = SupportView.CornerColor.ToCGColor();
        textField.Font = UIFont.FromName(SupportView.FontFamily, (float)SupportView.FontSize);
        textField.Text = SupportView.Text;
    }

    public virtual void OnInitializeTableView()
    {
        tableView = new UITableView();
        tableView.AutoresizingMask = UIViewAutoresizing.All;
        tableView.Frame = textField.Frame;
        tableView.SeparatorColor = UIColor.Clear;
    }

    public virtual void OnInitializeTableSource()
    {
        dropSource = new DropItemSource(SupportItemList, SupportView, HeightOfRow, this);
        tableView.Source = dropSource;
    }

    public virtual void ShowData()
    {
        if (textField == null)
            return;

        FlagShow = !FlagShow;
        if (FlagShow)
        {
            var rect = textField.ConvertRectToView(textField.Frame, Window);
            NFloat height = Window.Bounds.Height - rect.Y - 10;
            CGRect r = new CGRect(rect.X, rect.Y, rect.Width, height);

            ShowSubviewAt(r, tableView, () =>
            {
                tableView.Layer.MasksToBounds = !SupportView.HasShadow;
            });
        }
        else
        {
            HideData();
        }
    }

    public virtual void HideData()
    {
        if (tableView != null)
            tableView.RemoveFromSuperview();
    }

    public UIWindow GetCurrentWindow(UIView view)
    {
        if (view.Superview is UIWindow)
            return (UIWindow)view.Superview;
        else return GetCurrentWindow(view.Superview);
    }

    public virtual void ShowSubviewAt(CGRect rect, UIView subView, Action didFinishAnimation)
    {
        float height = HeightOfRow * SupportItemList.Count();
        var y = rect.Y + textField.Frame.Height + 2;
        if (height > rect.Height / 2)
            height = (float)rect.Height / 2;

        subView.Frame = new CGRect(rect.X, y, rect.Width, 0);
        UIView.Animate(0.2, () =>
        {
            subView.Frame = new CGRect(rect.X, y, rect.Width, height);
            subView.SetShadow(2f, 2, 0.8f);
            Window.AddSubview(subView);
        }, didFinishAnimation);
    }

    protected virtual void OnSetNativeControl()
    {
        SetNativeControl(textField);
    }

    protected override void OnElementChanged(ElementChangedEventArgs<TSupport> e)
    {
        base.OnElementChanged(e);
        if (e.NewElement != null && e.NewElement is TSupport)
        {
            SupportView = e.NewElement as TSupport;
            if (Control == null)
            {
                OnInitializeTextField();
                OnInitializeTableView();
                SyncItemSource();
                OnInitializeTableSource();
                OnSetNativeControl();
            }
        }
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);
        if (e.PropertyName.Equals(SupportViewBase.TextProperty.PropertyName))
        {
            if (textField != null && !textField.Text.Equals(SupportView.Text))
            {
                textField.Text = SupportView.Text;
            }
        }
        else if (e.PropertyName.Equals(SupportViewDrop.ItemsSourceProperty.PropertyName))
        {
            SyncItemSource();
        }
        else if (e.PropertyName.Equals(SupportViewDrop.RefreshListProperty.PropertyName))
        {
            NotifyAdapterChanged();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && tableView != null) HideData();
        base.Dispose(disposing);
    }
}

public static class UIViewExtensions
{
    public static UIColor ColorSeperator = UIColor.FromRGB(232, 234, 238);

    public static void SetShadow(this UIView subView, NFloat Radius, NFloat size, float Opacity)
    {
        subView.Layer.ShadowRadius = Radius;
        subView.Layer.ShadowColor = ColorSeperator.CGColor;
        subView.Layer.ShadowOffset = new CGSize(size, size);
        subView.Layer.ShadowOpacity = Opacity;
        subView.Layer.ShadowPath = UIBezierPath.FromRect(subView.Layer.Bounds).CGPath;
        subView.Layer.MasksToBounds = true;
    }

    public static CGRect ResyncViewPosition(this CGRect cGRect, UIWindow window, int MinWidth, int ExtendWidth)
    {
        if (cGRect.Width < MinWidth)
            cGRect.Width = MinWidth;

        if (cGRect.X >= window.Bounds.Width - MinWidth)
            cGRect.X = window.Bounds.Width - MinWidth - ExtendWidth;

        if (cGRect.Y >= window.Bounds.Height - cGRect.Height)
            cGRect.Y = window.Bounds.Height - cGRect.Height - ExtendWidth;

        return cGRect;
    }

    public static void InitlizeReturnKey(this UITextField uITextField, SupportEntryReturnType returnType)
    {
        switch (returnType)
        {
            case SupportEntryReturnType.Go:
                uITextField.ReturnKeyType = UIReturnKeyType.Go;
                break;
            case SupportEntryReturnType.Next:
                uITextField.ReturnKeyType = UIReturnKeyType.Next;
                break;
            case SupportEntryReturnType.Send:
                uITextField.ReturnKeyType = UIReturnKeyType.Send;
                break;
            case SupportEntryReturnType.Search:
                uITextField.ReturnKeyType = UIReturnKeyType.Search;
                break;
            case SupportEntryReturnType.Done:
                uITextField.ReturnKeyType = UIReturnKeyType.Done;
                break;
            default:
                uITextField.ReturnKeyType = UIReturnKeyType.Default;
                break;
        }
    }
}

public class DropItemSource : UITableViewSource
{
    private List<IAutoDropItem> ItemsList;
    private int HeightOfRow;
    private SupportViewDrop ConfigStyle;
    private IDropItemSelected IDropItemSelected;
    private bool IsShowCheckbox = false;

    public DropItemSource(List<IAutoDropItem> _ItemsList, SupportViewDrop _ConfigStyle, int _HeightOfRow, IDropItemSelected dropItemSelected)
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
        if (ConfigStyle.DropMode == SupportAutoCompleteDropMode.TitleWithDescription)
        {
            var cellChild = tableView.DequeueReusableCell("DropItemTitleDescriptionID") as DropItemTitleDescription;
            cellChild = new DropItemTitleDescription();
            var viewChild = NSBundle.MainBundle.LoadNib("DropItemTitleDescription", cellChild, null);
            cellChild = Runtime.GetNSObject(viewChild.ValueAt(0)) as DropItemTitleDescription;
            cellChild.BindDataToCell(item, delegate
            {
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
            cellChild.BindDataToCell(item, delegate
            {
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
            cellChild.BindDataToCell(item, delegate
            {
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
            cellChild.BindDataToCell(item, delegate
            {
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

public class SupportBaseAutoCompleteRenderer<TSupportAutoComplete> : SupportDropRenderer<TSupportAutoComplete>
   where TSupportAutoComplete : SupportAutoComplete

{
    public SupportBaseAutoCompleteRenderer()
    {
    }

    protected virtual void RunFilterAutocomplete(string text)
    {
        SupportItemList.Clear();
        if (text != null && text.Length > 1 && SupportView.ItemsSource != null)
        {
            var key = text.ToLower();
            var result = SupportView.ItemsSource.ToList().Where(x => x.IF_GetTitle().ToLower().Contains(key)).Take(30);
            SupportItemList.AddRange(result);

            var Count = SupportItemList.Count;
            if (Count > 0)
            {
                FlagShow = false;
                tableView.ReloadData();
                ShowData();
            }
        }
        else
        {
            HideData();
        }
    }

    protected virtual void OnInitializePlaceHolderTextField()
    {
        if (textField != null)
        {
            textField.AttributedPlaceholder = new NSAttributedString(SupportView.Placeholder, font: UIFont.FromName(SupportView.FontFamily, size: (float)SupportView.FontSize), foregroundColor: SupportView.PlaceHolderColor.ToUIColor());
            textField.Placeholder = SupportView.Placeholder;
        }
    }

    public override void OnInitializeTextField()
    {
        base.OnInitializeTextField();
        OnInitializePlaceHolderTextField();
        textField.LeftView = new UIView(new CGRect(0, 0, SupportView.PaddingInside, 0));
        textField.LeftViewMode = UITextFieldViewMode.Always;

        textField.EditingChanged += Wrapper_EditingChanged; ;
        textField.ShouldEndEditing += Wrapper_ShouldEndEditing;
        textField.ShouldBeginEditing += Wrapper_ShouldBeginEditing;
        textField.ShouldReturn += (textField) =>
        {
            SupportView.SendOnReturnKeyClicked();
            return true;
        };
        textField.InitlizeReturnKey(SupportView.ReturnType);
    }

    protected virtual void Wrapper_EditingChanged(object sender, EventArgs e)
    {
        var textFieldInput = sender as UITextField;
        SupportView.SendOnTextChanged(textFieldInput.Text);
        RunFilterAutocomplete(textFieldInput.Text);
    }

    protected virtual bool Wrapper_ShouldBeginEditing(UITextField textFieldInput)
    {
        SupportView.SendOnTextFocused(true);
        return true;
    }

    protected virtual bool Wrapper_ShouldEndEditing(UITextField textFieldInput)
    {
        HideData();
        SupportView.SendOnTextFocused(false);
        return true;
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);
        if (e.PropertyName.Equals(SupportAutoComplete.CurrentCornerColorProperty.PropertyName))
        {
            textField.Layer.BorderColor = SupportView.CurrentCornerColor.ToCGColor();
        }
        else if (e.PropertyName.Equals(SupportAutoComplete.PlaceHolderColorProperty.PropertyName))
        {
            OnInitializePlaceHolderTextField();
        }
    }
}

public class SupportAutoCompleteRenderer : SupportBaseAutoCompleteRenderer<SupportAutoComplete>
{

}
