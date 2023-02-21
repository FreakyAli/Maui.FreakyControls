using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Handlers;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace Maui.FreakyControls;

public partial class InputField : Grid
{
    internal const double FirstDash = 6;
    private View content;
    public virtual View Content
    {
        get => content;
        set
        {
            content = value;
            if (value != null)
            {
                border.Content = value;
                RegisterForEvents();
            }
        }
    }

    protected Label labelTitle = new Label()
    {
        Text = "Title",
        HorizontalOptions = LayoutOptions.Start,
        VerticalOptions = LayoutOptions.Start,
        InputTransparent = true,
        Margin = 15,
    };

    protected Border border = new Border
    {
        Padding = 0,
        StrokeThickness = 1,
        StrokeDashOffset = 0,
        BackgroundColor = Colors.Transparent,
    };

    protected Grid rootGrid = new Grid();

    protected Lazy<Image> imageIcon = new Lazy<Image>(() =>
    {
        return new Image
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 20,
            HeightRequest = 20,
            Margin = new Thickness(10, 0, 0, 0),
        };
    });

    protected HorizontalStackLayout endIconsContainer = new HorizontalStackLayout
    {
        HorizontalOptions = LayoutOptions.End,
        Margin = 5,
    };

    private Color LastFontimageColor;

    private bool hasValue;

    public virtual bool HasValue
    {
        get => hasValue;
        set
        {
            hasValue = value;
            UpdateState();
        }
    }

    public InputField()
    {
        border.StrokeShape = new RoundRectangle
        {
            CornerRadius = this.CornerRadius,
            Stroke = this.BorderColor,
        };

        RegisterForEvents();

        this.Add(border);
        this.Add(labelTitle);

        border.Content = rootGrid;

        rootGrid.AddColumnDefinition(new ColumnDefinition(GridLength.Auto));
        rootGrid.AddColumnDefinition(new ColumnDefinition(GridLength.Star));
        rootGrid.Add(Content, column: 1);

        rootGrid.Add(endIconsContainer, column: 2);

        labelTitle.Scale = 1;
        labelTitle.SetBinding(Label.TextProperty, new Binding(nameof(Title), source: this));

    }

    ~InputField()
    {
        ReleaseEvents();
    }

    protected override async void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        await Task.Delay(100);
        InitializeBorder();
    }

    // TODO: Remove this member hiding after android unfocus fixed.
    public new void Unfocus()
    {
        base.Unfocus();
#if ANDROID
    var view = Content.Handler.PlatformView as Android.Views.View;

    view?.ClearFocus();
#endif
    }

    private void InitializeBorder()
    {
        var perimeter = (this.Width + this.Height) * 2;
        var calculatedFirstDash = FirstDash + CornerRadius.Clamp(FirstDash, double.MaxValue);
        var space = (labelTitle.Width + calculatedFirstDash) * .8;


        border.StrokeDashArray = new DoubleCollection { calculatedFirstDash * 0.9, space, perimeter, 0 };

#if WINDOWS
    this.Add(border);
#endif

        UpdateState(animate: false);
        border.StrokeThickness = 1;
    }

    protected virtual void UpdateState(bool animate = true)
    {
        if (border.StrokeDashArray == null || border.StrokeDashArray.Count == 0 || labelTitle.Width <= 0)
        {
            return;
        }

        if (HasValue || Content.IsFocused)
        {
            UpdateOffset(0.01, animate);
            labelTitle.TranslateTo(0, -25, 90, Easing.BounceOut);
            labelTitle.AnchorX = 0;
            labelTitle.ScaleTo(.8, 90);
        }
        else
        {
            var offsetToGo = border.StrokeDashArray[0] + border.StrokeDashArray[1] + FirstDash;
            UpdateOffset(offsetToGo, animate);

            labelTitle.CancelAnimations();
            labelTitle.TranslateTo(imageIcon.IsValueCreated ? imageIcon.Value.Width : 0, 0, 90, Easing.BounceOut);
            labelTitle.AnchorX = 0;
            labelTitle.ScaleTo(1, 90);
        }
    }

    protected virtual void UpdateOffset(double value, bool animate = true)
    {
        if (!animate)
        {
            border.StrokeDashOffset = value;
        }
        else
        {
            border.Animate("borderOffset", new Animation((d) =>
            {
                border.StrokeDashOffset = d;
            }, border.StrokeDashOffset, value, Easing.BounceIn), 2, 90);
        }
    }

    protected virtual void RegisterForEvents()
    {
        if (Content != null)
        {
            Content.Focused -= Content_Focused;
            Content.Focused += Content_Focused;
            Content.Unfocused -= Content_Unfocused;
            Content.Unfocused += Content_Unfocused;
        }
    }

    protected virtual void ReleaseEvents()
    {
        Content.Focused -= Content_Focused;
        Content.Unfocused -= Content_Unfocused;
    }

    private void Content_Unfocused(object sender, FocusEventArgs e)
    {
        border.Stroke = BorderColor;
        labelTitle.TextColor = TitleColor;
        UpdateState();

        if (Icon is FontImageSource fontImageSource)
        {
            fontImageSource.Color = LastFontimageColor;
        }
    }

    private void Content_Focused(object sender, FocusEventArgs e)
    {
        border.Stroke = AccentColor;
        labelTitle.TextColor = AccentColor;
        UpdateState();

        if (Icon is FontImageSource fontImageSource && fontImageSource.Color != AccentColor)
        {
            LastFontimageColor = fontImageSource.Color?.WithAlpha(1); // To createnew instance.
            fontImageSource.Color = AccentColor;
        }
    }

    protected virtual void OnIconChanged()
    {
        imageIcon.Value.Source = Icon;

        //if (Icon is FontImageSource font && font.Color.IsNullOrTransparent())
        //{
        //    // TODO: Add IconColor bindable property.??? What if it's not FontImage?
        //    font.SetAppThemeColor(
        //        FontImageSource.ColorProperty,
        //        ColorResource.GetColor("OnBackground", Colors.Gray),
        //        ColorResource.GetColor("OnBackgroundDark", Colors.Gray));
        //}

        if (!rootGrid.Contains(imageIcon.Value))
        {
            rootGrid.Add(imageIcon.Value, column: 0);
        }

        var leftMargin = Icon != null ? 5 : 10;
        this.Content.Margin = new Thickness(leftMargin, 0, 0, 0);

    }
    protected virtual void OnCornerRadiusChanged()
    {
        if (border.StrokeShape is RoundRectangle roundRectangle)
        {
            roundRectangle.CornerRadius = CornerRadius;
        }
    }

    #region BindableProperties
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(InputField),
        string.Empty,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var textField = (bindable as InputField);
            textField.labelTitle.Text = (string)newValue;
            textField.InitializeBorder();
        });

    public Color AccentColor { get => (Color)GetValue(AccentColorProperty); set => SetValue(AccentColorProperty, value); }

    public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(
        nameof(AccentColor),
        typeof(Color),
        typeof(InputField),
        Colors.Purple,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as InputField).OnPropertyChanged(nameof(AccentColor)));

    public Color TitleColor { get => (Color)GetValue(TitleColorProperty); set => SetValue(TitleColorProperty, value); }

    public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(
        nameof(TitleColor),
        typeof(Color),
        typeof(InputField),
        Colors.Gray,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as InputField).labelTitle.TextColor = (Color)newValue
        );

    public Color BorderColor { get => (Color)GetValue(BorderColorProperty); set => SetValue(BorderColorProperty, value); }

    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
        nameof(BorderColor),
        typeof(Color),
        typeof(InputField),
        Colors.Gray,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as InputField).border.Stroke = (Color)newValue);

    public ImageSource Icon { get => (ImageSource)GetValue(IconProperty); set => SetValue(IconProperty, value); }

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon),
        typeof(ImageSource),
        typeof(InputField),
        propertyChanged: (bindable, oldValue, newValue) => (bindable as InputField).OnIconChanged());

    public double CornerRadius { get => (double)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
        nameof(CornerRadius),
        typeof(double),
        typeof(InputField),
        defaultValue: 8.0,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as InputField).OnCornerRadiusChanged());
    #endregion
}

