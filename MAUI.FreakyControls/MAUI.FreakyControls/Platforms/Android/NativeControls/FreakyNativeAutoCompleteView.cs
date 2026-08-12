using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Java.Lang;
using Maui.FreakyControls.Enums;
using Microsoft.Maui.Platform;
using Rect = Android.Graphics.Rect;

namespace Maui.FreakyControls.Platforms.Android.NativeControls;

public class FreakyNativeAutoCompleteView : AppCompatAutoCompleteTextView
{
    private bool suppressTextChangedEvent;
    private Func<object, string>? textFunc;
    private SuggestCompleteAdapter adapter;
    private Drawable? drawableRight;
    private Drawable? drawableLeft;
    private Drawable? drawableTop;
    private Drawable? drawableBottom;
    private int actionX, actionY;
    private IDrawableClickListener? clickListener;

    public FreakyNativeAutoCompleteView(Context context) : base(context)
    {
        SetMaxLines(1);
        InputType = global::Android.Text.InputTypes.TextFlagNoSuggestions | global::Android.Text.InputTypes.TextVariationVisiblePassword; //Disables text suggestions as the auto-complete view is there to do that
        ItemClick += OnItemClick;
        Adapter = adapter = new SuggestCompleteAdapter(context, global::Android.Resource.Layout.SimpleDropDownItem1Line);
    }

    public override bool EnoughToFilter() => true;

    internal void SetItems(IEnumerable<object>? items, Func<object, string> labelFunc, Func<object, string> textFunc)
    {
        this.textFunc = textFunc;
        if (items is null)
            adapter.UpdateList(Enumerable.Empty<string>(), labelFunc);
        else
            adapter.UpdateList(items.OfType<object>(), labelFunc);
    }

    public virtual new string? Text
    {
        get => base.Text;
        set
        {
            suppressTextChangedEvent = true;
            base.Text = value;
            suppressTextChangedEvent = false;
            this.TextChanged?.Invoke(this, new FreakyAutoCompleteViewTextChangedEventArgs(value ?? string.Empty, TextChangeReason.ProgrammaticChange));
        }
    }

    public virtual void SetTextColor(Color color)
    {
        this.SetTextColor(color.ToPlatform());
    }

    public virtual string Placeholder
    {
        set => HintFormatted = new Java.Lang.String(value as string ?? "");
    }

    public virtual void SetPlaceholderColor(Color color)
    {
        this.SetHintTextColor(color.ToPlatform());
    }

    public virtual bool IsSuggestionListOpen
    {
        set
        {
            if (value)
                ShowDropDown();
            else
                DismissDropDown();
        }
    }

    public virtual bool UpdateTextOnSelect { get; set; } = true;

    protected override void OnTextChanged(ICharSequence? text, int start, int lengthBefore, int lengthAfter)
    {
        if (!suppressTextChangedEvent)
            this.TextChanged?.Invoke(this, new FreakyAutoCompleteViewTextChangedEventArgs(text?.ToString() ?? string.Empty, TextChangeReason.UserInput));
        base.OnTextChanged(text, start, lengthBefore, lengthAfter);
    }

    private void DismissKeyboard()
    {
        if (Context?.GetSystemService(Context.InputMethodService) is InputMethodManager imm)
            imm.HideSoftInputFromWindow(WindowToken, 0);
    }

    private void OnItemClick(object? sender, AdapterView.ItemClickEventArgs e)
    {
        DismissKeyboard();
        var obj = adapter.GetObject(e.Position);
        if (UpdateTextOnSelect)
        {
            suppressTextChangedEvent = true;
            string text = textFunc?.Invoke(obj) ?? obj?.ToString() ?? string.Empty;
            base.Text = text;
            suppressTextChangedEvent = false;
            TextChanged?.Invoke(this, new FreakyAutoCompleteViewTextChangedEventArgs(text, TextChangeReason.SuggestionChosen));
        }
        if (obj is not null)
            SuggestionChosen?.Invoke(this, new FreakyAutoCompleteViewSuggestionChosenEventArgs(obj));
        QuerySubmitted?.Invoke(this, new FreakyAutoCompleteViewQuerySubmittedEventArgs(Text ?? string.Empty, obj));
    }

    public override void OnEditorAction([GeneratedEnum] ImeAction actionCode)
    {
        if (actionCode == ImeAction.Done || actionCode == ImeAction.Next)
        {
            DismissDropDown();
            DismissKeyboard();
            QuerySubmitted?.Invoke(this, new FreakyAutoCompleteViewQuerySubmittedEventArgs(Text ?? string.Empty, null));
        }
        else
            base.OnEditorAction(actionCode);
    }

    protected override void ReplaceText(ICharSequence? text)
    {
        //Override to avoid updating textbox on itemclick. We'll do this later using TextMemberPath and raise the proper TextChanged event then
    }

    public new event EventHandler<FreakyAutoCompleteViewTextChangedEventArgs>? TextChanged;

    public event EventHandler<FreakyAutoCompleteViewQuerySubmittedEventArgs>? QuerySubmitted;

    public event EventHandler<FreakyAutoCompleteViewSuggestionChosenEventArgs>? SuggestionChosen;

    public override void SetCompoundDrawablesWithIntrinsicBounds(Drawable? left, Drawable? top,
         Drawable? right, Drawable? bottom)
    {
        drawableLeft = left;
        drawableRight = right;
        drawableTop = top;
        drawableBottom = bottom;
        base.SetCompoundDrawablesWithIntrinsicBounds(left, top, right, bottom);
    }

