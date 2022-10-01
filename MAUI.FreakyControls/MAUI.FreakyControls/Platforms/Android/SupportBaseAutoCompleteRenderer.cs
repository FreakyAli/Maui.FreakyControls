using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Maui.FreakyControls.Shared.Controls;
using System.ComponentModel;
using Android.Widget;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using TextAlignment = Android.Views.TextAlignment;
using Android.Graphics;
using Android.Views.InputMethods;
using Android.Views;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Java.Lang;
using System.Linq;
using Exception = System.Exception;
using View = Android.Views.View;
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;
using Object = Java.Lang.Object;
using Button = Android.Widget.Button;
using CheckBox = Android.Widget.CheckBox;

namespace Maui.FreakyControls.Platforms.Android;

public class SupportAutoCompleteRenderer : SupportBaseAutoCompleteRenderer<SupportAutoComplete, AutoCompleteTextView>
{
    public SupportAutoCompleteRenderer(Context context) : base(context)
    {
    }

    protected override void OnInitializeOriginalView()
    {
        OriginalView = new AutoCompleteTextView(Context);
        base.OnInitializeOriginalView();
    }
}

public class JavaHolder : Java.Lang.Object
{
    public readonly object Instance;

    public JavaHolder(object instance)
    {
        Instance = instance;
    }
}

public static class ObjectExtensions
{
    public static TObject ToNetObject<TObject>(this Java.Lang.Object value)
    {
        if (value == null)
            return default(TObject);

        if (!(value is JavaHolder))
            throw new InvalidOperationException("Unable to convert to .NET object. Only Java.Lang.Object created with .ToJavaObject() can be converted.");

        TObject returnVal;
        try { returnVal = (TObject)((JavaHolder)value).Instance; }
        finally { value.Dispose(); }
        return returnVal;
    }

    public static Java.Lang.Object ToJavaObject<TObject>(this TObject value)
    {
        if (Equals(value, default(TObject)) && !typeof(TObject).IsValueType)
            return null;

        var holder = new JavaHolder(value);

        return holder;
    }
}

public static class ViewExtensions
{
    public static void InitlizeReturnKey(this EditText editText, SupportEntryReturnType returnType)
    {
        switch (returnType)
        {
            case SupportEntryReturnType.Go:
                editText.ImeOptions = ImeAction.Go;
                editText.SetImeActionLabel("Go", ImeAction.Go);
                break;
            case SupportEntryReturnType.Next:
                editText.ImeOptions = ImeAction.Next;
                editText.SetImeActionLabel("Next", ImeAction.Next);
                break;
            case SupportEntryReturnType.Send:
                editText.ImeOptions = ImeAction.Send;
                editText.SetImeActionLabel("Send", ImeAction.Send);
                break;
            case SupportEntryReturnType.Search:
                editText.ImeOptions = ImeAction.Search;
                editText.SetImeActionLabel("Search", ImeAction.Search);
                break;
            default:
                editText.ImeOptions = ImeAction.Done;
                editText.SetImeActionLabel("Done", ImeAction.Done);
                break;
        }
    }
}

public class SpecAndroid
{
    static object Lock = new object();
    public static System.Collections.Hashtable CACHE = new System.Collections.Hashtable();

    public static Typeface CreateTypeface(Context context, string fontName)
    {
        lock (Lock)
        {
            try
            {
                if (!CACHE.ContainsKey(fontName))
                {
                    Typeface typeface;
                    typeface = Typeface.CreateFromAsset(context.Assets, fontName);
                    CACHE.Add(fontName, typeface);
                }
                return CACHE[fontName] as Typeface;
            }
            catch (Exception ex)
            {
                return Typeface.Default;
            }
        }
    }
}

public class SupportDropRenderer<TSupport, TOrignal> : Microsoft.Maui.Controls.Handlers.Compatibility.ViewRenderer<TSupport, View>, IDropItemSelected where TSupport : SupportViewDrop where TOrignal : View
{
    protected TSupport SupportView;
    protected TOrignal OriginalView;
    protected GradientDrawable gradientDrawable;
    protected ArrayAdapter dropItemAdapter;
    protected List<IAutoDropItem> SupportItemList = new List<IAutoDropItem>();

    public SupportDropRenderer(Context context) : base(context)
    {

    }

    protected virtual void OnInitializeAdapter()
    {

    }

    protected virtual void SyncItemSource()
    {
        SupportItemList.Clear();
        if (SupportView.ItemsSource != null)
        {
            SupportItemList.AddRange(SupportView.ItemsSource.ToList());
        }
    }

