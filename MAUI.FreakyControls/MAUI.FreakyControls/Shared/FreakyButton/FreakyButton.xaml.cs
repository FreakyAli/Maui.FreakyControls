using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.TouchPress;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maui.FreakyControls;

public partial class MaterialButton : Grid, ITouchAndPressEffectConsumer
{
    #region Bindable properties

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MaterialButton), defaultValue: null);

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(MaterialButton), defaultValue: null);

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public new static readonly BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(MaterialButton), defaultValue: true);

    public new bool IsEnabled
    {
        get { return (bool)GetValue(IsEnabledProperty); }
        set { SetValue(IsEnabledProperty, value); }
    }

    public static readonly BindableProperty AnimationProperty =
        BindableProperty.Create(nameof(Animation), typeof(AnimationTypes), typeof(MaterialButton), defaultValue: AnimationTypes.Fade);

    public AnimationTypes Animation
    {
        get { return (AnimationTypes)GetValue(AnimationProperty); }
        set { SetValue(AnimationProperty, value); }
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(MaterialButton), defaultValue: null);

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(MaterialButton), new CornerRadius(10));

    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly BindableProperty IsBusyProperty =
        BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(MaterialButton), defaultValue: false);

    public bool IsBusy
    {
        get { return (bool)GetValue(IsBusyProperty); }
        set { SetValue(IsBusyProperty, value); }
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(MaterialButton), defaultValue: Colors.White);

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    public new static readonly BindableProperty BackgroundColorProperty =
       BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(MaterialButton), defaultValue: Colors.Black);

    public new Color BackgroundColor
    {
        get { return (Color)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }

    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(MaterialButton), defaultValue: Colors.White);

    public Color BorderColor
    {
        get { return (Color)GetValue(BorderColorProperty); }
        set { SetValue(BorderColorProperty, value); }
    }

    public static readonly BindableProperty BusyColorProperty =
        BindableProperty.Create(nameof(BusyColor), typeof(Color), typeof(MaterialButton), defaultValue: Colors.White);

    public Color BusyColor
    {
        get { return (Color)GetValue(BusyColorProperty); }
        set { SetValue(BusyColorProperty, value); }
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(MaterialButton), defaultValue: Microsoft.Maui.Font.Default.Size);

    public double FontSize
    {
        get { return (double)GetValue(FontSizeProperty); }
        set { SetValue(FontSizeProperty, value); }
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(MaterialButton), defaultValue: null);

    public string FontFamily
    {
        get { return (string)GetValue(FontFamilyProperty); }
        set { SetValue(FontFamilyProperty, value); }
    }

    public static readonly BindableProperty LeadingIconProperty =
        BindableProperty.Create(nameof(LeadingIcon), typeof(View), typeof(MaterialButton), defaultValue: null, propertyChanged: OnLeadingIconChanged);

    private static void OnLeadingIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var freakyButton = bindable as MaterialButton;
        freakyButton.leadingContentView.Content = newValue as View;
    }

    public View LeadingIcon
    {
        get { return (View)GetValue(LeadingIconProperty); }
        set { SetValue(LeadingIconProperty, value); }
    }

    public static readonly BindableProperty TrailingIconProperty =
        BindableProperty.Create(nameof(TrailingIcon), typeof(View), typeof(MaterialButton), defaultValue: null, propertyChanged: OnTrailingIconChanged);

    private static void OnTrailingIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var freakyButton = bindable as MaterialButton;
        freakyButton.trailingContentView.Content = newValue as View;
    }

    public View TrailingIcon
    {
        get { return (View)GetValue(TrailingIconProperty); }
        set { SetValue(TrailingIconProperty, value); }
    }

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(nameof(IconSize), typeof(double), typeof(MaterialButton), defaultValue: 24.0);

    public double IconSize
    {
        get { return (double)GetValue(IconSizeProperty); }
        set { SetValue(IconSizeProperty, value); }
    }

    public static readonly BindableProperty ActivityIndicatorSizeProperty =
        BindableProperty.Create(nameof(ActivityIndicatorSize), typeof(double), typeof(MaterialButton), defaultValue: 30.0);

    public double ActivityIndicatorSize
    {
        get { return (double)GetValue(ActivityIndicatorSizeProperty); }
        set { SetValue(ActivityIndicatorSizeProperty, value); }
    }

    public new static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(MaterialButton), defaultValue: new Thickness(12, 0));

    public new Thickness Padding
    {
        get { return (Thickness)GetValue(PaddingProperty); }
        set { SetValue(PaddingProperty, value); }
    }

    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(nameof(Spacing), typeof(int), typeof(MaterialButton), defaultValue: 12);

    public int Spacing
    {
        get { return (int)GetValue(SpacingProperty); }
        set { SetValue(SpacingProperty, value); }
    }

    public event EventHandler Clicked;

    #endregion Bindable properties

    #region Constructors

    public MaterialButton()
    {
        InitializeComponent();
        this.Effects.Add(new TouchAndPressEffect());
    }

    #endregion Constructors

    #region Methods

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(IsEnabled))
            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled");
    }

    public void ConsumeEvent(EventType gestureType)
    {
        if (IsEnabled)
            TouchAndPressAnimation.Animate(this, gestureType);
    }

    public void ExecuteAction()
    {
        if (IsEnabled)
            Command?.ExecuteCommandIfAvailable(CommandParameter);

        if (IsEnabled && Clicked != null)
            Clicked.Invoke(this, EventArgs.Empty);
    }

    #endregion Methods
}