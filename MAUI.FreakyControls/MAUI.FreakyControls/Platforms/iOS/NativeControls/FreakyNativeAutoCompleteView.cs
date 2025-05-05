using Foundation;
using Maui.FreakyControls.Enums;
using Microsoft.Maui.Platform;
using UIKit;
using CoreFoundation;

namespace Maui.FreakyControls.Platforms.iOS.NativeControls;

public partial class FreakyNativeAutoCompleteView : UIView
{
    private nfloat keyboardHeight;
    private NSLayoutConstraint bottomConstraint;
    private Func<object, string> textFunc;

    private nfloat _suggestionListHeight = -1;
    private nfloat _suggestionListWidth = -1;

    public nfloat SuggestionListHeight
    {
        get => _suggestionListHeight;
        set
        {
            _suggestionListHeight = value;
            if (_isSuggestionListOpen)
                UpdateSuggestionListOpenState();
        }
    }

    public nfloat SuggestionListWidth
    {
        get => _suggestionListWidth;
        set
        {
            _suggestionListWidth = value;
            if (_isSuggestionListOpen)
                UpdateSuggestionListOpenState();
        }
    }

    public FreakyUITextfield InputTextField { get; }

    public UITableView SelectionList { get; }

    public int Threshold { get; set; }

    public FreakyNativeAutoCompleteView()
    {
        InputTextField = new FreakyUITextfield
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
            BorderStyle = UITextBorderStyle.None,
            ReturnKeyType = UIReturnKeyType.Search,
            AutocorrectionType = UITextAutocorrectionType.No,
            ShouldReturn = InputText_OnShouldReturn,
            ClipsToBounds = true,
        };

        Threshold = 1;

        InputTextField.Layer.BorderWidth = 0;
        InputTextField.Layer.BorderColor = UIColor.Clear.CGColor;
        InputTextField.EditingDidBegin += OnEditingDidBegin;
        InputTextField.EditingDidEnd += OnEditingDidEnd;
        InputTextField.EditingChanged += InputText_EditingChanged;

        AddSubview(InputTextField);

        InputTextField.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
        InputTextField.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
        InputTextField.WidthAnchor.ConstraintEqualTo(WidthAnchor).Active = true;
        InputTextField.HeightAnchor.ConstraintEqualTo(HeightAnchor).Active = true;

        SelectionList = new UITableView { TranslatesAutoresizingMaskIntoConstraints = false };

        UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
        UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
    }

    public override void MovedToWindow()
    {
        base.MovedToWindow();
        UpdateSuggestionListOpenState();
    }

    private void OnEditingDidBegin(object sender, EventArgs e)
    {
        if (InputTextField.Text.Length > Threshold)
            IsSuggestionListOpen = true;

        EditingDidBegin?.Invoke(this, e);
    }

    private void OnEditingDidEnd(object sender, EventArgs e)
    {
        IsSuggestionListOpen = false;
        EditingDidEnd?.Invoke(this, e);
    }

    internal EventHandler EditingDidBegin;
    internal EventHandler EditingDidEnd;

    public virtual UIFont Font
    {
        get => InputTextField.Font;
        set => InputTextField.Font = value;
    }

    internal void SetItems(IEnumerable<object> items, Func<object, string> labelFunc, Func<object, string> textFunc)
    {
        this.textFunc = textFunc;

        if (SelectionList.Source is TableSource<object> oldSource)
            oldSource.TableRowSelected -= SuggestionTableSource_TableRowSelected;

        SelectionList.Source = null;

        var suggestions = items?.OfType<object>();
        if (suggestions is not null && suggestions.Any())
        {
            var suggestionTableSource = new TableSource<object>(suggestions, labelFunc);
            suggestionTableSource.TableRowSelected += SuggestionTableSource_TableRowSelected;
            SelectionList.Source = suggestionTableSource;
            SelectionList.ReloadData();

            if (InputTextField.Text.Length > Threshold)
                IsSuggestionListOpen = true;
        }
        else
        {
            IsSuggestionListOpen = false;
        }
    }

    public virtual string Placeholder
    {
        get => InputTextField.Placeholder;
        set => InputTextField.Placeholder = value;
    }

    public virtual void SetPlaceholderColor(Color color)
    {
        InputTextField.AttributedPlaceholder = new NSAttributedString(
            InputTextField.Placeholder ?? string.Empty,
            null,
            color.ToPlatform());
    }

    private bool _isSuggestionListOpen;

    public virtual bool IsSuggestionListOpen
    {
        get => _isSuggestionListOpen;
        set
        {
            _isSuggestionListOpen = value;
            UpdateSuggestionListOpenState();
        }
    }

    private void UpdateSuggestionListOpenState()
    {
        if (_isSuggestionListOpen && SelectionList.Source is not null && SelectionList.Source.RowsInSection(SelectionList, 0) > 0)
        {
            var viewController = InputTextField.Window?.RootViewController;
            if (viewController is null)
                return;
            if (viewController.PresentedViewController is not null)
                viewController = viewController.PresentedViewController;

            if (SelectionList.Superview is null)
            {
                viewController.Add(SelectionList);
            }

            SelectionList.RemoveConstraints(SelectionList.Constraints);
            if (bottomConstraint != null)
            {
                bottomConstraint.Active = false;
            }

            SelectionList.TranslatesAutoresizingMaskIntoConstraints = false;
            SelectionList.TopAnchor.ConstraintEqualTo(InputTextField.BottomAnchor).Active = true;
            SelectionList.LeftAnchor.ConstraintEqualTo(InputTextField.LeftAnchor).Active = true;
            SelectionList.WidthAnchor.ConstraintEqualTo(InputTextField.WidthAnchor).Active = true;

            bottomConstraint = SelectionList.BottomAnchor.ConstraintLessThanOrEqualTo(SelectionList.Superview.BottomAnchor, -keyboardHeight);
            bottomConstraint.Priority = 999;
            bottomConstraint.Active = true;

            // Optional: Apply SuggestionHeight limit
            if (SuggestionListHeight > 0)
            {
                var heightConstraint = SelectionList.HeightAnchor.ConstraintLessThanOrEqualTo(SuggestionListHeight);
                heightConstraint.Priority = 1000;
                heightConstraint.Active = true;
            }

            SelectionList.UpdateConstraints();
            SelectionList.LayoutIfNeeded();
        }
        else
        {
            if (SelectionList.Superview is not null)
            {
                SelectionList.RemoveFromSuperview();
            }
        }
    }
    
    public virtual bool UpdateTextOnSelect { get; set; } = true;

    private void OnKeyboardHide(object sender, UIKeyboardEventArgs e)
    {
        keyboardHeight = 0;
        if (bottomConstraint != null)
        {
            bottomConstraint.Constant = keyboardHeight;
            SelectionList.UpdateConstraints();
        }
    }

    private void OnKeyboardShow(object sender, UIKeyboardEventArgs e)
    {
        var nsKeyboardBounds = (NSValue)e.Notification.UserInfo.ObjectForKey(UIKeyboard.FrameBeginUserInfoKey);
        var keyboardBounds = nsKeyboardBounds.RectangleFValue;
        keyboardHeight = keyboardBounds.Height;

        if (bottomConstraint != null)
        {
            bottomConstraint.Constant = -keyboardHeight;
            SelectionList.UpdateConstraints();
        }
    }

    private bool InputText_OnShouldReturn(UITextField field)
    {
        if (string.IsNullOrWhiteSpace(field.Text)) return false;

        field.ResignFirstResponder();
        QuerySubmitted?.Invoke(this, new FreakyAutoCompleteViewQuerySubmittedEventArgs(InputTextField.Text, null));
        return true;
    }

    public override bool BecomeFirstResponder() => InputTextField.BecomeFirstResponder();

    public override bool ResignFirstResponder() => InputTextField.ResignFirstResponder();

    public override bool IsFirstResponder => InputTextField.IsFirstResponder;

    private void SuggestionTableSource_TableRowSelected(object sender, TableRowSelectedEventArgs<object> e)
    {
        SelectionList.DeselectRow(e.SelectedItemIndexPath, false);
        var selection = e.SelectedItem;

        if (UpdateTextOnSelect)
        {
            InputTextField.Text = textFunc(selection);
            TextChanged?.Invoke(this, new FreakyAutoCompleteViewTextChangedEventArgs(textFunc(selection), TextChangeReason.SuggestionChosen));
        }

        SuggestionChosen?.Invoke(this, new FreakyAutoCompleteViewSuggestionChosenEventArgs(selection));
        QuerySubmitted?.Invoke(this, new FreakyAutoCompleteViewQuerySubmittedEventArgs(Text, selection));

        IsSuggestionListOpen = false;
    }

    private void InputText_EditingChanged(object sender, EventArgs e)
    {
        TextChanged?.Invoke(this, new FreakyAutoCompleteViewTextChangedEventArgs(Text, TextChangeReason.UserInput));
        if (InputTextField.Text.Length > Threshold)
            IsSuggestionListOpen = true;
    }

    public virtual string Text
    {
        get => InputTextField.Text;
        set
        {
            InputTextField.Text = value;
            TextChanged?.Invoke(this, new FreakyAutoCompleteViewTextChangedEventArgs(value, TextChangeReason.ProgrammaticChange));
        }
    }

    public virtual void SetTextColor(Color color)
    {
        InputTextField.TextColor = color.ToPlatform();
    }

    public event EventHandler<FreakyAutoCompleteViewTextChangedEventArgs> TextChanged;
    public event EventHandler<FreakyAutoCompleteViewQuerySubmittedEventArgs> QuerySubmitted;
    public event EventHandler<FreakyAutoCompleteViewSuggestionChosenEventArgs> SuggestionChosen;

    private class TableSource<T> : UITableViewSource
    {
        readonly IEnumerable<T> _items;
        readonly Func<T, string> _labelFunc;
        readonly string _cellIdentifier;

        public TableSource(IEnumerable<T> items, Func<T, string> labelFunc)
        {
            _items = items;
            _labelFunc = labelFunc;
            _cellIdentifier = Guid.NewGuid().ToString();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(_cellIdentifier) ?? new UITableViewCell(UITableViewCellStyle.Default, _cellIdentifier);
            var item = _items.ElementAt(indexPath.Row);
            var content = cell.DefaultContentConfiguration;
            content.Text = _labelFunc(item);
            cell.ContentConfiguration = content;
            cell.AutomaticallyUpdatesContentConfiguration = false;
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath) =>
            TableRowSelected?.Invoke(this, new TableRowSelectedEventArgs<T>(_items.ElementAt(indexPath.Row), _labelFunc(_items.ElementAt(indexPath.Row)), indexPath));

        public override nint RowsInSection(UITableView tableview, nint section) => _items.Count();

        public event EventHandler<TableRowSelectedEventArgs<T>> TableRowSelected;
    }

    private class TableRowSelectedEventArgs<T> : EventArgs
    {
        public TableRowSelectedEventArgs(T selectedItem, string selectedItemLabel, NSIndexPath selectedItemIndexPath)
        {
            SelectedItem = selectedItem;
            SelectedItemLabel = selectedItemLabel;
            SelectedItemIndexPath = selectedItemIndexPath;
        }

        public T SelectedItem { get; }
        public string SelectedItemLabel { get; }
        public NSIndexPath SelectedItemIndexPath { get; }
    }
}
