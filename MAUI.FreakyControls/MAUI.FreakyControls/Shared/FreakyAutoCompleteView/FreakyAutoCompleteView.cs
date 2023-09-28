using System.Windows.Input;
using Maui.FreakyControls.Shared.Enums;
using Color = Microsoft.Maui.Graphics.Color;

namespace Maui.FreakyControls;

public class FreakyAutoCompleteView : View, IFreakyAutoCompleteView
{
    private bool suppressTextChangedEvent;

    private readonly WeakEventManager querySubmittedEventManager = new();
    public readonly WeakEventManager textChangedEventManager = new();
    private readonly WeakEventManager suggestionChosenEventManager = new();

    /// <summary>
    /// Gets or sets the Text property
    /// </summary>
    /// <seealso cref="TextColor"/>
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
        if (!box.suppressTextChangedEvent) //Ensure this property changed didn't get call because we were updating it from the native text property
            box.textChangedEventManager.HandleEvent(box, new FreakyAutoCompleteViewTextChangedEventArgs("", TextChangeReason.ProgrammaticChange), nameof(TextChanged));
    }

    /// <summary>
    /// Gets or sets the Threshold for showing the list 
    /// </summary>
    public int Threshold
    {
        get { return (int)GetValue(ThresholdProperty); }
        set { SetValue(ThresholdProperty, value); }
    }

    public static readonly BindableProperty ThresholdProperty =
        BindableProperty.Create(nameof(Threshold), typeof(int), typeof(FreakyAutoCompleteView), 1, BindingMode.OneWay, null, OnTextPropertyChanged);


    /// <summary>
    /// Gets or sets the foreground color of the control
    /// </summary>
    /// <seealso cref="Text"/>
    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FreakyAutoCompleteView), Colors.Gray, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the Placeholder
    /// </summary>
    /// <seealso cref="PlaceholderColor"/>
    public string Placeholder
    {
        get { return (string)GetValue(PlaceholderProperty); }
        set { SetValue(PlaceholderProperty, value); }
    }

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the foreground color of the control
    /// </summary>
    /// <seealso cref="Placeholder"/>
    public Color PlaceholderColor
    {
        get { return (Color)GetValue(PlaceholderColorProperty); }
        set { SetValue(PlaceholderColorProperty, value); }
    }

    public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(FreakyAutoCompleteView), Colors.Gray, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the property path that is used to get the value for display in the
    /// text box portion of the FreakyAutoCompleteView control, when an item is selected.
    /// </summary>
    /// <value>
    /// The property path that is used to get the value for display in the text box portion
    /// of the FreakyAutoCompleteView control, when an item is selected.
    /// </value>
    public string TextMemberPath
    {
        get { return (string)GetValue(TextMemberPathProperty); }
        set { SetValue(TextMemberPathProperty, value); }
    }

    public static readonly BindableProperty TextMemberPathProperty =
        BindableProperty.Create(nameof(TextMemberPath), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the name or path of the property that is displayed for each data item.
    /// </summary>
    /// <value>
    /// The name or path of the property that is displayed for each the data item in
    /// the control. The default is an empty string ("").
    /// </value>
    public string DisplayMemberPath
    {
        get { return (string)GetValue(DisplayMemberPathProperty); }
        set { SetValue(DisplayMemberPathProperty, value); }
    }

    public static readonly BindableProperty DisplayMemberPathProperty =
        BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets a Boolean value indicating whether the drop-down portion of the FreakyAutoCompleteView is open.
    /// </summary>
    /// <value>A Boolean value indicating whether the drop-down portion of the FreakyAutoCompleteView is open.</value>
    public bool IsSuggestionListOpen
    {
        get { return (bool)GetValue(IsSuggestionListOpenProperty); }
        set { SetValue(IsSuggestionListOpenProperty, value); }
    }


    public static readonly BindableProperty IsSuggestionListOpenProperty =
        BindableProperty.Create(nameof(IsSuggestionListOpen), typeof(bool), typeof(FreakyAutoCompleteView), false, BindingMode.OneWay, null, null);

    /// <summary>
    /// Used in conjunction with <see cref="TextMemberPath"/>, gets or sets a value indicating whether items in the view will trigger an update
    /// of the editable text part of the <see cref="FreakyAutoCompleteView"/> when clicked.
    /// </summary>
    /// <value>A value indicating whether items in the view will trigger an update of the editable text part of the <see cref="FreakyAutoCompleteView"/> when clicked.</value>
    public bool UpdateTextOnSelect
    {
        get { return (bool)GetValue(UpdateTextOnSelectProperty); }
        set { SetValue(UpdateTextOnSelectProperty, value); }
    }

    public static readonly BindableProperty UpdateTextOnSelectProperty =
        BindableProperty.Create(nameof(UpdateTextOnSelect), typeof(bool), typeof(FreakyAutoCompleteView), true, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the header object for the text box portion of this control.
    /// </summary>
    /// <value>The header object for the text box portion of this control.</value>
    public System.Collections.IList ItemsSource
    {
        get { return GetValue(ItemsSourceProperty) as System.Collections.IList; }
        set { SetValue(ItemsSourceProperty, value); }
    }

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(FreakyAutoCompleteView), null, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets if copy pasting is allowed
    /// </summary>
    public bool AllowCopyPaste
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public static readonly BindableProperty AllowCopyPasteProperty =
        BindableProperty.Create(nameof(AllowCopyPaste), typeof(bool), typeof(FreakyAutoCompleteView), false, BindingMode.OneWay, null, null);

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
              nameof(Image),
              typeof(ImageSource),
              typeof(FreakyEntry),
              default(ImageSource));

    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(
           nameof(ImageHeight),
           typeof(int),
           typeof(FreakyEntry),
           25);

    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(
           nameof(ImageWidth),
           typeof(int),
           typeof(FreakyEntry),
           25);

    public static readonly BindableProperty ImageAlignmentProperty = BindableProperty.Create(
           nameof(ImageAlignment),
           typeof(ImageAlignment),
           typeof(FreakyEntry),
           ImageAlignment.Right);

    public static readonly BindableProperty ImagePaddingProperty = BindableProperty.Create(
           nameof(ImagePadding),
           typeof(int),
           typeof(FreakyEntry),
           5);

    public static readonly BindableProperty ImageCommandProperty = BindableProperty.Create(
          nameof(ImagePadding),
          typeof(ICommand),
          typeof(FreakyEntry),
          default(ICommand));

    public static readonly BindableProperty ImageCommandParameterProperty = BindableProperty.Create(
          nameof(ImageCommandParameter),
          typeof(object),
          typeof(FreakyEntry),
          default(object));

    /// <summary>
    /// Command parameter for your Image tap command
    /// </summary>
    public object ImageCommandParameter
    {
        get => GetValue(ImageCommandParameterProperty);
        set => SetValue(ImageCommandParameterProperty, value);
    }

    /// <summary>
    /// A command that you can use to bind with your Image that you added to your Entry's ViewPort
    /// </summary>
    public ICommand ImageCommand
    {
        get => (ICommand)GetValue(ImageCommandProperty);
        set => SetValue(ImageCommandProperty, value);
    }

    /// <summary>
    /// Padding of the Image that you added to the ViewPort
    /// </summary>
    public int ImagePadding
    {
        get => (int)GetValue(ImagePaddingProperty);
        set => SetValue(ImagePaddingProperty, value);
    }

    /// <summary>
    /// Width of the Image in your ViewPort
    /// </summary>
    public int ImageWidth
    {
        get => (int)GetValue(ImageWidthProperty);
        set => SetValue(ImageWidthProperty, value);
    }

    /// <summary>
    /// Height of the Image in your ViewPort
    /// </summary>
    public int ImageHeight
    {
        get => (int)GetValue(ImageHeightProperty);
        set => SetValue(ImageHeightProperty, value);
    }

    /// <summary>
    /// An ImageSource that you want to add to your ViewPort
    /// </summary>
    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    /// <summary>
    /// Alignment for your Image's ViewPort, By default set to Right.
    /// </summary>
    public ImageAlignment ImageAlignment
    {
        get => (ImageAlignment)GetValue(ImageAlignmentProperty);
        set => SetValue(ImageAlignmentProperty, value);
    }

    public bool IsPassword { get; set; }

    public ReturnType ReturnType { get; set; }

    public ClearButtonVisibility ClearButtonVisibility { get; set; }

    public bool IsTextPredictionEnabled { get; set; }

    public bool IsReadOnly { get; set; }

    public Keyboard Keyboard { get; set; }

    public int MaxLength { get; set; }

    public int CursorPosition { get; set; }
    public int SelectionLength { get; set; }

    public Microsoft.Maui.Font Font { get; set; }

    public double CharacterSpacing { get; set; }

    public TextAlignment HorizontalTextAlignment { get; set; }

    public TextAlignment VerticalTextAlignment { get; set; }

    public event EventHandler<FreakyAutoCompleteViewSuggestionChosenEventArgs> SuggestionChosen
    {
        add => suggestionChosenEventManager.AddEventHandler(value);
        remove => suggestionChosenEventManager.RemoveEventHandler(value);
    }

    public void RaiseSuggestionChosen(FreakyAutoCompleteViewSuggestionChosenEventArgs args)
    {
        suggestionChosenEventManager.HandleEvent(this, args, nameof(SuggestionChosen));
    }

    /// <summary>
    /// Raised before the text content of the editable control component is updated.
    /// </summary>
    public event EventHandler<FreakyAutoCompleteViewTextChangedEventArgs> TextChanged
    {
        add => textChangedEventManager.AddEventHandler(value);
        remove => textChangedEventManager.RemoveEventHandler(value);
    }

    // Called by the native control when users enter text
    public void NativeControlTextChanged(FreakyAutoCompleteViewTextChangedEventArgs args)
    {
        suppressTextChangedEvent = true; //prevent loop of events raising, as setting this property will make it back into the native control
        Text = args.Text;
        suppressTextChangedEvent = false;
        textChangedEventManager.HandleEvent(this, args, nameof(TextChanged));
    }

    /// <summary>
    /// Raised after the text content of the editable control component is updated.
    /// </summary>
    public void RaiseQuerySubmitted(FreakyAutoCompleteViewQuerySubmittedEventArgs args)
    {
        querySubmittedEventManager.HandleEvent(this, args, nameof(QuerySubmitted));
    }

    public void Completed()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Executes QuerySubmitted event
    /// Occurs when the user submits a search query.
    /// </summary>
    public event EventHandler<FreakyAutoCompleteViewQuerySubmittedEventArgs> QuerySubmitted
    {
        add => querySubmittedEventManager.AddEventHandler(value);
        remove => querySubmittedEventManager.RemoveEventHandler(value);
    }
}