    protected virtual void NotifyAdapterChanged()
    {
        if (dropItemAdapter != null)
            dropItemAdapter.NotifyDataSetChanged();
    }

    protected virtual void OnInitializeOriginalView()
    {

    }

    protected virtual void OnInitializeBorderView()
    {
        gradientDrawable = new GradientDrawable();
        gradientDrawable.SetStroke((int)SupportView.CornerWidth, SupportView.CornerColor.ToPlatform());
        gradientDrawable.SetShape(ShapeType.Rectangle);
        gradientDrawable.SetCornerRadius((float)SupportView.CornerRadius);
    }

    protected override void OnElementChanged(ElementChangedEventArgs<TSupport> e)
    {
        base.OnElementChanged(e);
        if (e.NewElement != null && e.NewElement is TSupport)
        {
            SupportView = e.NewElement as TSupport;
            OnInitializeBorderView();
            OnInitializeOriginalView();
            SyncItemSource();
            OnInitializeAdapter();
            NotifyAdapterChanged();
            SetNativeControl(OriginalView);
        }
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);
        if (e.PropertyName.Equals(nameof(SupportViewDrop.ItemsSource)))
        {
            SyncItemSource();
            NotifyAdapterChanged();
        }
        else if (e.PropertyName.Equals(nameof(SupportViewDrop.RefreshList)))
        {
            NotifyAdapterChanged();
        }
        else if (e.PropertyName.Equals(SupportViewDrop.RefreshListProperty.PropertyName))
        {
            NotifyAdapterChanged();
        }
    }

    public virtual void IF_ItemSelectd(int position)
    {

    }
}

