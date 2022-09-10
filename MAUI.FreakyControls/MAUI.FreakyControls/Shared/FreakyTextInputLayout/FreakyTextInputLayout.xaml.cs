using System.ComponentModel;
using System.Runtime.CompilerServices;
using Maui.FreakyControls.Shared.Enums;
using System.Windows.Input;
using Microsoft.Maui.Graphics.Text;

namespace Maui.FreakyControls;

public partial class FreakyTextInputLayout : ContentView
{
    public FreakyTextInputLayout()
    {
        InitializeComponent();
        LabelTitle.TranslationX = 10;
        LabelTitle.FontSize = _placeholderFontSize;
#if ANDROID
        _topMargin = this.BorderType == BorderType.Full ? 35 : -45;
#endif
#if IOS
        _topMargin= -35;
#endif
    }


    private int _topMargin;
    int _placeholderFontSize = 18;
    int _titleFontSize = 14;

    public event EventHandler Completed;
    public event EventHandler<TextChangedEventArgs> TextChanged;

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(FreakyTextInputLayout),
        string.Empty,
        BindingMode.TwoWay,
        null,
        HandleBindingPropertyChangedDelegate
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
        typeof(Microsoft.Maui.Keyboard),
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

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
      nameof(FontSize),
      typeof(double),
      typeof(FreakyTextInputLayout),
      default(double)
       );

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource),
        typeof(ImageSource),
        typeof(FreakyTextInputLayout),
        default(ImageSource)
        );

    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(
           nameof(ImageHeight),
           typeof(int),
           typeof(FreakyTextInputLayout),
           25)
        ;

    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(
           nameof(ImageWidth),
           typeof(int),
           typeof(FreakyTextInputLayout),
           25
        );

    public static readonly BindableProperty ImagePaddingProperty = BindableProperty.Create(
           nameof(ImagePadding),
           typeof(int),
           typeof(FreakyTextInputLayout),
           5
        );

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
          default(object)
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
         Microsoft.Maui.ClearButtonVisibility.Never
        );

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
         nameof(FontAttributes),
         typeof(FontAttributes),
         typeof(FreakyTextInputLayout),
         Microsoft.Maui.Controls.FontAttributes.None
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
     Microsoft.Maui.TextTransform.Default
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

    public Color TitleColor
    {
        get => (Color)GetValue(TitleColorProperty);
        set => SetValue(TitleColorProperty, value);
    }

    public BorderType BorderType
    {
        get => (BorderType)GetValue(BorderTypeProperty);
        set => SetValue(BorderTypeProperty, value);
    }

    public TextTransform TextTransform
    {
        get => (TextTransform)GetValue(TextTransformProperty);
        set => SetValue(TextTransformProperty, value);
    }

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public bool IsSpellCheckEnabled
    {
        get => (bool)GetValue(IsSpellCheckEnabledProperty);
        set => SetValue(IsSpellCheckEnabledProperty, value);
    }

    public int SelectionLength
    {
        get => (int)GetValue(SelectionLengthProperty);
        set => SetValue(SelectionLengthProperty, value);
    }

    public object ReturnCommandParameter
    {
        get => GetValue(ReturnCommandParameterProperty);
        set => SetValue(ReturnCommandParameterProperty, value);
    }

    public ICommand ReturnCommand
    {
        get => (ICommand)GetValue(ReturnCommandProperty);
        set => SetValue(ReturnCommandProperty, value);
    }

    public bool IsTextPredictionEnabled
    {
        get => (bool)GetValue(IsTextPredictionEnabledProperty);
        set => SetValue(IsTextPredictionEnabledProperty, value);
    }

    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public bool FontAutoScalingEnabled
    {
        get => (bool)GetValue(FontAutoScalingEnabledProperty);
        set => SetValue(FontAutoScalingEnabledProperty, value);
    }

    public int CursorPosition
    {
        get => (int)GetValue(CursorPositionProperty);
        set => SetValue(CursorPositionProperty, value);
    }

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public ClearButtonVisibility ClearButtonVisibility
    {
        get => (ClearButtonVisibility)GetValue(ClearButtonVisibilityProperty);
        set => SetValue(ClearButtonVisibilityProperty, value);
    }

    public double CharacterSpacing
    {
        get => (double)GetValue(CharacterSpacingProperty);
        set => SetValue(CharacterSpacingProperty, value);
    }

    public double UnderlineThickness
    {
        get => (double)GetValue(UnderlineThicknessProperty);
        set => SetValue(UnderlineThicknessProperty, value);
    }

    public Color UnderlineColor
    {
        get => (Color)GetValue(UnderlineColorProperty);
        set => SetValue(UnderlineColorProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public bool AllowCopyPaste
    {
        get => (bool)GetValue(AllowCopyPasteProperty);
        set => SetValue(AllowCopyPasteProperty, value);
    }

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

    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public CornerRadius BorderCornerRadius
    {
        get => (CornerRadius)GetValue(BorderCornerRadiusProperty);
        set => SetValue(BorderCornerRadiusProperty, value);
    }

    public Brush BorderStroke
    {
        get => (Brush)GetValue(BorderStrokeProperty);
        set => SetValue(BorderStrokeProperty, value);
    }

    public double BorderStrokeThickness
    {
        get => (double)GetValue(BorderStrokeThicknessProperty);
        set => SetValue(BorderStrokeThicknessProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }

    public bool IsPassword
    {
        get { return (bool)GetValue(IsPasswordProperty); }
        set { SetValue(IsPasswordProperty, value); }
    }

    [System.ComponentModel.TypeConverter(typeof(Microsoft.Maui.Converters.KeyboardTypeConverter))]
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    static void BorderTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FreakyTextInputLayout;
        switch (control.BorderType)
        {
            case BorderType.None:
                break;
            case BorderType.Underline:
#if IOS
#endif
                break;
            case BorderType.Full:
#if ANDROID
#endif
                break;
        }
    }

    static async void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FreakyTextInputLayout;
        if (!control.EntryField.IsFocused)
        {
            if (!string.IsNullOrEmpty((string)newValue))
            {
                await control.TransitionToTitle(false);
            }
            else
            {
                await control.TransitionToPlaceholder(false);
            }
        }
    }

    public new void Focus()
    {
        if (IsEnabled)
        {
            EntryField.Focus();
        }
    }

    async void Handle_Focused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            await TransitionToTitle(true);
        }
    }

    async void Handle_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            await TransitionToPlaceholder(true);
        }
    }

    async Task TransitionToTitle(bool animated)
    {
        if (animated)
        {
            var t1 = LabelTitle.TranslateTo(0, _topMargin, 100);
            var t2 = SizeTo(_titleFontSize);
            await Task.WhenAll(t1, t2);
        }
        else
        {
            LabelTitle.TranslationX = 0;
            LabelTitle.TranslationY = -30;
            LabelTitle.FontSize = 14;
        }
    }

    async Task TransitionToPlaceholder(bool animated)
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

    void Handle_Tapped(object sender, EventArgs e)
    {
        if (IsEnabled)
        {
            EntryField.Focus();
        }
    }

    Task SizeTo(int fontSize)
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

    void Handle_Completed(object sender, EventArgs e)
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

    void EntryField_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        TextChanged?.Invoke(this, e);
    }
}
