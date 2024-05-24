using Maui.FreakyControls.Shared.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maui.FreakyControls;

public partial class FreakyTextInputLayout : ContentView, IDisposable
{
    private int _leftMargin;
    private double _placeholderFontSize = 18;
    private double _titleFontSize = 14;

    public FreakyTextInputLayout()
    {
        InitializeComponent();
        LabelTitle.TranslationX = 10;
        LabelTitle.FontSize = _placeholderFontSize;
    }

    /// <summary>
    /// raised when the user finalizes text in the <see cref="FreakyTextInputLayout"/> with the return key.
    /// </summary>
    public event EventHandler Completed;

    /// <summary>
    ///  raised when the text in the <see cref="FreakyTextInputLayout"/> changes.
    ///  The <see cref="TextChangedEventArgs"/> object that accompanies the TextChanged event has NewTextValue and OldTextValue properties,
    ///  which specify the new and old text, respectively.
    /// </summary>
    public event EventHandler<TextChangedEventArgs> TextChanged;

    #region Bindable Properties

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(FreakyTextInputLayout),
        string.Empty,
        BindingMode.TwoWay
        );

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(FreakyTextInputLayout),
        string.Empty,
        BindingMode.TwoWay,
        null
        );

    public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
        nameof(ReturnType),
        typeof(ReturnType),
        typeof(FreakyTextInputLayout),
        ReturnType.Default
        );

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword),
        typeof(bool),
        typeof(FreakyTextInputLayout),
        default(bool)
        );

    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
        nameof(Keyboard),
        typeof(Keyboard),
        typeof(FreakyTextInputLayout),
        Keyboard.Default,
        coerceValue: (o, v) => (Keyboard)v ?? Keyboard.Default
        );

    public static readonly BindableProperty BorderStrokeThicknessProperty = BindableProperty.Create(
        nameof(BorderStrokeThickness),
        typeof(double),
        typeof(FreakyTextInputLayout),
        default(double)
        );

    public static readonly BindableProperty BorderStrokeProperty = BindableProperty.Create(
       nameof(BorderStroke),
       typeof(Brush),
       typeof(FreakyTextInputLayout),
       Brush.Black
       );

    public static readonly BindableProperty BorderCornerRadiusProperty = BindableProperty.Create(
       nameof(BorderCornerRadius),
       typeof(CornerRadius),
       typeof(FreakyTextInputLayout),
       default(CornerRadius)
       );

    public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(
        nameof(TitleFontSize),
        typeof(double),
        typeof(FreakyTextInputLayout),
        default(double),
        propertyChanged: OnTitleFontSizeChanged);

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
          nameof(FontSize),
          typeof(double),
          typeof(FreakyTextInputLayout),
          default(double),
          propertyChanged: OnFontSizeChanged);

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource),
        typeof(ImageSource),
        typeof(FreakyTextInputLayout),
        default(ImageSource),
        propertyChanged: OnImageSourceChanged);

    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(
           nameof(ImageHeight),
           typeof(int),
           typeof(FreakyTextInputLayout),
           25);

    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(
           nameof(ImageWidth),
           typeof(int),
           typeof(FreakyTextInputLayout),
           25,
           propertyChanged: OnImageWidthChanged);

    public static readonly BindableProperty ImagePaddingProperty = BindableProperty.Create(
           nameof(ImagePadding),
           typeof(int),
           typeof(FreakyTextInputLayout),
           5);

    public static readonly BindableProperty ImageCommandProperty = BindableProperty.Create(
          nameof(ImageCommand),
          typeof(ICommand),
          typeof(FreakyTextInputLayout),
          default(ICommand)
        );

    public static readonly BindableProperty ImageCommandParameterProperty = BindableProperty.Create(
          nameof(ImageCommandParameter),
          typeof(object),
          typeof(FreakyTextInputLayout),
          default
        );

    public static readonly BindableProperty AllowCopyPasteProperty = BindableProperty.Create(
          nameof(AllowCopyPaste),
          typeof(bool),
          typeof(FreakyTextInputLayout),
          true
        );

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
          nameof(TextColor),
          typeof(Color),
          typeof(FreakyTextInputLayout),
          Colors.Black
        );

    public static readonly BindableProperty UnderlineColorProperty = BindableProperty.Create(
         nameof(UnderlineColor),
         typeof(Color),
         typeof(FreakyTextInputLayout),
         Colors.Black
        );

    public static readonly BindableProperty UnderlineThicknessProperty = BindableProperty.Create(
         nameof(UnderlineThickness),
         typeof(double),
         typeof(FreakyTextInputLayout),
         default(double)
        );

    public static readonly BindableProperty CharacterSpacingProperty = BindableProperty.Create(
         nameof(CharacterSpacing),
         typeof(double),
         typeof(FreakyTextInputLayout),
         default(double)
        );

    public static readonly BindableProperty ClearButtonVisibilityProperty = BindableProperty.Create(
         nameof(ClearButtonVisibility),
         typeof(ClearButtonVisibility),
         typeof(FreakyTextInputLayout),
         ClearButtonVisibility.Never
        );

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
         nameof(FontAttributes),
         typeof(FontAttributes),
         typeof(FreakyTextInputLayout),
         FontAttributes.None
        );

    public static readonly BindableProperty CursorPositionProperty = BindableProperty.Create(
         nameof(CursorPosition),
         typeof(int),
         typeof(FreakyTextInputLayout)
        );

    public static readonly BindableProperty FontAutoScalingEnabledProperty = BindableProperty.Create(
        nameof(FontAutoScalingEnabled),
        typeof(bool),
        typeof(FreakyTextInputLayout),
        true
       );

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(FreakyTextInputLayout)
       );

    public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(
        nameof(HorizontalTextAlignment),
        typeof(TextAlignment),
        typeof(FreakyTextInputLayout)
       );

    public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create(
       nameof(VerticalTextAlignment),
       typeof(TextAlignment),
       typeof(FreakyTextInputLayout)
      );

    public static readonly BindableProperty IsTextPredictionEnabledProperty = BindableProperty.Create(
       nameof(IsTextPredictionEnabled),
       typeof(bool),
       typeof(FreakyTextInputLayout),
       true
      );

    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
      nameof(ReturnCommand),
      typeof(ICommand),
      typeof(FreakyTextInputLayout)
      );

    public static readonly BindableProperty ReturnCommandParameterProperty = BindableProperty.Create(
     nameof(ReturnCommandParameter),
     typeof(object),
     typeof(FreakyTextInputLayout)
     );

    public static readonly BindableProperty SelectionLengthProperty = BindableProperty.Create(
     nameof(SelectionLength),
     typeof(int),
     typeof(FreakyTextInputLayout)
     );

    public static readonly BindableProperty IsSpellCheckEnabledProperty = BindableProperty.Create(
     nameof(IsSpellCheckEnabled),
     typeof(bool),
     typeof(FreakyTextInputLayout)
     );

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
     nameof(IsReadOnly),
     typeof(bool),
     typeof(FreakyTextInputLayout)
     );

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
     nameof(MaxLength),
     typeof(int),
     typeof(FreakyTextInputLayout),
     int.MaxValue
     );

    public static readonly BindableProperty TextTransformProperty = BindableProperty.Create(
     nameof(TextTransform),
     typeof(TextTransform),
     typeof(FreakyTextInputLayout),
     TextTransform.Default
     );

    public static readonly BindableProperty BorderTypeProperty = BindableProperty.Create(
     nameof(BorderType),
     typeof(BorderType),
     typeof(FreakyTextInputLayout),
     BorderType.None,
     propertyChanged: BorderTypePropertyChanged
     );

    public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(
     nameof(TitleColor),
     typeof(Color),
     typeof(FreakyTextInputLayout),
     Colors.Black
     );

    public static readonly BindableProperty OutlineTitleBackgroundColorProperty = BindableProperty.Create(
        nameof(OutlineTitleBackgroundColor),
        typeof(Color),
        typeof(FreakyTextInputLayout),
        Colors.White,
        propertyChanged: OnOutlineTitleBackgroundColorProperty
        );

    public static readonly BindableProperty ControlBackgroundColorProperty = BindableProperty.Create(
        nameof(ControlBackgroundColor),
        typeof(Color),
        typeof(FreakyTextInputLayout),
        Colors.Transparent
        );

    /// <summary>ß
    /// Color of your Title Label's background when border type is outlined
    /// </summary>
    public Color ControlBackgroundColor
    {
        get => (Color)GetValue(ControlBackgroundColorProperty);
        set => SetValue(ControlBackgroundColorProperty, value);
    }

    /// <summary>ß
    /// Color of your Title Label's background when border type is outlined
    /// </summary>
    public Color OutlineTitleBackgroundColor
    {
        get => (Color)GetValue(OutlineTitleBackgroundColorProperty);
        set => SetValue(OutlineTitleBackgroundColorProperty, value);
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
    /// Type of Border you want for your <see cref="FreakyTextInputLayout", By default set to None.
    /// </summary>
    public BorderType BorderType
    {
        get => (BorderType)GetValue(BorderTypeProperty);
        set => SetValue(BorderTypeProperty, value);
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
    /// of type int, represents the length of selected text within the <see cref="FreakyTextInputLayout".
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
    /// of type int, defines the position of the cursor within the <see cref="FreakyTextInputLayout"/>.
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
    /// Thickness of the Underline of your <see cref="FreakyTextInputLayout"
    /// </summary>
    public double UnderlineThickness
    {
        get => (double)GetValue(UnderlineThicknessProperty);
        set => SetValue(UnderlineThicknessProperty, value);
    }

    /// <summary>
    /// Color of your <see cref="FreakyTextInputLayout" Underline
    /// </summary>
    public Color UnderlineColor
    {
        get => (Color)GetValue(UnderlineColorProperty);
        set => SetValue(UnderlineColorProperty, value);
    }

    /// <summary>
    /// of type Color, defines the color of the entered text.
    /// </summary>
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    /// <summary>
    /// Gets and Sets if your <see cref="FreakyTextInputLayout" allows Copy Paste. default is true!
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
    /// A command that you can use to bind with your Image that you added to your <see cref="FreakyTextInputLayout"'s ViewPort
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
    /// of type double, defines the title font size.
    /// </summary>
    [TypeConverter(typeof(FontSizeConverter))]
    public double TitleFontSize
    {
        get => (double)GetValue(TitleFontSizeProperty);
        set => SetValue(TitleFontSizeProperty, value);
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
    /// of type string, defines the text entered into the <see cref="FreakyTextInputLayout".
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// of type string, defines the text to become the placeholder for the <see cref="FreakyTextInputLayout".
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
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
    /// of type bool, specifies whether the <see cref="FreakyTextInputLayout" should visually obscure typed text.
    /// </summary>
    public bool IsPassword
    {
        get { return (bool)GetValue(IsPasswordProperty); }
        set { SetValue(IsPasswordProperty, value); }
    }

    /// <summary>
    /// of type Keyboard, specifies the virtual keyboard that's displayed when entering text.
    /// </summary>
    [TypeConverter(typeof(Microsoft.Maui.Converters.KeyboardTypeConverter))]
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    private static void BorderTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FreakyTextInputLayout;
        switch (control.BorderType)
        {
            case BorderType.Full:
                control._leftMargin = 0;
                control.LabelTitle.BackgroundColor = Colors.Transparent;
                break;
            case BorderType.Outlined:
                control._leftMargin = 20;
                control.LabelTitle.BackgroundColor = control.OutlineTitleBackgroundColor;
                break;
            case BorderType.None:
            case BorderType.Underline:
                control._leftMargin = 0;
                control.LabelTitle.BackgroundColor = Colors.Transparent;
                break;
        }
    }

    private static void OnOutlineTitleBackgroundColorProperty(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FreakyTextInputLayout til && newValue is Color color)
        {
            til.LabelTitle.BackgroundColor = til.BorderType ==
                BorderType.Outlined ? color : Colors.Transparent;
        }
    }

    private static void OnTitleFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FreakyTextInputLayout;
        control._titleFontSize = (double)newValue;
    }

    private static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FreakyTextInputLayout;
        control._placeholderFontSize = (double)newValue;
        control.LabelTitle.FontSize = control._placeholderFontSize;
    }

    private static void OnImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        //fix placeholder width overlapping with the image if the image source changes
        var control = bindable as FreakyTextInputLayout;
        if (control.ImageWidth > 0)
        {
            control.LabelTitle.Margin = new Thickness(control.LabelTitle.Margin.Left, control.LabelTitle.Margin.Top, control.ImageWidth + (control.ImagePadding * 2), control.LabelTitle.Margin.Bottom);
        }
        else
        {
            control.LabelTitle.Margin = new Thickness(control.LabelTitle.Margin.Left, control.LabelTitle.Margin.Top, 0, control.LabelTitle.Margin.Bottom);
        }
    }

    private static void OnImageWidthChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FreakyTextInputLayout;
        if (control.ImageSource == null)
        {
            control.LabelTitle.Margin = new Thickness(control.LabelTitle.Margin.Left, control.LabelTitle.Margin.Top, 0, control.LabelTitle.Margin.Bottom);
        }
        else
        {
            control.LabelTitle.Margin = new Thickness(control.LabelTitle.Margin.Left, control.LabelTitle.Margin.Top, (int)newValue + (control.ImagePadding * 2), control.LabelTitle.Margin.Bottom);
        }
    }

    #endregion Bindable Properties

    public new void Focus()
    {
        if (IsEnabled)
        {
            EntryField.Focus();
        }
    }

    private async void Handle_Focused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            await TransitionToTitle(true);
        }
    }

    private async void Handle_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            await TransitionToPlaceholder(true);
        }
    }

    private async Task TransitionToTitle(bool animated)
    {
        double yoffset = 0;

        //calculate the top margin based on the title font size
        if (this.BorderType == BorderType.Outlined || this.BorderType == BorderType.Underline)
            yoffset = -(ctrlBorder.Height / 2);
        else if (this.BorderType == BorderType.Full)
            yoffset = -((ctrlBorder.Height / 2) + (hiddenTitle.Height / 2));

        if (animated)
        {
            var t1 = LabelTitle.TranslateTo(_leftMargin, yoffset, 100);
            var t2 = SizeTo(_titleFontSize);
            await Task.WhenAll(t1, t2);
        }
        else
        {
            LabelTitle.TranslationX = _leftMargin;
            LabelTitle.TranslationY = yoffset;
            LabelTitle.FontSize = _titleFontSize;
        }
    }

    private async Task TransitionToPlaceholder(bool animated)
    {
        if (animated)
        {
            var t1 = LabelTitle.TranslateTo(10, 0, 250, Easing.Linear);
            var t2 = SizeTo(_placeholderFontSize);
            await Task.WhenAll(t1, t2);
        }
        else
        {
            LabelTitle.TranslationX = 10;
            LabelTitle.TranslationY = 0;
            LabelTitle.FontSize = _placeholderFontSize;
        }
    }

    private void Handle_Tapped(object sender, EventArgs e)
    {
        if (IsEnabled)
        {
            EntryField.Focus();
        }
    }

    private Task<bool> SizeTo(double fontSize)
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();

        // setup information for animation
        Action<double> callback = input => { LabelTitle.FontSize = input; };
        double startingHeight = LabelTitle.FontSize;
        double endingHeight = fontSize;
        uint rate = 10;
        uint length = 250;
        Easing easing = Easing.Linear;

        // now start animation with all the setup information
        LabelTitle.Animate("animate", callback, startingHeight, endingHeight, rate, length, easing, (v, c) => taskCompletionSource.SetResult(c));

        return taskCompletionSource.Task;
    }

    private void Handle_Completed(object sender, EventArgs e)
    {
        Completed?.Invoke(this, e);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(IsEnabled))
        {
            EntryField.IsEnabled = IsEnabled;
        }
    }

    private void EntryField_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextChanged?.Invoke(this, e);
    }

    private void HiddenTitle_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Height) && BorderType == BorderType.Full)
        {
            //add margin so there is space to prevent overlap with surrounding controls
            ctrlGrid.Margin = new Thickness(ctrlGrid.Margin.Left, hiddenTitle.Height, ctrlGrid.Margin.Right, ctrlGrid.Margin.Bottom);
        }
    }

    private async void EntryField_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Height) && !string.IsNullOrEmpty(EntryField?.Text))
        {
            //Make label floating if the entry field already has text it in when it is loaded
            await TransitionToTitle(false);
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~FreakyTextInputLayout()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {

    }
}