    public override bool OnTouchEvent(MotionEvent? e)
    {
        if (e is null) return base.OnTouchEvent(e);

        if (e.Action != MotionEventActions.Down)
            return base.OnTouchEvent(e);

        actionX = (int)e.GetX();
        actionY = (int)e.GetY();

        if (TryHandleSimpleDrawableClick())
            return base.OnTouchEvent(e);

        if (TryHandleLeftDrawableClick())
            return false;

        if (TryHandleRightDrawableClick())
            return false;

        return base.OnTouchEvent(e);
    }

    private bool TryHandleSimpleDrawableClick()
    {
        if (drawableBottom is not null && drawableBottom.Bounds.Contains(actionX, actionY))
        {
            clickListener?.OnClick(DrawablePosition.Bottom);
            return true;
        }

        if (drawableTop is not null && drawableTop.Bounds.Contains(actionX, actionY))
        {
            clickListener?.OnClick(DrawablePosition.Top);
            return true;
        }

        return false;
    }

    private bool TryHandleLeftDrawableClick()
    {
        if (drawableLeft is null)
            return false;

        var bounds = drawableLeft.Bounds;
        int extraTapArea = (int)((13 * (Resources?.DisplayMetrics?.Density ?? 1f)) + 0.5);
        var (x, y) = AdjustCoordinatesForLeftDrawable(actionX, actionY, bounds, extraTapArea);

        if (bounds.Contains(x, y) && clickListener is not null)
        {
            clickListener.OnClick(DrawablePosition.Left);
            return true;
        }

        return false;
    }

    private bool TryHandleRightDrawableClick()
    {
        if (drawableRight is null)
            return false;

        var bounds = drawableRight.Bounds;
        const int extraTapArea = 13;
        var (x, y) = AdjustCoordinatesForRightDrawable(actionX, actionY, bounds, extraTapArea);

        if (bounds.Contains(x, y) && clickListener is not null)
        {
            clickListener.OnClick(DrawablePosition.Right);
            return true;
        }

        return false;
    }

    private (int, int) AdjustCoordinatesForLeftDrawable(int actionX, int actionY, Rect bounds, int extraTapArea)
    {
        int x = actionX;
        int y = actionY;

        if (!bounds.Contains(actionX, actionY))
        {
            x = (int)(actionX - extraTapArea);
            y = (int)(actionY - extraTapArea);

            if (x <= 0) x = actionX;
            if (y <= 0) y = actionY;

            if (x < y)
                y = x;
        }

        return (x, y);
    }

    private (int, int) AdjustCoordinatesForRightDrawable(int actionX, int actionY, Rect bounds, int extraTapArea)
    {
        int x = (int)(actionX + extraTapArea);
        int y = (int)(actionY - extraTapArea);

        x = Width - x;

        if (x <= 0)
            x += extraTapArea;

        if (y <= 0)
            y = actionY;

        return (x, y);
    }

    protected override void JavaFinalize()
    {
        drawableRight = null;
        drawableBottom = null;
        drawableLeft = null;
        drawableTop = null;
        base.JavaFinalize();
    }

    public void SetDrawableClickListener(IDrawableClickListener listener)
    {
        this.clickListener = listener;
    }

    public virtual void SetBorderStyle(Color borderColor, double borderWidth, double cornerRadius)
    {
        // Create a GradientDrawable for border and corner radius
        var drawable = new GradientDrawable();
        drawable.SetColor(Colors.White.ToPlatform()); // Background color

        // Convert DIP units to pixels for Android using density
        var density = Context?.Resources?.DisplayMetrics?.Density ?? 1.0f;
        int borderWidthPx = (int)System.Math.Round(borderWidth * density);
        drawable.SetStroke(borderWidthPx, borderColor.ToPlatform());

        if (cornerRadius > 0)
        {
            float cornerRadiusPx = (float)(cornerRadius * density);
            drawable.SetCornerRadius(cornerRadiusPx);
        }

        // Apply to the dropdown popup, not the input field itself
        SetDropDownBackgroundDrawable(drawable);
    }

    private class SuggestCompleteAdapter : ArrayAdapter, IFilterable
    {
        private SuggestFilter filter = new SuggestFilter();
        private List<object> resultList;
        private Func<object, string>? labelFunc;

        public SuggestCompleteAdapter(Context context, int textViewResourceId) : base(context, textViewResourceId)
        {
            resultList = new List<object>();
            SetNotifyOnChange(true);
        }

        public void UpdateList(IEnumerable<object> list, Func<object, string> labelFunc)
        {
            this.labelFunc = labelFunc;
            resultList = list.ToList();
            filter.SetFilter(resultList.Select(s => labelFunc(s)));
            NotifyDataSetChanged();
        }

        public override int Count
        {
            get
            {
                return resultList.Count;
            }
        }

        public override Filter Filter => filter;

        public override Java.Lang.Object GetItem(int position)
        {
            var obj = GetObject(position);
            string label = labelFunc?.Invoke(obj) ?? obj?.ToString() ?? string.Empty;
            return new Java.Lang.String(label);
        }

        public object GetObject(int position)
        {
            return resultList[position];
        }

        public override long GetItemId(int position)
        {
            return base.GetItemId(position);
        }

        private class SuggestFilter : Filter
        {
            private IEnumerable<string>? resultList;

            public SuggestFilter()
            {
            }

            public void SetFilter(IEnumerable<string> list)
            {
                resultList = list;
            }

            protected override FilterResults PerformFiltering(ICharSequence? constraint)
            {
                if (resultList is null)
                    return new FilterResults() { Count = 0, Values = null };
                var arr = resultList.ToArray();
                return new FilterResults() { Count = arr.Length, Values = arr };
            }

            protected override void PublishResults(ICharSequence? constraint, FilterResults? results)
            {
            }
        }
    }
}