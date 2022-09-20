using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public partial class FreakyAutoCompleteView : View, IFreakyAutoCompleteView
{
    private bool suppressTextChangedEvent;

    /// <summary>
    /// Initializes a new instance of the <see cref="FreakyAutoCompleteView"/> class
    /// </summary>
    public FreakyAutoCompleteView()
    {
        MessagingCenter.Subscribe(this, "FreakyAutoCompleteView_" + nameof(SuggestionChosen), (FreakyAutoCompleteView box, object selectedItem) => { if (box == this) RaiseSuggestionChosen(selectedItem); });
        MessagingCenter.Subscribe(this, "FreakyAutoCompleteView_" + nameof(TextChanged), (FreakyAutoCompleteView box, (string queryText, TextChangeReason reason) args) => { if (box == this) NativeControlTextChanged(args.queryText, args.reason); });
        MessagingCenter.Subscribe(this, "FreakyAutoCompleteView_" + nameof(QuerySubmitted), (FreakyAutoCompleteView box, (string queryText, object chosenSuggestion) args) => { if (box == this) RaiseQuerySubmitted(args.queryText, args.chosenSuggestion); });
    }

    /// <summary>
    /// Gets or sets the Text property
    /// </summary>
    /// <seealso cref="TextColor"/>
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="Text"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(FreakyAutoCompleteView), "", BindingMode.OneWay, null, OnTextPropertyChanged);

    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var box = (FreakyAutoCompleteView)bindable;
        if (!box.suppressTextChangedEvent) //Ensure this property changed didn't get call because we were updating it from the native text property
            box.TextChanged?.Invoke(box, new FreakyAutoCompleteViewTextChangedEventArgs(TextChangeReason.ProgrammaticChange));
    }

    /// <summary>
    /// Gets or sets the foreground color of the control
    /// </summary>
    /// <seealso cref="Text"/>
    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="TextColor"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FreakyAutoCompleteView), Colors.Gray, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the PlaceHolder
    /// </summary>
    /// <seealso cref="PlaceHolderColor"/>
    public string PlaceHolder
    {
        get { return (string)GetValue(PlaceHolderProperty); }
        set { SetValue(PlaceHolderProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="PlaceHolder"/> bindable property.
    /// </summary>
    public static readonly BindableProperty PlaceHolderProperty =
        BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the foreground color of the control
    /// </summary>
    /// <seealso cref="PlaceHolder"/>
    public Color PlaceHolderColor
    {
        get { return (Color)GetValue(PlaceHolderColorProperty); }
        set { SetValue(PlaceHolderColorProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="PlaceHolderColor"/> bindable property.
    /// </summary>
    public static readonly BindableProperty PlaceHolderColorProperty =
        BindableProperty.Create(nameof(PlaceHolderColor), typeof(Color), typeof(FreakyAutoCompleteView), Colors.Gray, BindingMode.OneWay, null, null);

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

    /// <summary>
    /// Identifies the <see cref="TextMemberPath"/> bindable property.
    /// </summary>
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

    /// <summary>
    /// Identifies the <see cref="DisplayMemberPath"/> bindable property.
    /// </summary>
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

    /// <summary>
    /// Identifies the <see cref="IsSuggestionListOpen"/> bindable property.
    /// </summary>
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

    /// <summary>
    /// Identifies the <see cref="UpdateTextOnSelect"/> bindable property.
    /// </summary>
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

    public bool IsTextPredictionEnabled => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public Keyboard Keyboard => throw new NotImplementedException();

    public int MaxLength => throw new NotImplementedException();

    public int CursorPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public int SelectionLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Microsoft.Maui.Font Font => throw new NotImplementedException();

    public double CharacterSpacing => throw new NotImplementedException();

    public string Placeholder => throw new NotImplementedException();

    public Color PlaceholderColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public TextAlignment HorizontalTextAlignment => throw new NotImplementedException();

    public TextAlignment VerticalTextAlignment => throw new NotImplementedException();

    public bool IsPassword => throw new NotImplementedException();

    public ReturnType ReturnType => throw new NotImplementedException();

    public ClearButtonVisibility ClearButtonVisibility => throw new NotImplementedException();

    /// <summary>
    /// Identifies the <see cref="ItemsSource"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(FreakyAutoCompleteView), null, BindingMode.OneWay, null, null);

    private void RaiseSuggestionChosen(object selectedItem)
    {
        SuggestionChosen?.Invoke(this, new FreakyAutoCompleteViewSuggestionChosenEventArgs(selectedItem));
    }

    /// <summary>
    /// Raised before the text content of the editable control component is updated.
    /// </summary>
    public event EventHandler<FreakyAutoCompleteViewSuggestionChosenEventArgs> SuggestionChosen;

    // Called by the native control when users enter text
    private void NativeControlTextChanged(string text, TextChangeReason reason)
    {
        suppressTextChangedEvent = true; //prevent loop of events raising, as setting this property will make it back into the native control
        Text = text;
        suppressTextChangedEvent = false;
        TextChanged?.Invoke(this, new FreakyAutoCompleteViewTextChangedEventArgs(reason));
    }

    /// <summary>
    /// Raised after the text content of the editable control component is updated.
    /// </summary>
    public event EventHandler<FreakyAutoCompleteViewTextChangedEventArgs> TextChanged;

    private void RaiseQuerySubmitted(string queryText, object chosenSuggestion)
    {
        QuerySubmitted?.Invoke(this, new FreakyAutoCompleteViewQuerySubmittedEventArgs(queryText, chosenSuggestion));
    }

    public void Completed()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Occurs when the user submits a search query.
    /// </summary>
    public event EventHandler<FreakyAutoCompleteViewQuerySubmittedEventArgs> QuerySubmitted;
}


