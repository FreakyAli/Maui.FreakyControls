using Maui.FreakyControls.Effects;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Samples.ScratchView;

public partial class ScratchViewPage : ContentPage
{
    public ScratchViewPage()
    {
        InitializeComponent();
        BindingContext = new ScratchViewModel();

    }

    //private ISKScene _scene;
    //private ITouchGestureRecognizer _touchGestureRecognizer;
    //private ISceneGestureResponder _sceneGestureResponder;

    //public MainPage()
    //{
    //    InitializeComponent();
    //    this.SizeChanged += OnSizeChanged;
    //}

    //private void OnSizeChanged(object sender, EventArgs eventArgs)
    //{
    //    SetSceneCenter();
    //}


    //private void SetSceneCenter()
    //{
    //    if (_scene == null)
    //    {
    //        return;
    //    }
    //    var centerPoint = new SKPoint(canvasView.CanvasSize.Width / 2, canvasView.CanvasSize.Height / 2);
    //    _scene.ScreenCenter = centerPoint;
    //}

    //private void InitSceneObjects()
    //{
    //    _scene = new SKScene(new TestScenereRenderer())
    //    {
    //        MaxScale = 10,
    //        MinScale = 0.3f,
    //    };
    //    SetSceneCenter();
    //    _touchGestureRecognizer = new TouchGestureRecognizer();
    //    _sceneGestureResponder = new SceneGestureRenderingResponder(() => canvasView.InvalidateSurface(), _scene, _touchGestureRecognizer)
    //    {
    //        TouchManipulationMode = TouchManipulationMode.IsotropicScale,
    //        MaxFramesPerSecond = 100,
    //    };
    //    _sceneGestureResponder.StartResponding();
    //}

    private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
    {
        var viewPoint = args.Location;
        SKPoint point =
            new SKPoint((float)(canvasView.CanvasSize.Width * viewPoint.X / canvasView.Width),
                        (float)(canvasView.CanvasSize.Height * viewPoint.Y / canvasView.Height));

        var actionType = args.Type;
        // _touchGestureRecognizer.ProcessTouchEvent(args.Id, actionType, point);
    }

    private void OnPaint(object sender, SKPaintSurfaceEventArgs args)
    {
        //if (_scene == null)
        //{
        //    InitSceneObjects();

        //}
        SKImageInfo info = args.Info;
        SKSurface surface = args.Surface;
        SKCanvas canvas = surface.Canvas;
        //_scene.Render(canvas);
    }
}