public static class NumExtensions
{
    public static double Clamp(this double value, double min, double max)
    {
        if (value > max)
        {
            return max;
        }

        if (value < min)
        {
            return min;
        }

        return value;
    }
}

public class FreakyAutoCompleteTextField : InputField
{
    private bool _clearTapped;

    public FreakyAutoCompleteView AutoCompleteView => Content as FreakyAutoCompleteView;

    public override View Content { get; set; } = new FreakyAutoCompleteView
    {
        Margin = new Thickness(10, 0),
        BackgroundColor = Colors.Transparent,
        VerticalOptions = LayoutOptions.Center
    };

    protected ContentView iconClear = new ContentView
    {
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.End,
        IsVisible = false,
        Padding = 10,
        Content = new Path
        {
            Data = UraniumShapes.X,
            Fill =  Colors.DarkGray.WithAlpha(0.5f),
        }
    };

    public FreakyAutoCompleteTextField()
    {
        ItemsSource = new List<string>();

        iconClear.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(OnClearTapped)
        });

        AutoCompleteView.SetBinding(FreakyAutoCompleteView.TextProperty, new Binding(nameof(Text), source: this));
        AutoCompleteView.SetBinding(FreakyAutoCompleteView.SelectedTextProperty, new Binding(nameof(SelectedText), source: this));
        AutoCompleteView.SetBinding(FreakyAutoCompleteView.ItemsSourceProperty, new Binding(nameof(ItemsSource), source: this));

        AutoCompleteView.Focused += AutoCompleteTextField_Focused;
        this.Focused += AutoCompleteTextField_Focused;
    }

    private void AutoCompleteTextField_Focused(object sender, FocusEventArgs e)
    {
        Console.WriteLine("Focused");
    }

    public override bool HasValue => !string.IsNullOrEmpty(Text);

    

    protected override void OnHandlerChanged()
    {
        AutoCompleteView.TextChanged += AutoCompleteView_TextChanged;

        if (Handler is null)
        {
            AutoCompleteView.TextChanged -= AutoCompleteView_TextChanged;
        }
    }

    private void AutoCompleteView_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.OldTextValue) || string.IsNullOrEmpty(e.NewTextValue))
        {
            UpdateState();
        }

        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            //CheckAndShowValidations();
        }
        else
        {
            if (!_clearTapped)
            {
                SelectedText = string.Empty;
            }
            else
            {
                _clearTapped = false;
            }
        }

        TextChanged?.Invoke(this, e);
    }

    public event EventHandler<TextChangedEventArgs> TextChanged;


    protected virtual void OnClearTapped()
    {
        _clearTapped = true;
        AutoCompleteView.Text = string.Empty;
    }

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(FreakyAutoCompleteView),
        string.Empty,
        BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as FreakyAutoCompleteTextField).OnPropertyChanged(nameof(Text)));

    public string SelectedText { get => (string)GetValue(SelectedTextProperty); set => SetValue(SelectedTextProperty, value); }

    public static readonly BindableProperty SelectedTextProperty = BindableProperty.Create(
        nameof(SelectedText),
        typeof(string),
        typeof(FreakyAutoCompleteView),
        string.Empty,
        BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as FreakyAutoCompleteTextField).OnPropertyChanged(nameof(SelectedText)));

    public IList<string> ItemsSource { get => (IList<string>)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IList<string>),
            typeof(FreakyAutoCompleteView),
            null,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as FreakyAutoCompleteTextField).OnPropertyChanged(nameof(ItemsSource)));


    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(FreakyAutoCompleteView),
        Colors.DarkGray,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as FreakyAutoCompleteTextField).AutoCompleteView.TextColor = (Color)newValue);

}
public class FreakyAutoCompleteView : View
{
    public FreakyAutoCompleteView()
    {
        ItemsSource = new List<string>();
    }

