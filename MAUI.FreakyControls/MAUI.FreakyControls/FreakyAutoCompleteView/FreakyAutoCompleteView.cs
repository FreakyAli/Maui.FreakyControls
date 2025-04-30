using Maui.FreakyControls.Enums;
using System.ComponentModel;
using System.Windows.Input;

namespace Maui.FreakyControls;

public class FreakyAutoCompleteView : View, IFreakyAutoCompleteView
{
    private bool suppressTextChangedEvent;
    private readonly WeakEventManager querySubmittedEventManager = new();
    public readonly WeakEventManager textChangedEventManager = new();
    private readonly WeakEventManager suggestionChosenEventManager = new();

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(FreakyAutoCompleteView), "", BindingMode.OneWay, null, OnTextPropertyChanged);

    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var box = (FreakyAutoCompleteView)bindable;
        if (!box.suppressTextChangedEvent)
            box.textChangedEventManager.HandleEvent(box, new FreakyAutoCompleteViewTextChangedEventArgs("", TextChangeReason.ProgrammaticChange), nameof(TextChanged));
    }

    public int Threshold
    {
        get { return (int)GetValue(ThresholdProperty); }
        set { SetValue(ThresholdProperty, value); }
    }

    public static readonly BindableProperty ThresholdProperty =
        BindableProperty.Create(nameof(Threshold), typeof(int), typeof(FreakyAutoCompleteView), 1, BindingMode.OneWay, null, OnTextPropertyChanged);

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FreakyAutoCompleteView), Colors.Gray, BindingMode.OneWay, null, null);

    public string Placeholder
    {
        get { return (string)GetValue(PlaceholderProperty); }
        set { SetValue(PlaceholderProperty, value); }
    }

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    public Color PlaceholderColor
    {
        get { return (Color)GetValue(PlaceholderColorProperty); }
        set { SetValue(PlaceholderColorProperty, value); }
    }

    public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(FreakyAutoCompleteView), Colors.Gray, BindingMode.OneWay, null, null);

    public string TextMemberPath
    {
        get { return (string)GetValue(TextMemberPathProperty); }
        set { SetValue(TextMemberPathProperty, value); }
    }

    public static readonly BindableProperty TextMemberPathProperty =
        BindableProperty.Create(nameof(TextMemberPath), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    public string DisplayMemberPath
    {
        get { return (string)GetValue(DisplayMemberPathProperty); }
        set { SetValue(DisplayMemberPathProperty, value); }
    }

    public static readonly BindableProperty DisplayMemberPathProperty =
        BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    public bool IsSuggestionListOpen
    {
        get { return (bool)GetValue(IsSuggestionListOpenProperty); }
        set { SetValue(IsSuggestionListOpenProperty, value); }
    }

    public static readonly BindableProperty IsSuggestionListOpenProperty =
        BindableProperty.Create(nameof(IsSuggestionListOpen), typeof(bool), typeof(FreakyAutoCompleteView), false, BindingMode.OneWay, null, null);

    public bool UpdateTextOnSelect
    {
        get { return (bool)GetValue(UpdateTextOnSelectProperty); }
        set { SetValue(UpdateTextOnSelectProperty, value); }
    }

    public static readonly BindableProperty UpdateTextOnSelectProperty =
        BindableProperty.Create(nameof(UpdateTextOnSelect), typeof(bool), typeof(FreakyAutoCompleteView), true, BindingMode.OneWay, null, null);

    public System.Collections.IList ItemsSource
    {
        get { return GetValue(ItemsSourceProperty) as System.Collections.IList; }
        set { SetValue(ItemsSourceProperty, value); }
    }

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(FreakyAutoCompleteView), null, BindingMode.OneWay, null, null);

    public bool AllowCopyPaste
    {
        get => (bool)GetValue(AllowCopyPasteProperty);
        set => SetValue(AllowCopyPasteProperty, value);
    }

    public static readonly BindableProperty HorizontalTextAlignmentProperty =
    BindableProperty.Create(
        nameof(HorizontalTextAlignment),
        typeof(TextAlignment),
        typeof(FreakyAutoCompleteView),
        TextAlignment.Start);

    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    public static readonly BindableProperty VerticalTextAlignmentProperty =
        BindableProperty.Create(
            nameof(VerticalTextAlignment),
            typeof(TextAlignment),
            typeof(FreakyAutoCompleteView),
            TextAlignment.Center);

    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(
            nameof(FontFamily),
            typeof(string),
            typeof(FreakyAutoCompleteView),
            default(string));

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
    BindableProperty.Create(
        nameof(FontSize),
        typeof(double),
        typeof(FreakyAutoCompleteView),
        14.0); 

    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(
            nameof(FontAttributes),
            typeof(FontAttributes),
            typeof(FreakyAutoCompleteView),
            FontAttributes.None);

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public static readonly BindableProperty TextTransformProperty =
        BindableProperty.Create(
            nameof(TextTransform),
            typeof(TextTransform),
            typeof(FreakyAutoCompleteView),
            TextTransform.None);

    public TextTransform TextTransform
    {
        get => (TextTransform)GetValue(TextTransformProperty);
        set => SetValue(TextTransformProperty, value);
    }

    public static readonly BindableProperty AllowCopyPasteProperty = BindableProperty.Create(
        nameof(AllowCopyPaste),
        typeof(bool),
        typeof(FreakyAutoCompleteView),
        false,
        BindingMode.OneWay,
        null,
        null);

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            nameof(Image),
            typeof(ImageSource),
            typeof(FreakyAutoCompleteView),
            default(ImageSource));

    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(
           nameof(ImageHeight),
           typeof(int),
           typeof(FreakyAutoCompleteView),
           25);

    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(
           nameof(ImageWidth),
           typeof(int),
           typeof(FreakyAutoCompleteView),
           25);

    public static readonly BindableProperty ImageAlignmentProperty = BindableProperty.Create(
           nameof(ImageAlignment),
           typeof(ImageAlignment),
           typeof(FreakyAutoCompleteView),
           ImageAlignment.Right);

    public static readonly BindableProperty ImagePaddingProperty = BindableProperty.Create(
           nameof(ImagePadding),
           typeof(int),
           typeof(FreakyAutoCompleteView),
           5);

    public static readonly BindableProperty ImageCommandProperty = BindableProperty.Create(
          nameof(ImagePadding),
          typeof(ICommand),
          typeof(FreakyAutoCompleteView),
          default(ICommand));

    public static readonly BindableProperty ImageCommandParameterProperty = BindableProperty.Create(
          nameof(ImageCommandParameter),
          typeof(object),
          typeof(FreakyAutoCompleteView),
          default);

    public object ImageCommandParameter
    {
        get => GetValue(ImageCommandParameterProperty);
        set => SetValue(ImageCommandParameterProperty, value);
    }

    public ICommand ImageCommand
    {
        get => (ICommand)GetValue(ImageCommandProperty);
        set => SetValue(ImageCommandProperty, value);
    }

    public int ImagePadding
    {
        get => (int)GetValue(ImagePaddingProperty);
        set => SetValue(ImagePaddingProperty, value);
    }

    public int ImageWidth
    {
        get => (int)GetValue(ImageWidthProperty);
        set => SetValue(ImageWidthProperty, value);
    }

    public int ImageHeight
    {
        get => (int)GetValue(ImageHeightProperty);
        set => SetValue(ImageHeightProperty, value);
    }

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public ImageAlignment ImageAlignment
    {
        get => (ImageAlignment)GetValue(ImageAlignmentProperty);
        set => SetValue(ImageAlignmentProperty, value);
    }
   
    public event EventHandler<FreakyAutoCompleteViewSuggestionChosenEventArgs> SuggestionChosen
    {
        add => suggestionChosenEventManager.AddEventHandler(value);
        remove => suggestionChosenEventManager.RemoveEventHandler(value);
    }

    public void RaiseSuggestionChosen(FreakyAutoCompleteViewSuggestionChosenEventArgs args)
    {
        suggestionChosenEventManager.HandleEvent(this, args, nameof(SuggestionChosen));
    }

    public event EventHandler<FreakyAutoCompleteViewTextChangedEventArgs> TextChanged
    {
        add => textChangedEventManager.AddEventHandler(value);
        remove => textChangedEventManager.RemoveEventHandler(value);
    }

    public void NativeControlTextChanged(FreakyAutoCompleteViewTextChangedEventArgs args)
    {
        suppressTextChangedEvent = true;
        Text = args.Text;
        suppressTextChangedEvent = false;
        textChangedEventManager.HandleEvent(this, args, nameof(TextChanged));
    }

    public void RaiseQuerySubmitted(FreakyAutoCompleteViewQuerySubmittedEventArgs args)
    {
        querySubmittedEventManager.HandleEvent(this, args, nameof(QuerySubmitted));
    }

    public event EventHandler<FreakyAutoCompleteViewQuerySubmittedEventArgs> QuerySubmitted
    {
        add => querySubmittedEventManager.AddEventHandler(value);
        remove => querySubmittedEventManager.RemoveEventHandler(value);
    }
}