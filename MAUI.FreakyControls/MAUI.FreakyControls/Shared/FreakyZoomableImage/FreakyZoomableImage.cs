namespace Maui.FreakyControls;

public class FreakyZoomableImage : ContentView, IDisposable
{
    private double currentScale = 1;
    private double startScale = 1;
    private double xOffset = 0;
    private double yOffset = 0;
    private bool repeatedDoubleTap = false; //boolean checking if the user doubletapped for the first time or second time

    readonly PinchGestureRecognizer pinchGesture;
    readonly PanGestureRecognizer panGesture;
    readonly TapGestureRecognizer tapGesture;

    public FreakyZoomableImage()
    {
        pinchGesture = new PinchGestureRecognizer();
        pinchGesture.PinchUpdated += PinchUpdated;
        GestureRecognizers.Add(pinchGesture);

        panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        GestureRecognizers.Add(panGesture);

        tapGesture = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
        tapGesture.Tapped += DoubleTapped;
        GestureRecognizers.Add(tapGesture);
    }

    private void PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        switch (e.Status)
        {
            case GestureStatus.Started:
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
                break;
            case GestureStatus.Running:
                {
                    currentScale += (e.Scale - 1) * startScale;
                    currentScale = Math.Max(1, currentScale);

                    var renderedX = Content.X + xOffset;
                    var deltaX = renderedX / Width;
                    var deltaWidth = Width / (Content.Width * startScale);
                    var originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                    var renderedY = Content.Y + yOffset;
                    var deltaY = renderedY / Height;
                    var deltaHeight = Height / (Content.Height * startScale);
                    var originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                    var targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                    var targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                    Content.TranslationX = Math.Min(0, Math.Max(targetX, -Content.Width * (currentScale - 1)));
                    Content.TranslationY = Math.Min(0, Math.Max(targetY, -Content.Height * (currentScale - 1)));

                    Content.Scale = currentScale;
                    break;
                }
            case GestureStatus.Completed:
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
                break;
        }
    }

    public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (Content.Scale == 1)
        {
            return;
        }

        switch (e.StatusType)
        {
            case GestureStatus.Running:

                var newX = (e.TotalX * Scale) + xOffset;
                var newY = (e.TotalY * Scale) + yOffset;

                var width = (Content.Width * Content.Scale);
                var height = (Content.Height * Content.Scale);

                var canMoveX = width > Application.Current.MainPage.Width;
                var canMoveY = height > Application.Current.MainPage.Height;

                if (canMoveX)
                {
                    var minX = (width - (Application.Current.MainPage.Width / 2)) * -1;
                    var maxX = Math.Min(Application.Current.MainPage.Width / 2, width / 2);

                    if (newX < minX)
                    {
                        newX = minX;
                    }

                    if (newX > maxX)
                    {
                        newX = maxX;
                    }
                }
                else
                {
                    newX = 0;
                }

                if (canMoveY)
                {
                    var minY = (height - (Application.Current.MainPage.Height / 2)) * -1;
                    var maxY = Math.Min(Application.Current.MainPage.Width / 2, height / 2);

                    if (newY < minY)
                    {
                        newY = minY;
                    }

                    if (newY > maxY)
                    {
                        newY = maxY;
                    }
                }
                else
                {
                    newY = 0;
                }

                Content.TranslationX = newX;
                Content.TranslationY = newY;
                break;
            case GestureStatus.Completed:
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
                break;
            case GestureStatus.Started:
                break;
            case GestureStatus.Canceled:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async void DoubleTapped(object sender, EventArgs e)
    {
        var multiplicator = Math.Pow(2, 1.0 / 10.0);
        startScale = Content.Scale;
        Content.AnchorX = 0;
        Content.AnchorY = 0;

        for (var i = 0; i < 10; i++)
        {
            if (!repeatedDoubleTap) //if it's not the second double tapp we enlarge the scale
            {
                currentScale *= multiplicator;
            }
            else //if it's the second double tap we make the scale smaller again 
            {
                currentScale /= multiplicator;
            }

            var renderedX = Content.X + xOffset;
            var deltaX = renderedX / Width;
            var deltaWidth = Width / (Content.Width * startScale);
            var originX = (0.5 - deltaX) * deltaWidth;

            var renderedY = Content.Y + yOffset;
            var deltaY = renderedY / Height;
            var deltaHeight = Height / (Content.Height * startScale);
            var originY = (0.5 - deltaY) * deltaHeight;

            var targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
            var targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

            Content.TranslationX = Math.Min(0, Math.Max(targetX, -Content.Width * (currentScale - 1)));
            Content.TranslationY = Math.Min(0, Math.Max(targetY, -Content.Height * (currentScale - 1)));

            Content.Scale = currentScale;
            await Task.Delay(10);
        }
        repeatedDoubleTap = !repeatedDoubleTap;
        xOffset = Content.TranslationX;
        yOffset = Content.TranslationY;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
        pinchGesture.PinchUpdated -= PinchUpdated;
        panGesture.PanUpdated -= OnPanUpdated;
        tapGesture.Tapped -= DoubleTapped;
        GestureRecognizers.Clear();
    }

    ~FreakyZoomableImage()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
}