    public event EventHandler<TextChangedEventArgs> TextChanged;

    public event EventHandler Completed;

    internal void TriggerCompleted()
    {
        Completed?.Invoke(this, EventArgs.Empty);

        if (ReturnCommand?.CanExecute(ReturnCommandParameter) ?? false)
        {
            ReturnCommand?.Execute(ReturnCommandParameter);
        }
    }

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(FreakyAutoCompleteView),
        string.Empty,
        BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is FreakyAutoCompleteView view)
            {
                view.TextChanged?.Invoke(view, new TextChangedEventArgs((string)oldValue, (string)newValue));
            }
        });

    public string SelectedText { get => (string)GetValue(SelectedTextProperty); set => SetValue(SelectedTextProperty, value); }

    public static readonly BindableProperty SelectedTextProperty = BindableProperty.Create(
        nameof(SelectedText),
        typeof(string),
        typeof(FreakyAutoCompleteView),
        string.Empty,
        BindingMode.TwoWay);

    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(FreakyAutoCompleteView),
        Colors.DarkGray);

    public IList<string> ItemsSource { get => (IList<string>)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IList<string>),
            typeof(FreakyAutoCompleteView),
            null);

    public ICommand ReturnCommand { get => (ICommand)GetValue(ReturnCommandProperty); set => SetValue(ReturnCommandProperty, value); }

    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(nameof(ReturnCommand),
            typeof(ICommand),
            typeof(FreakyAutoCompleteView),
            null);

    public object ReturnCommandParameter { get => GetValue(ReturnCommandParameterProperty); set => SetValue(ReturnCommandParameterProperty, value); }

    public static readonly BindableProperty ReturnCommandParameterProperty = BindableProperty.Create(nameof(ReturnCommandParameter),
            typeof(object),
            typeof(FreakyAutoCompleteView),
            null);
}

