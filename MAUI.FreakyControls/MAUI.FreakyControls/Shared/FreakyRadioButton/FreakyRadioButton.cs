using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Shared.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Windows.Input;

namespace Maui.FreakyControls
{
    public class FreakyRadioButton : ContentView, IDisposable
    {
        #region Fields

        private const string defaultName = "RadioButton";
        private static readonly float outlineWidth = 6.0f;
        private static readonly double size = 24.0;
        private readonly SKCanvasView skiaView;
        private readonly TapGestureRecognizer tapped = new();
        private bool isAnimating;

        #endregion Fields

        public FreakyRadioButton()
        {
            skiaView = new SKCanvasView();
            WidthRequest = HeightRequest = skiaView.WidthRequest = skiaView.HeightRequest = size;
            HorizontalOptions = VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false);
            Content = skiaView;

            skiaView.PaintSurface += Handle_PaintSurface;
            tapped.Tapped += Radiobutton_Tapped;
            GestureRecognizers.Add(tapped);
        }

        public void Dispose()
        {
            tapped.Tapped -= Radiobutton_Tapped;
            GestureRecognizers.Clear();
            skiaView.PaintSurface -= Handle_PaintSurface;
        }

        protected override void ChangeVisualState()
        {
            ApplyIsCheckedState();
            base.ChangeVisualState();
        }

        private void ApplyIsCheckedState()
        {
            if (IsChecked)
            {
                VisualStateManager.GoToState(this, RadioButton.CheckedVisualState);
            }
            else
            {
                VisualStateManager.GoToState(this, RadioButton.UncheckedVisualState);
            }
        }

        /// <summary>
        /// Method to create a checked version of the <see cref="FreakyRadioButton"/>
        /// This method creates a filled radiobutton that that uses two SKPaint objects to avoid design flaws when creating a filled radiobutton
        /// </summary>
        /// <param name="e"></param>
        private void DrawCheckFilled(SKPaintSurfaceEventArgs e)
        {
            var imageInfo = e.Info;
            var canvas = e?.Surface?.Canvas;

            using (var checkfill = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = FillColor.ToSKColor(),
                StrokeWidth = OutlineWidth,
                StrokeJoin = SKStrokeJoin.Round,
                IsAntialias = true,
            })
            {
                canvas.DrawCircle(
                    imageInfo.Width / 2,
                    imageInfo.Height / 2,
                    (imageInfo.Width / 2) - (OutlineWidth / 2),
                    checkfill);
            }

