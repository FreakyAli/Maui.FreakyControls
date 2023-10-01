using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Java.Lang;
using Maui.FreakyControls.Shared.Enums;
using Microsoft.Maui.Platform;
using Rect = Android.Graphics.Rect;

namespace Maui.FreakyControls.Platforms.Android.NativeControls;

public class FreakyNativeAutoCompleteView : AppCompatAutoCompleteTextView
{
    private bool suppressTextChangedEvent;
    private Func<object, string> textFunc;
    private SuggestCompleteAdapter adapter;
    private Drawable drawableRight;
    private Drawable drawableLeft;
    private Drawable drawableTop;
    private Drawable drawableBottom;
    private int actionX, actionY;
    private IDrawableClickListener clickListener;

    public FreakyNativeAutoCompleteView(Context context) : base(context)
    {
        SetMaxLines(1);
        InputType = global::Android.Text.InputTypes.TextFlagNoSuggestions | global::Android.Text.InputTypes.TextVariationVisiblePassword; //Disables text suggestions as the auto-complete view is there to do that
        ItemClick += OnItemClick;
        Adapter = adapter = new SuggestCompleteAdapter(Context, global::Android.Resource.Layout.SimpleDropDownItem1Line);
    }

    public override bool EnoughToFilter() => true;

    internal void SetItems(IEnumerable<object> items, Func<object, string> labelFunc, Func<object, string> textFunc)
    {
        this.textFunc = textFunc;
        if (items == null)
            adapter.UpdateList(Enumerable.Empty<string>(), labelFunc);
        else
            adapter.UpdateList(items.OfType<object>(), labelFunc);
    }

