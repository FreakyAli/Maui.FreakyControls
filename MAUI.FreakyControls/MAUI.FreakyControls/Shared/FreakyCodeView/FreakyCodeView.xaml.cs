using System.Diagnostics;
using System.Windows.Input;
using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public partial class FreakyCodeView : ContentView
{
    #region Events
    public event EventHandler<FreakyCodeCompletedEventArgs> CodeEntryCompleted;
    #endregion

    #region Constructor and Initializations
    public FreakyCodeView()
    {
        InitializeComponent();
        hiddenTextEntry.TextChanged += FreakyCodeView_TextChanged;
        hiddenTextEntry.Focused += HiddenTextEntry_Focused;
        hiddenTextEntry.Unfocused += HiddenTextEntry_Unfocused;
        CreateControl();
    }

    private void HiddenTextEntry_Unfocused(object sender, FocusEventArgs e)
    {
        var CodeItemArray = CodeItemContainer.Children.Select(x => x as Container).ToList();
        for (int i = 0; i < CodeLength; i++)
        {
            CodeItemArray[i].UnfocusAnimate();
        }
    }

    private void HiddenTextEntry_Focused(object sender, FocusEventArgs e)
    {
        var length = CodeValue == null ? 0 : CodeValue.Length;
        hiddenTextEntry.CursorPosition = length;

        var CodeItemArray = CodeItemContainer.Children.Select(x => x as Container).ToArray();

        if (length == CodeLength)
        {
            CodeItemArray[length - 1].FocusAnimate();
        }
        else
        {
            for (int i = 0; i < CodeLength; i++)
            {
                if (i == length)
                {
                    CodeItemArray[i].FocusAnimate();
                }
                else
                {
                    CodeItemArray[i].UnfocusAnimate();
                }
            }
        }
    }

    #endregion

    #region Methods

    public void CreateControl()
    {
        hiddenTextEntry.MaxLength = CodeLength;
        SetInputType(CodeInputType);

        var count = CodeItemContainer.Children.Count;

        if (count < CodeLength)
        {
            int newItemesToAdd = CodeLength - count;
            char[] CodeCharsArray = CodeValue.ToCharArray();

            for (int i = 1; i <= newItemesToAdd; i++)
            {
                Container container = CreateItem();
                CodeItemContainer.Children.Add(container);
                if (CodeValue.Length >= CodeLength)
                {
                    container.SetValueWithAnimation(CodeCharsArray[Container.DefaultCodeLength + i - 1]);
                }
            }
        }
        else if (count > CodeLength)
        {
            int ItemesToRemove = count - CodeLength;
            for (int i = 1; i <= ItemesToRemove; i++)
            {
                CodeItemContainer.Children.RemoveAt(CodeItemContainer.Children.Count - 1);
            }
        }
    }

    public new void Focus()
    {
        base.Focus();
        hiddenTextEntry.Focus();
    }

    private Container CreateItem(char? charValue = null)
    {
        Container container = new()
        {
            HeightRequest = ItemSize,
            WidthRequest = ItemSize,
            ItemFocusColor = ItemFocusColor,
            FocusAnimationType = ItemFocusAnimation
        };
        container.Item.BackgroundColor = ItemBackgroundColor;
        container.CharLabel.FontSize = ItemSize / 2;
        container.SecureMode(IsPassword);
        container.SetColor(Color, ItemBorderColor);
        container.SetRadius(ItemShape);
        if (charValue.HasValue)
        {
            container.SetValueWithAnimation(charValue.Value);
        }
        return container;
    }

    #endregion

    #region Events
    private void FreakyCodeView_TextChanged(object sender, TextChangedEventArgs e)
    {
        CodeValue = e.NewTextValue;

        if (e.NewTextValue.Length >= CodeLength)
        {
            if (ShouldAutoDismissKeyboard == true)
            {
                (sender as Entry).Unfocus();
            }
            CodeEntryCompleted?.Invoke(this, new FreakyCodeCompletedEventArgs(CodeValue));
            CodeEntryCompletedCommand?.Execute(CodeValue);
        }
    }
    #endregion

    public string CodeValue
    {
        get => (string)GetValue(CodeValueProperty);
        set => SetValue(CodeValueProperty, value);
    }

    public static readonly BindableProperty CodeValueProperty =
       BindableProperty.Create(
           nameof(CodeValue),
           typeof(string),
           typeof(FreakyCodeView),
           string.Empty,
           defaultBindingMode: BindingMode.TwoWay,
           propertyChanged: CodeValuePropertyChanged);

    private static async void CodeValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        try
        {
            var control = (FreakyCodeView)bindable;

            string newCode = newValue.ToString();
            string oldCode = oldValue.ToString();

            int newCodeLength = newCode.Length;
            int oldCodeLength = oldCode.Length;

            if (newCodeLength == 0 && oldCodeLength == 0)
            {
                return;
            }

            char[] newCodeChars = newCode.ToCharArray();

            control.hiddenTextEntry.Text = newCode;
            var CodeItemArray = control.CodeItemContainer.Children.Select(x => x as Container).ToArray();

            bool isCodeEnteredProgramatically = (oldCodeLength == 0 && newCodeLength == control.CodeLength) || newCodeLength == oldCodeLength;

            if (isCodeEnteredProgramatically)
            {
                for (int i = 0; i < control.CodeLength; i++)
                {
                    CodeItemArray[i].ClearValueWithAnimation();
                }
            }

            for (int i = 0; i < control.CodeLength; i++)
            {
                if (i < newCodeLength)
                {
                    if (isCodeEnteredProgramatically)
                    {
                        await Task.Delay(50);
                    }

                    CodeItemArray[i].SetValueWithAnimation(newCodeChars[i]);
                }
                else
                {
                    if (CodeItemArray.Length >= control.CodeLength)
                    {
                        CodeItemArray[i].ClearValueWithAnimation();
                        CodeItemArray[i].UnfocusAnimate();
                    }
                }
            }

            if (control.hiddenTextEntry.IsFocused)
            {
                if (newCodeLength < control.CodeLength)
                {
                    CodeItemArray[newCodeLength].FocusAnimate();
                }
                else if (newCodeLength == control.CodeLength)
                {
                    CodeItemArray[newCodeLength - 1].FocusAnimate();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    public int CodeLength
    {
        get => (int)GetValue(CodeLengthProperty);
        set => SetValue(CodeLengthProperty, value);
    }

    public static readonly BindableProperty CodeLengthProperty =
      BindableProperty.Create(
          nameof(CodeLength),
          typeof(int),
          typeof(FreakyCodeView),
          Container.DefaultCodeLength,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: CodeLengthPropertyChanged);

    private static void CodeLengthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((int)newValue <= 0)
        {
            return;
        }

       ((FreakyCodeView)bindable).CreateControl();
    }

    public KeyboardType CodeInputType
    {
        get => (KeyboardType)GetValue(CodeInputTypeProperty);
        set => SetValue(CodeInputTypeProperty, value);
    }

    public static readonly BindableProperty CodeInputTypeProperty =
       BindableProperty.Create(
           nameof(CodeInputType),
           typeof(KeyboardType),
           typeof(FreakyCodeView),
            KeyboardType.Numeric,
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: CodeInputTypePropertyChanged);

    private static void CodeInputTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = ((FreakyCodeView)bindable);
        control.SetInputType((KeyboardType)newValue);
    }

    public void SetInputType(KeyboardType inputKeyboardType)
    {
        if (inputKeyboardType == KeyboardType.Numeric)
        {
            hiddenTextEntry.Keyboard = Keyboard.Numeric;
        }
        else if (inputKeyboardType == KeyboardType.AlphaNumeric)
        {
            hiddenTextEntry.Keyboard = Keyboard.Create(0);
        }
    }

    public ICommand CodeEntryCompletedCommand
    {
        get { return (ICommand)GetValue(CodeEntryCompletedCommandProperty); }
        set { SetValue(CodeEntryCompletedCommandProperty, value); }
    }

    public static readonly BindableProperty CodeEntryCompletedCommandProperty =
       BindableProperty.Create(
          nameof(CodeEntryCompletedCommand),
          typeof(ICommand),
          typeof(FreakyCodeView),
          null);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public static readonly BindableProperty IsPasswordProperty =
      BindableProperty.Create(
          nameof(IsPassword),
          typeof(bool),
          typeof(FreakyCodeView),
          true,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: IsPasswordPropertyChanged);

    private static void IsPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = ((FreakyCodeView)bindable);
        foreach (var x in control.CodeItemContainer.Children)
        {
            var container = (Container)x;
            container.SecureMode((bool)newValue);
        }
    }

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly BindableProperty ColorProperty =
      BindableProperty.Create(
          nameof(Color),
          typeof(Color),
          typeof(FreakyCodeView),
          Colors.Black,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: ColorPropertyChanged);

    private static void ColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = ((FreakyCodeView)bindable);
        foreach (var x in control.CodeItemContainer.Children)
        {
            var container = (Container)x;
            container.SetColor(color: (Color)newValue, ItemBorderColor: control.ItemBorderColor);
        }
    }

    public double ItemSpacing
    {
        get => (double)GetValue(ItemSpacingProperty);
        set => SetValue(ItemSpacingProperty, value);
    }

    public static readonly BindableProperty ItemSpacingProperty =
      BindableProperty.Create(
          nameof(ItemSpacing),
          typeof(double),
          typeof(FreakyCodeView),
          Container.DefaultItemSpacing,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: ItemSpacingPropertyChanged);

    private static void ItemSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((double)newValue < 0)
        {
            return;
        }
        var control = ((FreakyCodeView)bindable);
        control.CodeItemContainer.Spacing = (double)newValue;
    }

    public double ItemSize
    {
        get => (double)GetValue(ItemSizeProperty);
        set => SetValue(ItemSizeProperty, value);
    }

    public static readonly BindableProperty ItemSizeProperty =
      BindableProperty.Create(
          nameof(ItemSize),
          typeof(double),
          typeof(FreakyCodeView),
          Container.DefaultItemSize,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: ItemSizePropertyChanged);

    private static void ItemSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((double)newValue < 0)
        {
            return;
        }

        var control = ((FreakyCodeView)bindable);
        foreach (var x in control.CodeItemContainer.Children)
        {
            var container = (Container)x;
            container.HeightRequest = (double)newValue;
            container.WidthRequest = (double)newValue;
            container.CharLabel.FontSize = ((double)newValue / 2);
            container.SetRadius(control.ItemShape);
        }
    }

    public ItemShape ItemShape
    {
        get => (ItemShape)GetValue(ItemShapeProperty);
        set => SetValue(ItemShapeProperty, value);
    }

    public static readonly BindableProperty ItemShapeProperty =
       BindableProperty.Create(
           nameof(ItemShape),
           typeof(ItemShape),
           typeof(FreakyCodeView),
           ItemShape.Circle,
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: ItemShapePropertyChanged);

    private static void ItemShapePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = ((FreakyCodeView)bindable);
        foreach (var x in control.CodeItemContainer.Children)
        {
            var container = (Container)x;
            container.SetRadius((ItemShape)newValue);
        }
    }

    public Color ItemFocusColor
    {
        get => (Color)GetValue(ItemFocusColorProperty);
        set => SetValue(ItemFocusColorProperty, value);
    }

    public static readonly BindableProperty ItemFocusColorProperty =
      BindableProperty.Create(
          nameof(ItemFocusColor),
          typeof(Color),
          typeof(FreakyCodeView),
          Colors.Black,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: ItemFocusColorPropertyChanged);

    private static void ItemFocusColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = ((FreakyCodeView)bindable);
        foreach (var x in control.CodeItemContainer.Children)
        {
            var container = (Container)x;
            container.ItemFocusColor = (Color)newValue;
        }
    }

    public FocusAnimation ItemFocusAnimation
    {
        get => (FocusAnimation)GetValue(ItemFocusAnimationProperty);
        set => SetValue(ItemFocusAnimationProperty, value);
    }

    public static readonly BindableProperty ItemFocusAnimationProperty =
       BindableProperty.Create(
           nameof(ItemFocusAnimation),
           typeof(FocusAnimation),
           typeof(FreakyCodeView),
           default(FocusAnimation),
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: ItemFocusAnimationPropertyChanged);

    private static void ItemFocusAnimationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = ((FreakyCodeView)bindable);
        foreach (var x in control.CodeItemContainer.Children)
        {
            var container = (Container)x;
            container.FocusAnimationType = (FocusAnimation)newValue;
        }
    }

    public Color ItemBorderColor
    {
        get => (Color)GetValue(ItemBorderColorProperty);
        set => SetValue(ItemBorderColorProperty, value);
    }

    public static readonly BindableProperty ItemBorderColorProperty =
      BindableProperty.Create(
          nameof(ItemBorderColor),
          typeof(Color),
          typeof(FreakyCodeView),
          Colors.Black,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: ItemBorderColorPropertyChanged);

    private static void ItemBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FreakyCodeView)bindable;
        if (control.Color != (Color)newValue)
        {
            foreach (var x in ((FreakyCodeView)bindable).CodeItemContainer.Children)
            {
                var container = (Container)x;
                container.SetColor(color: control.Color, ItemBorderColor: (Color)newValue);
            }
        }
    }

    public Color ItemBackgroundColor
    {
        get => (Color)GetValue(ItemBackgroundColorProperty);
        set => SetValue(ItemBackgroundColorProperty, value);
    }

    public static readonly BindableProperty ItemBackgroundColorProperty =
      BindableProperty.Create(
          nameof(ItemBackgroundColor),
          typeof(Color),
          typeof(FreakyCodeView),
          default(Color),
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: ItemBackgroundColorPropertyChanged);

    private static void ItemBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        foreach (var x in ((FreakyCodeView)bindable).CodeItemContainer.Children)
        {
            var container = (Container)x;
            container.Item.BackgroundColor = (Color)newValue;
        }
    }

    public bool ShouldAutoDismissKeyboard
    {
        get => (bool)GetValue(ShouldAutoDismissKeyboardProperty);
        set => SetValue(ShouldAutoDismissKeyboardProperty, value);
    }

    public static readonly BindableProperty ShouldAutoDismissKeyboardProperty =
      BindableProperty.Create(
          nameof(ShouldAutoDismissKeyboard),
          typeof(bool),
          typeof(FreakyCodeView),
          true,
          defaultBindingMode: BindingMode.OneWay);

    void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        this.Focus();
    }
}