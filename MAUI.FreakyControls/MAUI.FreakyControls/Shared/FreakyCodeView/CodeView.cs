using Maui.FreakyControls.Shared.Enums;
using Microsoft.Maui.Controls.Shapes;

namespace Maui.FreakyControls;

internal class CodeView : Border
{
    public const double DefaultItemSize = 50.0;
    public const double DefaultDotSize = 20.0;
    public const double DefaultItemSpacing = 5.0;
    public const int DefaultCodeLength = 4;
    public static Color DefaultColor = Colors.Black;
    public static Color DefaultItemBackgroundColor = Colors.Transparent;

    private string inputChar;
    private Color color;
    private Color ItemBorderColor;

    public FocusAnimation FocusAnimationType { get; set; }
    public Color ItemFocusColor { get; set; }
    public Border Item => this;
    public Border Dot { get; }
    public Label CharLabel { get; }

    public CodeView()
    {
        Padding = 0;
        BackgroundColor = DefaultItemBackgroundColor;
        StrokeShape = new RoundRectangle()
        {
            CornerRadius = (float)DefaultItemSize / 2,
        };
        Stroke = DefaultColor;
        HeightRequest = WidthRequest = DefaultItemSize;
        VerticalOptions = LayoutOptions.Center;

        Dot = new Border()
        {
            BackgroundColor = DefaultColor,
            StrokeShape = new RoundRectangle()
            {
                CornerRadius = (float)DefaultItemSize / 2,
            },
            HeightRequest = DefaultDotSize,
            WidthRequest = DefaultDotSize,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Scale = 0,
            Padding = 0
        };

        CharLabel = new Label()
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = DefaultColor,
            FontAttributes = FontAttributes.Bold,
            VerticalTextAlignment = TextAlignment.Center,
            Scale = 0,
            TextTransform= TextTransform.Uppercase
        };

        Content = Dot;
    }

    private void Grow()
    {
        Content.ScaleTo(1.0, 100);
    }

    private void Shrink()
    {
        Content.ScaleTo(0, 100);
    }

    public void SetColor(Color color, Color ItemBorderColor)
    {
        this.color = color;
        this.ItemBorderColor = ItemBorderColor;
        SetBorderColor();
        Dot.BackgroundColor = color;
        CharLabel.TextColor = color;
    }

    public void SetRadius(ItemShape shapeType)
    {
        if (shapeType == ItemShape.Circle)
        {
            this.StrokeShape = new RoundRectangle() { CornerRadius = (float)HeightRequest / 2 };
        }
        else if (shapeType == ItemShape.Square)
        {
            this.StrokeShape = new RoundRectangle() { CornerRadius = 0 };
        }
        else if (shapeType == ItemShape.Squircle)
        {
            this.StrokeShape = new RoundRectangle() { CornerRadius = 10 };
        }
    }

    public void SecureMode(bool isPassword)
    {
        if (isPassword)
        {
            Content = Dot;
        }
        else
        {
            Content = CharLabel;
        }

        if (!string.IsNullOrEmpty(inputChar))
        {
            Grow();
        }
        else
        {
            Shrink();
        }
    }

    public void ClearValueWithAnimation()
    {
        inputChar = null;
        Shrink();
    }


    public void SetValueWithAnimation(char inputChar)
    {
        UnfocusAnimate();
        CharLabel.Text = inputChar.ToString();
        this.inputChar = inputChar.ToString();
        Grow();
    }

    public async void FocusAnimate()
    {
        Stroke = ItemFocusColor;

        if (FocusAnimationType == FocusAnimation.Bounce)
        {
            await this.ScaleTo(1.2, 100);
            await this.ScaleTo(1, 100);
        }
        else if (FocusAnimationType == FocusAnimation.Scale)
        {
            await this.ScaleTo(1.2, 100);
        }
    }

    public void UnfocusAnimate()
    {
        SetBorderColor();
        this.ScaleTo(1, 100);
    }

    private void SetBorderColor()
    {
        Stroke = ItemBorderColor == Colors.Black ? color : ItemBorderColor;
    }
}