public interface IAutoCompleteView : IView
{
    string Text { get; set; }
    string SelectedText { get; set; }
    Color TextColor { get; set; }
    IList<string> ItemsSource { get; set; }

    void Completed();
}

public static class UraniumShapes
{
    public static Geometry ArrowRight = GeometryConverter.FromPath(Paths.ArrowRight);
    public static Geometry ExclamationCircle = GeometryConverter.FromPath(Paths.ExclamationCircle);
    public static Geometry X = GeometryConverter.FromPath(Paths.X);

    public static class Paths
    {
        public const string ArrowRight = "m 5.9697 2.9697 c 0.2929 -0.2929 0.7677 -0.2929 1.0606 0 l 6 6 c 0.2929 0.2929 0.2929 0.7677 0 1.0606 l -6 6 c -0.2929 0.2929 -0.7677 0.2929 -1.0606 0 c -0.2929 -0.2929 -0.2929 -0.7677 0 -1.0606 l 5.4696 -5.4697 l -5.4696 -5.4697 c -0.2929 -0.2929 -0.2929 -0.7678 0 -1.0607 z";
        public const string ExclamationCircle = "M 2.9835 16.4165 A 9.5 9.5 90 1 1 16.4165 2.9835 A 9.5 9.5 90 0 1 2.9835 16.4165 z M 8.75 4.95 v 5.7 h 1.9 V 4.95 H 8.75 z m 0 7.6 v 1.9 h 1.9 v -1.9 H 8.75 z";
        public const string X = "M 11.705 1.705 L 10.295 0.295 L 6 4.59 L 1.705 0.295 L 0.295 1.705 L 4.59 6 L 0.295 10.295 L 1.705 11.705 L 6 7.41 L 10.295 11.705 L 11.705 10.295 L 7.41 6 L 11.705 1.705 Z";
    }
}

public static class GeometryConverter
{
    private static PathGeometryConverter PathGeometryConverter { get; } = new PathGeometryConverter();
    public static Geometry FromPath(string path)
    {
        return (Geometry)PathGeometryConverter.ConvertFromInvariantString(path);
    }
}