public class SupportBaseAutoCompleteRenderer<TSupportAutoComplete, TOriginal> : SupportDropRenderer<TSupportAutoComplete, TOriginal>
   where TSupportAutoComplete : SupportAutoComplete
   where TOriginal : AutoCompleteTextView
{
    public SupportBaseAutoCompleteRenderer(Context context) : base(context)
    {
    }

    protected override void OnInitializeOriginalView()
    {
        base.OnInitializeOriginalView();

        OriginalView.SetSingleLine(true);
        if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
        {
            OriginalView.SetBackgroundDrawable(gradientDrawable);
        }
        else
        {
            OriginalView.SetBackground(gradientDrawable);
        }
        OriginalView.SetPadding((int)SupportView.PaddingInside, 0, (int)SupportView.PaddingInside, 0);
        OriginalView.TextSize = (float)SupportView.FontSize;
        OriginalView.SetTextColor(SupportView.TextColor.ToPlatform());
        OriginalView.TextAlignment = TextAlignment.TextStart;
        //OriginalView.Typeface = SpecAndroid.CreateTypeface(Context, SupportView.FontFamily.Split('#')[0]);
        OriginalView.Hint = SupportView.Placeholder;

        OriginalView.Focusable = true;
        OriginalView.FocusableInTouchMode = true;
        OriginalView.RequestFocusFromTouch();

        OriginalView.FocusChange += OriginalView_FocusChange;
        OriginalView.TextChanged += OriginalView_TextChanged;
        OriginalView.InitlizeReturnKey(SupportView.ReturnType);
        OriginalView.EditorAction += (sender, ev) =>
        {
            SupportView.SendOnReturnKeyClicked();
        };
    }

    void OriginalView_TextChanged(object sender, TextChangedEventArgs e)
    {
        SupportView.SendOnTextChanged(OriginalView.Text);
    }

    void OriginalView_FocusChange(object sender, FocusChangeEventArgs e)
    {
        SupportView.SendOnTextFocused(e.HasFocus);
    }

    protected override void OnInitializeAdapter()
    {
        base.OnInitializeAdapter();
        dropItemAdapter = new DropItemAdapter(Context, SupportItemList, SupportView, this);
        OriginalView.Adapter = dropItemAdapter;
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);
        if (e.PropertyName.Equals(nameof(SupportAutoComplete.CurrentCornerColor)))
        {
            gradientDrawable.SetStroke((int)SupportView.CornerWidth, SupportView.CurrentCornerColor.ToPlatform());
            if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
            {
                OriginalView.SetBackgroundDrawable(gradientDrawable);
            }
            else
            {
                OriginalView.SetBackground(gradientDrawable);
            }
        }
        else if (e.PropertyName.Equals(nameof(SupportViewBase.Text)))
        {
            if (OriginalView != null && !OriginalView.Text.Equals(SupportView.Text))
            {
                OriginalView.SetText(SupportView.Text, false);
            }
        }
    }

    public override void IF_ItemSelectd(int position)
    {
        base.IF_ItemSelectd(position);

        var text = ((DropItemAdapter)dropItemAdapter).items[position].IF_GetTitle();
        OriginalView.SetText(text, false);
        SupportView.Text = text;

        Task.Delay(10).ContinueWith(delegate
        {
            MainThread.BeginInvokeOnMainThread(delegate
              {
                  OriginalView.SetSelection(text.Length);
                  OriginalView.DismissDropDown();
              });
        });
    }

    public class ChemicalFilter : Filter
    {
        private DropItemAdapter dropItemAdapter;

        public ChemicalFilter(DropItemAdapter dropItemAdapter)
        {
            this.dropItemAdapter = dropItemAdapter;
        }

        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            var returnObj = new FilterResults();
            var results = new List<IAutoDropItem>();

            if (dropItemAdapter.originalData == null)
                dropItemAdapter.originalData = dropItemAdapter.items;

            if (constraint == null)
                return returnObj;

            if (dropItemAdapter.originalData != null && dropItemAdapter.originalData.Any())
            {
                var key = constraint.ToString().ToLower();
                results.AddRange(dropItemAdapter.originalData.Where(drop => drop.IF_GetTitle().ToLower().Contains(key)));
            }

            returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
            returnObj.Count = results.Count;
            constraint.Dispose();
            return returnObj;
        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            if (results.Values != null)
            {
                using (var values = results.Values)
                    dropItemAdapter.items = values.ToArray<Object>().Select(r => r.ToNetObject<IAutoDropItem>()).ToList();

                dropItemAdapter.NotifyDataSetChanged();
                constraint.Dispose();
                results.Dispose();
            }
        }
    }

    public class DropItemAdapter : ArrayAdapter, IFilterable
    {
        public List<IAutoDropItem> originalData, items;
        private Context mContext;
        private SupportAutoComplete ConfigStyle;
        public Filter Filter { get; private set; }
        private IDropItemSelected IDropItemSelected;

        public DropItemAdapter(Context context, List<IAutoDropItem> storeDataLst, SupportAutoComplete _ConfigStyle, IDropItemSelected dropItemSelected) : base(context, 0)
        {
            originalData = storeDataLst;
            mContext = context;
            ConfigStyle = _ConfigStyle;
            Filter = new ChemicalFilter(this);
            items = new List<IAutoDropItem>();
            IDropItemSelected = dropItemSelected;
        }

        public override int Count => items.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TextView txtTitle = null, txtDescription = null, txtSeperator = null;
            ImageView imgIcon = null;
            Button bttClick;
            CheckBox checkBox = null;

            IAutoDropItem item = items[position];

            if (ConfigStyle.DropMode == SupportAutoCompleteDropMode.TitleWithDescription)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.layout_title_and_description, parent, false);
                txtDescription = convertView.FindViewById<TextView>(Resource.Id.txtDescription);
            }
            else if (ConfigStyle.DropMode == SupportAutoCompleteDropMode.IconAndTitle)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.layout_title_and_icon, parent, false);
                imgIcon = convertView.FindViewById<ImageView>(Resource.Id.imgIcon);
            }
            else if (ConfigStyle.DropMode == SupportAutoCompleteDropMode.FullTextAndIcon)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.layout_full_text_and_icon, parent, false);
                txtDescription = convertView.FindViewById<TextView>(Resource.Id.txtDescription);
                imgIcon = convertView.FindViewById<ImageView>(Resource.Id.imgIcon);
            }
            else
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.layout_single_title, parent, false);
            }

            txtTitle = convertView.FindViewById<TextView>(Resource.Id.txtTitle);
            txtSeperator = convertView.FindViewById<TextView>(Resource.Id.txtSeperator);
            bttClick = convertView.FindViewById<Button>(Resource.Id.bttClick);


            txtTitle.Text = item.IF_GetTitle();
            if (txtDescription != null)
            {
                txtDescription.Text = item.IF_GetDescription();
                txtDescription.SetTextColor(ConfigStyle.DescriptionTextColor.ToAndroid());
            }
            txtSeperator.SetBackgroundColor(ConfigStyle.SeperatorColor.ToAndroid());

            bttClick.Click += (sender, e) =>
            {
                IDropItemSelected.IF_ItemSelectd(position);
            };

            try
            {
                if (imgIcon != null)
                {
                    if (item.IF_GetIcon() != null)
                    {
                        var image = Context.Resources.GetIdentifier(item.IF_GetIcon(), "drawable", Context.PackageName);
                        imgIcon.SetImageResource(image);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return convertView;
        }
    }
}

