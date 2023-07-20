using System.Runtime.CompilerServices;
using Maui.FreakyControls.Shared.TouchPress;
using Font = Microsoft.Maui.Font;

namespace Maui.FreakyControls;

public partial class MaterialSlider : ContentView
{
    #region Constructors

    public MaterialSlider()
    {
        InitializeComponent();
    }

    #endregion Constructors

    #region Attributes

    public double OldValue;

    #endregion Attributes

    #region Properties

    public event EventHandler<ValueChangedEventArgs> ValueChanged;

    #region LabelText

    public static readonly BindableProperty LabelTextProperty =
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(MaterialSlider));

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public static readonly BindableProperty LabelTextColorProperty =
        BindableProperty.Create(nameof(LabelTextColor), typeof(Color), typeof(MaterialSlider), Colors.Gray);

    public Color LabelTextColor
    {
        get => (Color)GetValue(LabelTextColorProperty);
        set => SetValue(LabelTextColorProperty, value);
    }

    public static readonly BindableProperty DisabledLabelTextColorProperty =
        BindableProperty.Create(nameof(DisabledLabelTextColor), typeof(Color), typeof(MaterialSlider), Colors.Gray);

    public Color DisabledLabelTextColor
    {
        get => (Color)GetValue(DisabledLabelTextColorProperty);
        set => SetValue(DisabledLabelTextColorProperty, value);
    }

    public static readonly BindableProperty LabelSizeProperty =
        BindableProperty.Create(nameof(LabelSize), typeof(double), typeof(MaterialSlider), Font.Default.Size);

    public double LabelSize
    {
        get => (double)GetValue(LabelSizeProperty);
        set => SetValue(LabelSizeProperty, value);
    }

    #endregion LabelText

    #region LabelValue

    public static readonly BindableProperty LabelValueFormatProperty =
        BindableProperty.Create(nameof(LabelValueFormat), typeof(string), typeof(MaterialSlider));

    public string LabelValueFormat
    {
        get => (string)GetValue(LabelValueFormatProperty);
        set => SetValue(LabelValueFormatProperty, value);
    }

    public static readonly BindableProperty LabelValueColorProperty =
        BindableProperty.Create(nameof(LabelValueColor), typeof(Color), typeof(MaterialSlider), Colors.Gray);

    public Color LabelValueColor
    {
        get => (Color)GetValue(LabelValueColorProperty);
        set => SetValue(LabelValueColorProperty, value);
    }

    public static readonly BindableProperty DisabledLabelValueColorProperty =
        BindableProperty.Create(nameof(DisabledLabelValueColor), typeof(Color), typeof(MaterialSlider), Colors.Gray);

    public Color DisabledLabelValueColor
    {
        get => (Color)GetValue(DisabledLabelValueColorProperty);
        set => SetValue(DisabledLabelValueColorProperty, value);
    }

    public static readonly BindableProperty LabelValueSizeProperty =
        BindableProperty.Create(nameof(LabelValueSize), typeof(double), typeof(MaterialSlider), Font.Default.Size);

    public double LabelValueSize
    {
        get => (double)GetValue(LabelValueSizeProperty);
        set => SetValue(LabelValueSizeProperty, value);
    }

    public static readonly BindableProperty LabelValueIsVisibleProperty =
        BindableProperty.Create(nameof(LabelValueIsVisible), typeof(bool), typeof(MaterialSlider), false);

    public bool LabelValueIsVisible
    {
        get => (bool)GetValue(LabelValueIsVisibleProperty);
        set => SetValue(LabelValueIsVisibleProperty, value);
    }

    #endregion LabelValue

    #region LabelMinimum

    public static readonly BindableProperty LabelMinimumTextProperty =
        BindableProperty.Create(nameof(LabelMinimumText), typeof(string), typeof(MaterialSlider));

    public string LabelMinimumText
    {
        get => (string)GetValue(LabelMinimumTextProperty);
        set => SetValue(LabelMinimumTextProperty, value);
    }

    public static readonly BindableProperty LabelMinimumTextColorProperty =
        BindableProperty.Create(nameof(LabelMinimumTextColor), typeof(Color), typeof(MaterialSlider), Colors.Gray);

    public Color LabelMinimumTextColor
    {
        get => (Color)GetValue(LabelMinimumTextColorProperty);
        set => SetValue(LabelMinimumTextColorProperty, value);
    }

    public static readonly BindableProperty DisabledLabelMinimumTextColorProperty =
        BindableProperty.Create(nameof(DisabledLabelMinimumTextColor), typeof(Color), typeof(MaterialSlider),
            Colors.Gray);

    public Color DisabledLabelMinimumTextColor
    {
        get => (Color)GetValue(DisabledLabelMinimumTextColorProperty);
        set => SetValue(DisabledLabelMinimumTextColorProperty, value);
    }

    public static readonly BindableProperty LabelMinimumSizeProperty =
        BindableProperty.Create(nameof(LabelMinimumSize), typeof(double), typeof(MaterialSlider), Font.Default.Size);

    public double LabelMinimumSize
    {
        get => (double)GetValue(LabelMinimumSizeProperty);
        set => SetValue(LabelMinimumSizeProperty, value);
    }

    #endregion LabelMinimum

    #region LabelMaximum

    public static readonly BindableProperty LabelMaximumTextProperty =
        BindableProperty.Create(nameof(LabelMaximumText), typeof(string), typeof(MaterialSlider));

    public string LabelMaximumText
    {
        get => (string)GetValue(LabelMaximumTextProperty);
        set => SetValue(LabelMaximumTextProperty, value);
    }

    public static readonly BindableProperty LabelMaximumTextColorProperty =
        BindableProperty.Create(nameof(LabelMaximumTextColor), typeof(Color), typeof(MaterialSlider), Colors.Gray);

    public Color LabelMaximumTextColor
    {
        get => (Color)GetValue(LabelMaximumTextColorProperty);
        set => SetValue(LabelMaximumTextColorProperty, value);
    }

    public static readonly BindableProperty DisabledLabelMaximumTextColorProperty =
        BindableProperty.Create(nameof(DisabledLabelMaximumTextColor), typeof(Color), typeof(MaterialSlider),
            Colors.Gray);

    public Color DisabledLabelMaximumTextColor
    {
        get => (Color)GetValue(DisabledLabelMaximumTextColorProperty);
        set => SetValue(DisabledLabelMaximumTextColorProperty, value);
    }

    public static readonly BindableProperty LabelMaximumSizeProperty =
        BindableProperty.Create(nameof(LabelMaximumSize), typeof(double), typeof(MaterialSlider), Font.Default.Size);

    public double LabelMaximumSize
    {
        get => (double)GetValue(LabelMaximumSizeProperty);
        set => SetValue(LabelMaximumSizeProperty, value);
    }

    #endregion LabelMaximum

    #region AssistiveText

    public static readonly BindableProperty AssistiveTextProperty =
        BindableProperty.Create(nameof(AssistiveText), typeof(string), typeof(MaterialSlider),
            validateValue: OnAssistiveTextValidate);

    public string AssistiveText
    {
        get => (string)GetValue(AssistiveTextProperty);
        set => SetValue(AssistiveTextProperty, value);
    }

    public static readonly BindableProperty AssistiveTextColorProperty =
        BindableProperty.Create(nameof(AssistiveTextColor), typeof(Color), typeof(MaterialSlider), Colors.Gray);

    public Color AssistiveTextColor
    {
        get => (Color)GetValue(AssistiveTextColorProperty);
        set => SetValue(AssistiveTextColorProperty, value);
    }

    public static readonly BindableProperty AssistiveSizeProperty =
        BindableProperty.Create(nameof(AssistiveSize), typeof(double), typeof(MaterialSlider), Font.Default.Size);

    public double AssistiveSize
    {
        get => (double)GetValue(AssistiveSizeProperty);
        set => SetValue(AssistiveSizeProperty, value);
    }

    public static readonly BindableProperty AnimateErrorProperty =
        BindableProperty.Create(nameof(AnimateError), typeof(bool), typeof(MaterialSlider), false);

    public bool AnimateError
    {
        get => (bool)GetValue(AnimateErrorProperty);
        set => SetValue(AnimateErrorProperty, value);
    }

    #endregion AssistiveText

    #region ImageMinimum

    public static readonly BindableProperty MinimumIconProperty =
        BindableProperty.Create(nameof(MinimumIcon), typeof(string), typeof(MaterialSlider));

    public string MinimumIcon
    {
        get => (string)GetValue(MinimumIconProperty);
        set => SetValue(MinimumIconProperty, value);
    }

    public static readonly BindableProperty CustomMinimumIconProperty =
        BindableProperty.Create(nameof(CustomMinimumIcon), typeof(View), typeof(MaterialSlider));

    public View CustomMinimumIcon
    {
        get => (View)GetValue(CustomMinimumIconProperty);
        set => SetValue(CustomMinimumIconProperty, value);
    }

    public bool MinimumIconIsVisible => !string.IsNullOrEmpty(MinimumIcon) || CustomMinimumIcon != null;

    #endregion ImageMinimum

    #region ImageMaximum

    public static readonly BindableProperty MaximumIconProperty =
        BindableProperty.Create(nameof(MaximumIcon), typeof(string), typeof(MaterialSlider));

    public string MaximumIcon
    {
        get => (string)GetValue(MaximumIconProperty);
        set => SetValue(MaximumIconProperty, value);
    }

    public static readonly BindableProperty CustomMaximumIconProperty =
        BindableProperty.Create(nameof(CustomMaximumIcon), typeof(View), typeof(MaterialSlider));

    public View CustomMaximumIcon
    {
        get => (View)GetValue(CustomMaximumIconProperty);
        set => SetValue(CustomMaximumIconProperty, value);
    }

    public bool MaximumIconIsVisible => !string.IsNullOrEmpty(MaximumIcon) || CustomMaximumIcon != null;

    #endregion ImageMaximum

    #region BackGroundImage

    public static readonly BindableProperty BackgroundImageProperty =
        BindableProperty.Create(nameof(BackgroundImage), typeof(string), typeof(MaterialSlider));

    public string BackgroundImage
    {
        get => (string)GetValue(BackgroundImageProperty);
        set => SetValue(BackgroundImageProperty, value);
    }

    public static readonly BindableProperty CustomBackgroundImageProperty =
        BindableProperty.Create(nameof(CustomBackgroundImage), typeof(View), typeof(MaterialSlider));

    public View CustomBackgroundImage
    {
        get => (View)GetValue(CustomBackgroundImageProperty);
        set => SetValue(CustomBackgroundImageProperty, value);
    }

    #endregion BackGroundImage

    #region ThumbImage

    public static readonly BindableProperty ThumbImageProperty =
        BindableProperty.Create(nameof(ThumbImage), typeof(string), typeof(MaterialSlider));

    public string ThumbImage
    {
        get => (string)GetValue(ThumbImageProperty);
        set => SetValue(ThumbImageProperty, value);
    }

    #endregion ThumbImage

    #region Values

    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(double), typeof(MaterialSlider), 0.0, BindingMode.TwoWay,
            propertyChanged: OnValuePropertyChanged);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty MinimumValueProperty =
        BindableProperty.Create(nameof(MinimumValue), typeof(double), typeof(MaterialSlider), 0.0);

    public double MinimumValue
    {
        get => (double)GetValue(MinimumValueProperty);
        set => SetValue(MinimumValueProperty, value);
    }

    public static readonly BindableProperty MaximumValueProperty =
        BindableProperty.Create(nameof(MaximumValue), typeof(double), typeof(MaterialSlider), 1.0);

    public double MaximumValue
    {
        get => (double)GetValue(MaximumValueProperty);
        set => SetValue(MaximumValueProperty, value);
    }

    #endregion Values

    public static readonly BindableProperty ActiveTrackColorProperty =
        BindableProperty.Create(nameof(ActiveTrackColor), typeof(Color), typeof(MaterialSlider), Colors.Black);

    public Color ActiveTrackColor
    {
        get => (Color)GetValue(ActiveTrackColorProperty);
        set => SetValue(ActiveTrackColorProperty, value);
    }

    public static readonly BindableProperty InactiveTrackColorProperty =
        BindableProperty.Create(nameof(InactiveTrackColor), typeof(Color), typeof(MaterialSlider), Colors.Gray);

    public Color InactiveTrackColor
    {
        get => (Color)GetValue(InactiveTrackColorProperty);
        set => SetValue(InactiveTrackColorProperty, value);
    }

    public static readonly BindableProperty ThumbColorProperty =
        BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(MaterialSlider), Colors.Black);

    public Color ThumbColor
    {
        get => (Color)GetValue(ThumbColorProperty);
        set => SetValue(ThumbColorProperty, value);
    }

    public static readonly BindableProperty TrackHeightProperty =
        BindableProperty.Create(nameof(TrackHeight), typeof(int), typeof(MaterialSlider), 6);

    public int TrackHeight
    {
        get => (int)GetValue(TrackHeightProperty);
        set => SetValue(TrackHeightProperty, value);
    }

    public static readonly BindableProperty TrackCornerRadiusProperty =
        BindableProperty.Create(nameof(TrackCornerRadius), typeof(int), typeof(MaterialSlider), 3);

    public int TrackCornerRadius
    {
        get => (int)GetValue(TrackCornerRadiusProperty);
        set => SetValue(TrackCornerRadiusProperty, value);
    }

    public static readonly BindableProperty ShowIconsProperty =
        BindableProperty.Create(nameof(ShowIcons), typeof(bool), typeof(MaterialSlider), false);

    public bool ShowIcons
    {
        get => (bool)GetValue(ShowIconsProperty);
        set => SetValue(ShowIconsProperty, value);
    }

    public static readonly BindableProperty DisabledActiveTrackColorProperty =
        BindableProperty.Create(nameof(DisabledActiveTrackColor), typeof(Color), typeof(MaterialSlider), Colors.Gray);

    public Color DisabledActiveTrackColor
    {
        get => (Color)GetValue(DisabledActiveTrackColorProperty);
        set => SetValue(DisabledActiveTrackColorProperty, value);
    }

    public static readonly BindableProperty DisabledInactiveTrackColorProperty =
        BindableProperty.Create(nameof(DisabledInactiveTrackColor), typeof(Color), typeof(MaterialSlider),
            Colors.LightGray);

    public Color DisabledInactiveTrackColor
    {
        get => (Color)GetValue(DisabledInactiveTrackColorProperty);
        set => SetValue(DisabledInactiveTrackColorProperty, value);
    }

    public static readonly BindableProperty DisabledThumbColorProperty =
        BindableProperty.Create(nameof(DisabledThumbColor), typeof(Color), typeof(MaterialSlider), Colors.Gray);

    public Color DisabledThumbColor
    {
        get => (Color)GetValue(DisabledThumbColorProperty);
        set => SetValue(DisabledThumbColorProperty, value);
    }

    #endregion Properties

    #region Methods

    public void ExecuteChanged()
    {
        var args = new ValueChangedEventArgs(OldValue, Value);
        ValueChanged?.Invoke(this, args);
    }

    private static bool OnAssistiveTextValidate(BindableObject bindable, object value)
    {
        var control = (MaterialSlider)bindable;
        // Used to animate the error when the assistive text doesn't change
        if (control.AnimateError && !string.IsNullOrEmpty(control.AssistiveText) &&
            control.AssistiveText == (string)value)
        {
            ShakeAnimation.Animate(control);
        }
        return true;
    }

    private void OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        Value = e.NewValue;
        OldValue = e.OldValue;

        //Todo Take a look at this
       // if (LabelValueIsVisible)
            //lblValue.Text = Value.ToString(LabelValueFormat);

        ExecuteChanged();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        switch (propertyName)
        {
            case nameof(LabelText):
                //Todo Use a converter
                //lblLabel.IsVisible = !string.IsNullOrEmpty(LabelText);
                break;

            case nameof(DisabledLabelTextColor):
                //Todo Fix this or remove disabled states
                //if (IsEnabled)
                //    lblLabel.TextColor = LabelTextColor;
                //else
                //    lblLabel.TextColor = DisabledLabelTextColor;
                break;

            case nameof(LabelValueFormat):
                //Todo Create an OnPropertyChanged method to handle this
               // lblValue.Text = Value.ToString(LabelValueFormat);
                break;

            case nameof(DisabledLabelValueColor):
                //Todo Fix this or remove it
                //lblValue.TextColor = IsEnabled ? LabelValueColor : DisabledLabelValueColor;
                break;

            case nameof(LabelValueSize):
                //lblValue.FontSize = LabelValueSize;
                break;

            case nameof(LabelValueIsVisible):
                //Todo onpropertychanged
                //lblValue.Text = Value.ToString(LabelValueFormat);
                break;

            case nameof(LabelMinimumText):
                lblMinimum.Text = LabelMinimumText;
                lblMinimum.IsVisible = !string.IsNullOrEmpty(LabelMinimumText) && (!ShowIcons || !MinimumIconIsVisible);
                break;

            case nameof(LabelMinimumTextColor):
                lblMinimum.TextColor = LabelMinimumTextColor;
                break;

            case nameof(DisabledLabelMinimumTextColor):
                if (IsEnabled)
                    lblMinimum.TextColor = LabelMinimumTextColor;
                else
                    lblMinimum.TextColor = DisabledLabelMinimumTextColor;
                break;

            case nameof(LabelMinimumSize):
                lblMinimum.FontSize = LabelMinimumSize;
                break;

            case nameof(LabelMaximumText):
                lblMaximum.Text = LabelMaximumText;
                lblMaximum.IsVisible = !string.IsNullOrEmpty(LabelMaximumText) && (!ShowIcons || !MaximumIconIsVisible);
                break;

            case nameof(LabelMaximumTextColor):
                lblMaximum.TextColor = LabelMaximumTextColor;
                break;

            case nameof(DisabledLabelMaximumTextColor):
                if (IsEnabled)
                    lblMaximum.TextColor = LabelMaximumTextColor;
                else
                    lblMaximum.TextColor = DisabledLabelMaximumTextColor;
                break;

            case nameof(LabelMaximumSize):
                lblMaximum.FontSize = LabelMaximumSize;
                break;

            case nameof(AssistiveText):
                lblAssistive.Text = AssistiveText;
                lblAssistive.IsVisible = !string.IsNullOrEmpty(AssistiveText);
                if (AnimateError && !string.IsNullOrEmpty(AssistiveText))
                {
                    // Todo: Fix this crap
                    // ShakeAnimation.Animate(this);
                }

                break;

            case nameof(AssistiveTextColor):
                lblAssistive.TextColor = AssistiveTextColor;
                break;

            case nameof(AssistiveSize):
                lblAssistive.FontSize = AssistiveSize;
                break;

            case nameof(MinimumIcon):
                if (!string.IsNullOrEmpty(MinimumIcon))
                    imgMinimum.SetImage(MinimumIcon);

                SetMinimumIconIsVisible();
                break;

            case nameof(CustomMinimumIcon):
                if (CustomMinimumIcon != null) imgMinimum.SetCustomImage(CustomMinimumIcon);

                SetMinimumIconIsVisible();
                break;

            case nameof(MaximumIcon):
                if (!string.IsNullOrEmpty(MaximumIcon)) imgMaximum.SetImage(MaximumIcon);

                SetMaximumIconIsVisible();
                break;

            case nameof(CustomMaximumIcon):
                if (CustomMaximumIcon != null) imgMaximum.SetCustomImage(CustomMaximumIcon);

                SetMaximumIconIsVisible();
                break;

            case nameof(BackgroundImage):
                if (!string.IsNullOrEmpty(BackgroundImage))
                {
                    bckgImage.SetImage(BackgroundImage);
                    bckgImage.IsVisible = true;
                }
                else
                {
                    bckgImage.IsVisible = false;
                }

                break;

            case nameof(CustomBackgroundImage):
                if (CustomBackgroundImage != null)
                {
                    bckgImage.SetCustomImage(CustomBackgroundImage);
                    bckgImage.IsVisible = true;
                }
                else
                {
                    bckgImage.IsVisible = false;
                }

                break;

            case nameof(ThumbImage):
                if (!string.IsNullOrEmpty(ThumbImage)) slider.ThumbImageSource = ThumbImage;
                break;

            case nameof(MinimumValue):
                slider.Minimum = MinimumValue;
                break;

            case nameof(MaximumValue):
                slider.Maximum = MaximumValue;
                break;

            case nameof(ActiveTrackColor):
                slider.ActiveTrackColor = ActiveTrackColor;
                break;

            case nameof(InactiveTrackColor):
                slider.InactiveTrackColor = InactiveTrackColor;
                break;

            case nameof(ThumbColor):
                slider.ThumbColor = ThumbColor;
                break;

            case nameof(TrackHeight):
                slider.TrackHeight = TrackHeight;
                break;

            case nameof(TrackCornerRadius):
                slider.TrackCornerRadius = TrackCornerRadius;
                break;

            case nameof(IsEnabled):
                IsEnabled = IsEnabled;
                SetEnable();
                break;

            case nameof(ShowIcons):
                SetMinimumIconIsVisible();
                SetMaximumIconIsVisible();
                break;
        }

        base.OnPropertyChanged(propertyName);
    }

    private void SetMinimumIconIsVisible()
    {
        imgMinimum.IsVisible = ShowIcons && MinimumIconIsVisible;
    }

    private void SetMaximumIconIsVisible()
    {
        imgMaximum.IsVisible = ShowIcons && MaximumIconIsVisible;
    }

    public static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MaterialSlider control && newValue != null && newValue is double value)
        {
            if (value >= control.MinimumValue && value <= control.MaximumValue)
                control.slider.Value = value;
            else
                control.Value = control.MinimumValue;
        }
    }

    public void SetEnable()
    {
        if (!IsEnabled)
        {
            //lblLabel.TextColor = DisabledLabelTextColor;
            //lblValue.TextColor = DisabledLabelValueColor;
            lblMinimum.TextColor = DisabledLabelMinimumTextColor;
            lblMaximum.TextColor = DisabledLabelMaximumTextColor;

            slider.ActiveTrackColor = DisabledActiveTrackColor;
            slider.InactiveTrackColor = DisabledInactiveTrackColor;
            slider.ThumbColor = DisabledThumbColor;
        }
        else
        {
            //lblLabel.TextColor = LabelTextColor;
            //lblValue.TextColor = LabelValueColor;
            lblMinimum.TextColor = LabelMinimumTextColor;
            lblMaximum.TextColor = LabelMaximumTextColor;

            slider.ActiveTrackColor = ActiveTrackColor;
            slider.InactiveTrackColor = InactiveTrackColor;
            slider.ThumbColor = ThumbColor;
        }
    }

    #endregion Methods
}