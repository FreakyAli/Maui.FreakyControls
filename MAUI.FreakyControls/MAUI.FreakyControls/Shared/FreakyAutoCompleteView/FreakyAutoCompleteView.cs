using System.ComponentModel;
using System.Windows.Input;
using Maui.FreakyControls.Shared.Enums;
using Font = Microsoft.Maui.Font;

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
    /// <seealso cref="PlaceholderColor"/>
    public string Placeholder
    {
        get { return (string)GetValue(PlaceholderProperty); }
        set { SetValue(PlaceholderProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="Placeholder"/> bindable property.
    /// </summary>
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the foreground color of the control
    /// </summary>
    /// <seealso cref="Placeholder"/>
    public Color PlaceholderColor
    {
        get { return (Color)GetValue(PlaceHolderColorProperty); }
        set { SetValue(PlaceHolderColorProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="PlaceholderColor"/> bindable property.
    /// </summary>
    public static readonly BindableProperty PlaceHolderColorProperty =
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

    #region Bindable Properties


    public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
        nameof(ReturnType),
        typeof(ReturnType),
        typeof(FreakyAutoCompleteView),
        ReturnType.Default
        );

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword),
        typeof(bool),
        typeof(FreakyAutoCompleteView),
        default(bool)
        );

    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
        nameof(Keyboard),
        typeof(Microsoft.Maui.Keyboard),
        typeof(FreakyAutoCompleteView),
        Keyboard.Default,
        coerceValue: (o, v) => (Keyboard)v ?? Keyboard.Default
        );

    public static readonly BindableProperty BorderStrokeThicknessProperty = BindableProperty.Create(
        nameof(BorderStrokeThickness),
        typeof(double),
        typeof(FreakyAutoCompleteView),
        default(double)
        );

    public static readonly BindableProperty BorderStrokeProperty = BindableProperty.Create(
       nameof(BorderStroke),
       typeof(Brush),
       typeof(FreakyAutoCompleteView),
       Brush.Black
       );

    public static readonly BindableProperty BorderCornerRadiusProperty = BindableProperty.Create(
       nameof(BorderCornerRadius),
       typeof(CornerRadius),
       typeof(FreakyAutoCompleteView),
       default(CornerRadius)
       );

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
      nameof(FontSize),
      typeof(double),
      typeof(FreakyAutoCompleteView),
      default(double)
       );

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource),
        typeof(ImageSource),
        typeof(FreakyAutoCompleteView),
        default(ImageSource)
        );

    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(
           nameof(ImageHeight),
           typeof(int),
           typeof(FreakyAutoCompleteView),
           25)
        ;

    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(
           nameof(ImageWidth),
           typeof(int),
           typeof(FreakyAutoCompleteView),
           25
        );

    public static readonly BindableProperty ImagePaddingProperty = BindableProperty.Create(
           nameof(ImagePadding),
           typeof(int),
           typeof(FreakyAutoCompleteView),
           5
        );

    public static readonly BindableProperty ImageCommandProperty = BindableProperty.Create(
          nameof(ImageCommand),
          typeof(ICommand),
          typeof(FreakyAutoCompleteView),
          default(ICommand)
        );

    public static readonly BindableProperty ImageCommandParameterProperty = BindableProperty.Create(
          nameof(ImageCommandParameter),
          typeof(object),
          typeof(FreakyAutoCompleteView),
          default(object)
        );

    public static readonly BindableProperty AllowCopyPasteProperty = BindableProperty.Create(
          nameof(AllowCopyPaste),
          typeof(bool),
          typeof(FreakyAutoCompleteView),
          true
        );

    public static readonly BindableProperty UnderlineColorProperty = BindableProperty.Create(
         nameof(UnderlineColor),
         typeof(Color),
         typeof(FreakyAutoCompleteView),
         Colors.Black
        );

    public static readonly BindableProperty UnderlineThicknessProperty = BindableProperty.Create(
         nameof(UnderlineThickness),
         typeof(double),
         typeof(FreakyAutoCompleteView),
         default(double)
        );

    public static readonly BindableProperty CharacterSpacingProperty = BindableProperty.Create(
         nameof(CharacterSpacing),
         typeof(double),
         typeof(FreakyAutoCompleteView),
         default(double)
        );

    public static readonly BindableProperty ClearButtonVisibilityProperty = BindableProperty.Create(
         nameof(ClearButtonVisibility),
         typeof(ClearButtonVisibility),
         typeof(FreakyAutoCompleteView),
         Microsoft.Maui.ClearButtonVisibility.Never
        );

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
         nameof(FontAttributes),
         typeof(FontAttributes),
         typeof(FreakyAutoCompleteView),
         Microsoft.Maui.Controls.FontAttributes.None
        );

    public static readonly BindableProperty CursorPositionProperty = BindableProperty.Create(
         nameof(CursorPosition),
         typeof(int),
         typeof(FreakyAutoCompleteView)
        );

    public static readonly BindableProperty FontAutoScalingEnabledProperty = BindableProperty.Create(
        nameof(FontAutoScalingEnabled),
        typeof(bool),
        typeof(FreakyAutoCompleteView),
        true
       );

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(FreakyAutoCompleteView)
       );

    public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(
        nameof(HorizontalTextAlignment),
        typeof(TextAlignment),
        typeof(FreakyAutoCompleteView)
       );

    public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create(
       nameof(VerticalTextAlignment),
       typeof(TextAlignment),
       typeof(FreakyAutoCompleteView)
      );

    public static readonly BindableProperty IsTextPredictionEnabledProperty = BindableProperty.Create(
       nameof(IsTextPredictionEnabled),
       typeof(bool),
       typeof(FreakyAutoCompleteView),
       true
      );

    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
      nameof(ReturnCommand),
      typeof(ICommand),
      typeof(FreakyAutoCompleteView)
      );

    public static readonly BindableProperty ReturnCommandParameterProperty = BindableProperty.Create(
     nameof(ReturnCommandParameter),
     typeof(object),
     typeof(FreakyAutoCompleteView)
     );

    public static readonly BindableProperty SelectionLengthProperty = BindableProperty.Create(
     nameof(SelectionLength),
     typeof(int),
     typeof(FreakyAutoCompleteView)
     );

    public static readonly BindableProperty IsSpellCheckEnabledProperty = BindableProperty.Create(
     nameof(IsSpellCheckEnabled),
     typeof(bool),
     typeof(FreakyAutoCompleteView)
     );

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
     nameof(IsReadOnly),
     typeof(bool),
     typeof(FreakyAutoCompleteView)
     );

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
     nameof(MaxLength),
     typeof(int),
     typeof(FreakyAutoCompleteView),
     int.MaxValue
     );

    public static readonly BindableProperty TextTransformProperty = BindableProperty.Create(
     nameof(TextTransform),
     typeof(TextTransform),
     typeof(FreakyAutoCompleteView),
     Microsoft.Maui.TextTransform.Default
     );

    public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(
     nameof(TitleColor),
     typeof(Color),
     typeof(FreakyAutoCompleteView),
     Colors.Black
     );

    public static readonly BindableProperty ImageAlignmentProperty = BindableProperty.Create(
     nameof(ImageAlignment),
     typeof(ImageAlignment),
     typeof(FreakyAutoCompleteView),
     ImageAlignment.Right
     );

    public static readonly BindableProperty FontProperty = BindableProperty.Create(
      nameof(Font),
      typeof(Font),
      typeof(FreakyAutoCompleteView),
      Font.Default
      );

    /// <summary>
    /// Alignment for your Image's ViewPort, By default set to Right.
    /// </summary>
    public ImageAlignment ImageAlignment
    {
        get => (ImageAlignment)GetValue(ImageAlignmentProperty);
        set => SetValue(ImageAlignmentProperty, value);
    }

    /// <summary>
    /// Color of your Title Label
    /// </summary>
    public Color TitleColor
    {
        get => (Color)GetValue(TitleColorProperty);
        set => SetValue(TitleColorProperty, value);
    }

    /// <summary>
    /// of type TextTransform, specifies the casing of the entered text.
    /// </summary>
    public TextTransform TextTransform
    {
        get => (TextTransform)GetValue(TextTransformProperty);
        set => SetValue(TextTransformProperty, value);
    }

    /// <summary>
    /// of type bool, defines whether the user should be prevented from modifying text. The default value of this property is false.
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>
    /// of type int, defines the maximum input length.
    /// </summary>
    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    /// <summary>
    /// of type bool, controls whether spell checking is enabled.
    /// </summary>
    public bool IsSpellCheckEnabled
    {
        get => (bool)GetValue(IsSpellCheckEnabledProperty);
        set => SetValue(IsSpellCheckEnabledProperty, value);
    }

    /// <summary>
    /// of type int, represents the length of selected text within the TIL.
    /// </summary>
    public int SelectionLength
    {
        get => (int)GetValue(SelectionLengthProperty);
        set => SetValue(SelectionLengthProperty, value);
    }

    /// <summary>
    /// of type object, specifies the parameter for the ReturnCommand
    /// </summary>
    public object ReturnCommandParameter
    {
        get => GetValue(ReturnCommandParameterProperty);
        set => SetValue(ReturnCommandParameterProperty, value);
    }

    /// <summary>
    /// of type ICommand, defines the command to be executed when the return key is pressed.
    /// </summary>
    public ICommand ReturnCommand
    {
        get => (ICommand)GetValue(ReturnCommandProperty);
        set => SetValue(ReturnCommandProperty, value);
    }

    /// <summary>
    /// of type bool, controls whether text prediction and automatic text correction is enabled.
    /// </summary>
    public bool IsTextPredictionEnabled
    {
        get => (bool)GetValue(IsTextPredictionEnabledProperty);
        set => SetValue(IsTextPredictionEnabledProperty, value);
    }

    /// <summary>
    /// of type TextAlignment, defines the vertical alignment of the text.
    /// </summary>
    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    /// <summary>
    /// of type TextAlignment, defines the horizontal alignment of the text.
    /// </summary>
    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    /// <summary>
    /// of type string, defines the font family.
    /// </summary>
    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    /// <summary>
    /// of type bool, defines whether the text will reflect scaling preferences set in the operating system.
    /// The default value of this property is true.
    /// </summary>
    public bool FontAutoScalingEnabled
    {
        get => (bool)GetValue(FontAutoScalingEnabledProperty);
        set => SetValue(FontAutoScalingEnabledProperty, value);
    }

    /// <summary>
    /// of type int, defines the position of the cursor within the TIL.
    /// </summary>
    public int CursorPosition
    {
        get => (int)GetValue(CursorPositionProperty);
        set => SetValue(CursorPositionProperty, value);
    }

    /// <summary>
    /// of type FontAttributes, determines text style.
    /// </summary>
    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    /// <summary>
    /// of type ClearButtonVisibility, controls whether a clear button is displayed,
    /// which enables the user to clear the text.
    /// The default value of this property ensures that a clear button isn't displayed.
    /// </summary>
    public ClearButtonVisibility ClearButtonVisibility
    {
        get => (ClearButtonVisibility)GetValue(ClearButtonVisibilityProperty);
        set => SetValue(ClearButtonVisibilityProperty, value);
    }

    /// <summary>
    /// of type double, sets the spacing between characters in the entered text.
    /// </summary>
    public double CharacterSpacing
    {
        get => (double)GetValue(CharacterSpacingProperty);
        set => SetValue(CharacterSpacingProperty, value);
    }

    /// <summary>
    /// Thickness of the Underline of your TIL 
    /// </summary>
    public double UnderlineThickness
    {
        get => (double)GetValue(UnderlineThicknessProperty);
        set => SetValue(UnderlineThicknessProperty, value);
    }

    /// <summary>
    /// Color of your TIL Underline 
    /// </summary>
    public Color UnderlineColor
    {
        get => (Color)GetValue(UnderlineColorProperty);
        set => SetValue(UnderlineColorProperty, value);
    }

    /// <summary>
    /// Gets and Sets if your TIL allows Copy Paste. default is true!
    /// </summary>
    public bool AllowCopyPaste
    {
        get => (bool)GetValue(AllowCopyPasteProperty);
        set => SetValue(AllowCopyPasteProperty, value);
    }

    /// <summary>
    /// Command parameter for your Image tap command 
    /// </summary>
    public object ImageCommandParameter
    {
        get => GetValue(ImageCommandParameterProperty);
        set => SetValue(ImageCommandParameterProperty, value);
    }

    /// <summary>
    /// A command that you can use to bind with your Image that you added to your TIL's ViewPort
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
    /// An ImageSource that you want to add to your Right ViewPort ()
    /// </summary>
    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    /// <summary>
    /// of type double, defines the font size.
    /// </summary>
    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <summary>
    /// of type CornerRadius, and defines the Cornder Radius of your Border.
    /// </summary>
    public CornerRadius BorderCornerRadius
    {
        get => (CornerRadius)GetValue(BorderCornerRadiusProperty);
        set => SetValue(BorderCornerRadiusProperty, value);
    }

    /// <summary>
    /// of type Brush, and defines the Stroke of your Border.
    /// </summary>
    public Brush BorderStroke
    {
        get => (Brush)GetValue(BorderStrokeProperty);
        set => SetValue(BorderStrokeProperty, value);
    }

    /// <summary>
    /// of type double, and defines the Thickness of the border stroke.
    /// </summary>
    public double BorderStrokeThickness
    {
        get => (double)GetValue(BorderStrokeThicknessProperty);
        set => SetValue(BorderStrokeThicknessProperty, value);
    }

    /// <summary>
    /// of type ReturnType, specifies the appearance of the return button.
    /// </summary>
    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }

    /// <summary>
    /// of type bool, specifies whether the TIL should visually obscure typed text.
    /// </summary>
    public bool IsPassword
    {
        get { return (bool)GetValue(IsPasswordProperty); }
        set { SetValue(IsPasswordProperty, value); }
    }

    /// <summary>
    /// of type Keyboard, specifies the virtual keyboard that's displayed when entering text.
    /// </summary>
    [System.ComponentModel.TypeConverter(typeof(Microsoft.Maui.Converters.KeyboardTypeConverter))]
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    /// <summary>
    /// of type Font, 
    /// </summary>
    public Font Font
    {
        get => (Font)GetValue(FontProperty);
        set => SetValue(FontProperty, value);
    }

    #endregion

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
    }

    /// <summary>
    /// Occurs when the user submits a search query.
    /// </summary>
    public event EventHandler<FreakyAutoCompleteViewQuerySubmittedEventArgs> QuerySubmitted;
}