            using (var fillPaint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = this.OutlineColor.ToSKColor(),
                StrokeWidth = OutlineWidth,
                StrokeJoin = SKStrokeJoin.Round,
                IsAntialias = true,
            })
            {
                canvas.DrawCircle(
                    imageInfo.Width / 2,
                    imageInfo.Height / 2,
                    (imageInfo.Width / 2) - (OutlineWidth / 2),
                    fillPaint);
            }

            using var checkPath = new SKPath();
            checkPath.DrawNativeRadioButtonCheck(imageInfo, ((imageInfo.Width / 2) - (OutlineWidth / 2)) / 1.75);

            using var checkStroke = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = CheckColor.ToSKColor(),
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round
            };

            canvas.DrawPath(checkPath, checkStroke);
        }

        /// <summary>
        /// Method to create a unchecked version of the <see cref="FreakyRadioButton"/>
        /// </summary>
        /// <param name="e"></param>
        private void DrawOutline(SKPaintSurfaceEventArgs e)
        {
            var imageInfo = e.Info;
            var canvas = e?.Surface?.Canvas;

            using (var outline = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = OutlineColor.ToSKColor(),
                StrokeWidth = OutlineWidth,
                StrokeJoin = SKStrokeJoin.Round,
                IsAntialias = true
            })
            {
                canvas.DrawCircle(
                    imageInfo.Width / 2,
                    imageInfo.Height / 2,
                    (imageInfo.Width / 2) - (OutlineWidth / 2),
                    outline);
            }
        }

        private void Handle_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            e?.Surface?.Canvas?.Clear();
            DrawOutline(e);
            if (IsChecked)
                DrawCheckFilled(e);
        }

        private void Radiobutton_Tapped(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                if (isAnimating)
                    return;
                if (!IsChecked)
                    IsChecked = true;
            }
        }

        private async Task ToggleAnimationAsync()
        {
            isAnimating = true;
            await skiaView.ScaleTo(0.80, 100);
            skiaView.InvalidateSurface();
            await skiaView.ScaleTo(1, 100, Easing.BounceOut);
            isAnimating = false;
        }

        #region Events

        /// <summary>
        /// Raised when <see cref="FreakyRadioButton.IsChecked"/> changes.
        /// </summary>
        public event EventHandler<CheckedChangedEventArgs> CheckedChanged;

        #endregion Events

        #region Bindable Properties

        public static readonly BindableProperty CheckColorProperty =
        BindableProperty.Create(
            nameof(CheckColor),
            typeof(Color),
            typeof(FreakyRadioButton),
            Colors.Black);

        public static readonly BindableProperty CheckedChangedCommandProperty =
        BindableProperty.Create(
            nameof(CheckedChangedCommand),
            typeof(ICommand),
            typeof(FreakyRadioButton));

        public static readonly BindableProperty FillColorProperty =
        BindableProperty.Create(
            nameof(FillColor),
            typeof(Color),
            typeof(FreakyRadioButton),
            Colors.White);

        public static readonly BindableProperty HasCheckAnimationProperty =
                                BindableProperty.Create(
            nameof(HasCheckAnimation),
            typeof(bool),
            typeof(FreakyRadioButton),
            true);

        public static readonly BindableProperty IsCheckedProperty =
                BindableProperty.Create(
                    nameof(IsChecked),
                    typeof(bool),
                    typeof(FreakyRadioButton),
                    false,
                    BindingMode.TwoWay,
                    propertyChanged: OnCheckedChanged);

        public static readonly BindableProperty NameProperty =
                BindableProperty.Create(
                    nameof(Name),
                    typeof(string),
                    typeof(FreakyRadioButton),
                    defaultName,
                    propertyChanged: SizeRequestChanged);

        public static readonly BindableProperty OutlineColorProperty =
                BindableProperty.Create(
                nameof(OutlineColor),
                typeof(Color),
                typeof(FreakyRadioButton),
                Colors.Black);

        public static readonly BindableProperty OutlineWidthProperty =
                BindableProperty.Create(
                    nameof(OutlineWidth),
                    typeof(float),
                    typeof(FreakyRadioButton),
                    outlineWidth);

        public static readonly BindableProperty SizeRequestProperty =
                BindableProperty.Create(
                    nameof(SizeRequest),
                    typeof(double),
                    typeof(FreakyRadioButton),
                    size,
                    propertyChanged: SizeRequestChanged);

        /// <summary>
        /// Gets or sets the color of the check.
        /// </summary>
        /// <value>Color of the check.</value>
        public Color CheckColor
        {
            get { return (Color)GetValue(CheckColorProperty); }
            set { SetValue(CheckColorProperty, value); }
        }

        /// <summary>
        /// Triggered when <see cref="FreakyRadioButton.IsChecked"/> changes.
        /// </summary>
        public ICommand CheckedChangedCommand
        {
            get { return (ICommand)GetValue(CheckedChangedCommandProperty); }
            set { SetValue(CheckedChangedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        /// <value>Color value of the fill.</value>
        public Color FillColor
        {
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the outline.
        /// </summary>
        /// <value>Color value of the outline</value>
        public bool HasCheckAnimation
        {
            get => (bool)GetValue(HasCheckAnimationProperty);
            set => SetValue(HasCheckAnimationProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FreakyRadioButton"/> is checked.
        /// </summary>
        /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the identifier of this <see cref="FreakyRadioButton"/>
        /// </summary>
        /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the outline.
        /// </summary>
        /// <value>Color value of the outline</value>
        public Color OutlineColor
        {
            get { return (Color)GetValue(OutlineColorProperty); }
            set { SetValue(OutlineColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the outline.
        /// </summary>
        /// <value>The width of the outline</value>
        public float OutlineWidth
        {
            get { return (float)GetValue(OutlineWidthProperty); }
            set { SetValue(OutlineWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the size of this <see cref="FreakyRadioButton"/>
        /// </summary>
        /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
        public double SizeRequest
        {
            get { return (double)GetValue(SizeRequestProperty); }
            set { SetValue(SizeRequestProperty, value); }
        }

        private static async void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is FreakyRadioButton radioButton)) return;
            radioButton.CheckedChanged?.Invoke(radioButton, new CheckedChangedEventArgs((bool)newValue));
            radioButton.CheckedChangedCommand?.ExecuteCommandIfAvailable(newValue);
            radioButton.ChangeVisualState();
            await radioButton.ToggleAnimationAsync();
        }

        private static void SizeRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is FreakyRadioButton RadioButton)) return;
            RadioButton.WidthRequest = RadioButton.HeightRequest = (double)(newValue);
            RadioButton.skiaView.WidthRequest = RadioButton.skiaView.HeightRequest = (double)(newValue);
        }

        #endregion Bindable Properties
    }
}