    public virtual new string Text
    {
        get => base.Text;
        set
        {
            suppressTextChangedEvent = true;
            base.Text = value;
            suppressTextChangedEvent = false;
            this.TextChanged?.Invoke(this, new FreakyAutoCompleteViewTextChangedEventArgs(value, TextChangeReason.ProgrammaticChange));
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

    protected override void OnTextChanged(ICharSequence text, int start, int lengthBefore, int lengthAfter)
    {
        if (!suppressTextChangedEvent)
            this.TextChanged?.Invoke(this, new FreakyAutoCompleteViewTextChangedEventArgs(text.ToString(), TextChangeReason.UserInput));
        base.OnTextChanged(text, start, lengthBefore, lengthAfter);
    }

    private void DismissKeyboard()
    {
        var imm = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
        imm.HideSoftInputFromWindow(WindowToken, 0);
    }

    private void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
    {
        DismissKeyboard();
        var obj = adapter.GetObject(e.Position);
        if (UpdateTextOnSelect)
        {
            suppressTextChangedEvent = true;
            string text = textFunc(obj);
            base.Text = text;
            suppressTextChangedEvent = false;
            TextChanged?.Invoke(this, new FreakyAutoCompleteViewTextChangedEventArgs(text, TextChangeReason.SuggestionChosen));
        }
        SuggestionChosen?.Invoke(this, new FreakyAutoCompleteViewSuggestionChosenEventArgs(obj));
        QuerySubmitted?.Invoke(this, new FreakyAutoCompleteViewQuerySubmittedEventArgs(Text, obj));
    }

    public override void OnEditorAction([GeneratedEnum] ImeAction actionCode)
    {
        if (actionCode == ImeAction.Done || actionCode == ImeAction.Next)
        {
            DismissDropDown();
            DismissKeyboard();
            QuerySubmitted?.Invoke(this, new FreakyAutoCompleteViewQuerySubmittedEventArgs(Text, null));
        }
        else
            base.OnEditorAction(actionCode);
    }

    protected override void ReplaceText(ICharSequence text)
    {
        //Override to avoid updating textbox on itemclick. We'll do this later using TextMemberPath and raise the proper TextChanged event then
    }

    public new event EventHandler<FreakyAutoCompleteViewTextChangedEventArgs> TextChanged;
    public event EventHandler<FreakyAutoCompleteViewQuerySubmittedEventArgs> QuerySubmitted;
    public event EventHandler<FreakyAutoCompleteViewSuggestionChosenEventArgs> SuggestionChosen;

    public override void SetCompoundDrawablesWithIntrinsicBounds(Drawable left, Drawable top,
         Drawable right, Drawable bottom)
    {
        if (left != null)
        {
            drawableLeft = left;
        }
        if (right != null)
        {
            drawableRight = right;
        }
        if (top != null)
        {
            drawableTop = top;
        }
        if (bottom != null)
        {
            drawableBottom = bottom;
        }
        base.SetCompoundDrawablesWithIntrinsicBounds(left, top, right, bottom);
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
        Rect bounds;
        if (e.Action == MotionEventActions.Down)
        {
            actionX = (int)e.GetX();
            actionY = (int)e.GetY();
            if (drawableBottom != null
                && drawableBottom.Bounds.Contains(actionX, actionY))
            {
                clickListener.OnClick(DrawablePosition.Bottom);
                return base.OnTouchEvent(e);
            }

            if (drawableTop != null
                    && drawableTop.Bounds.Contains(actionX, actionY))
            {
                clickListener.OnClick(DrawablePosition.Top);
                return base.OnTouchEvent(e);
            }

            // this works for left since container shares 0,0 origin with bounds
            if (drawableLeft != null)
            {
                bounds = null;
                bounds = drawableLeft.Bounds;

                int x, y;
                int extraTapArea = (int)((13 * Resources.DisplayMetrics.Density) + 0.5);

                x = actionX;
                y = actionY;

                if (!bounds.Contains(actionX, actionY))
                {
                    // Gives the +20 area for tapping. /
                    x = (int)(actionX - extraTapArea);
                    y = (int)(actionY - extraTapArea);

                    if (x <= 0)
                        x = actionX;
                    if (y <= 0)
                        y = actionY;

                    // Creates square from the smallest value /
                    if (x < y)
                    {
                        y = x;
                    }
                }

                if (bounds.Contains(x, y) && clickListener != null)
                {
                    clickListener.OnClick(DrawablePosition.Left);
                    e.Action = (MotionEventActions.Cancel);
                    return false;
                }
            }

            if (drawableRight != null)
            {
                bounds = null;
                bounds = drawableRight.Bounds;

                int x, y;
                int extraTapArea = 13;

                //
                //  IF USER CLICKS JUST OUT SIDE THE RECTANGLE OF THE DRAWABLE
                //  THAN ADD X AND SUBTRACT THE Y WITH SOME VALUE SO THAT AFTER
                //  CALCULATING X AND Y CO-ORDINATE LIES INTO THE DRAWBABLE
                //  BOUND. - this process help to increase the tappable area of
                //  the rectangle.
                //
                x = (int)(actionX + extraTapArea);
                y = (int)(actionY - extraTapArea);

                // Since this is right drawable subtract the value of x from the width
                // of view. so that width - tappedarea will result in x co-ordinate in drawable bound.
                //
                x = Width - x;

                //x can be negative if user taps at x co-ordinate just near the width.
                // e.g views width = 300 and user taps 290. Then as per previous calculation
                // 290 + 13 = 303. So subtract X from getWidth() will result in negative value.
                // So to avoid this add the value previous added when x goes negative.
                //

                if (x <= 0)
                {
                    x += extraTapArea;
                }

                // If result after calculating for extra tappable area is negative.
                // assign the original value so that after subtracting
                // extratapping area value doesn't go into negative value.
                //

                if (y <= 0)
                    y = actionY;

                //If drawble bounds contains the x and y points then move ahead./
                if (bounds.Contains(x, y) && clickListener != null)
                {
                    clickListener
                            .OnClick(DrawablePosition.Right);
                    e.Action = (MotionEventActions.Cancel);
                    return false;
                }
                return base.OnTouchEvent(e);
            }
        }
        return base.OnTouchEvent(e);
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

    private class SuggestCompleteAdapter : ArrayAdapter, IFilterable
    {
        private SuggestFilter filter = new SuggestFilter();
        private List<object> resultList;
        private Func<object, string> labelFunc;

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
            return labelFunc(GetObject(position));
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
            private IEnumerable<string> resultList;

            public SuggestFilter()
            {
            }

            public void SetFilter(IEnumerable<string> list)
            {
                resultList = list;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                if (resultList == null)
                    return new FilterResults() { Count = 0, Values = null };
                var arr = resultList.ToArray();
                return new FilterResults() { Count = arr.Length, Values = arr };
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
            }
        }
    }
}