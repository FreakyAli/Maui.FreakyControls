using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#if ANDROID
using NativeView = Maui.FreakyControls.Platforms.Android.SignaturePadCanvasView;
using NativePoint = System.Drawing.PointF;
#elif IOS
using NativeView = Maui.FreakyControls.Platforms.iOS.SignaturePadCanvasView;
using NativePoint = CoreGraphics.CGPoint;
#else
#endif

namespace Maui.FreakyControls
{
#if IOS || ANDROID
    public partial class FreakySignatureCanvasViewHandler : ViewHandler<FreakySignatureCanvasView, NativeView>
    {
        public static PropertyMapper<FreakySignatureCanvasView, FreakySignatureCanvasViewHandler> Mapper =
            new(ViewHandler.ViewMapper)
            {
                [nameof(FreakySignatureCanvasView.StrokeColor)] = MapStrokeColor,
                [nameof(FreakySignatureCanvasView.StrokeWidth)] = MapStrokeWidth
            };

        public static CommandMapper<FreakySignatureCanvasView, FreakySignatureCanvasViewHandler> CommandMapper =
            new(ViewHandler.ViewCommandMapper)
            {
            };

        public FreakySignatureCanvasViewHandler() : base(Mapper)
        {
        }

        public FreakySignatureCanvasViewHandler(IPropertyMapper? mapper)
            : base(mapper ?? Mapper, CommandMapper)
        {
        }

        public FreakySignatureCanvasViewHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
        }

        protected override void ConnectHandler(NativeView platformView)
        {
            base.ConnectHandler(platformView);

            platformView.StrokeCompleted += PlatformView_StrokeCompleted;
            platformView.Cleared += OnCleared;

            VirtualView.ImageStreamRequested += OnImageStreamRequested;
            VirtualView.IsBlankRequested += OnIsBlankRequested;
            VirtualView.PointsRequested += OnPointsRequested;
            VirtualView.PointsSpecified += OnPointsSpecified;
            VirtualView.StrokesRequested += OnStrokesRequested;
            VirtualView.StrokesSpecified += OnStrokesSpecified;
            VirtualView.ClearRequested += OnClearRequested;
        }


        protected override void DisconnectHandler(NativeView platformView)
        {
            base.DisconnectHandler(platformView);
            platformView.StrokeCompleted -= PlatformView_StrokeCompleted;
            platformView.Cleared -= OnCleared;

            VirtualView.ImageStreamRequested -= OnImageStreamRequested;
            VirtualView.IsBlankRequested -= OnIsBlankRequested;
            VirtualView.PointsRequested -= OnPointsRequested;
            VirtualView.PointsSpecified -= OnPointsSpecified;
            VirtualView.StrokesRequested -= OnStrokesRequested;
            VirtualView.StrokesSpecified -= OnStrokesSpecified;
            VirtualView.ClearRequested -= OnClearRequested;
        }

        private static void MapStrokeWidth(FreakySignatureCanvasViewHandler signaturePadCanvasViewHandler,
           FreakySignatureCanvasView signaturePadCanvasView)
        {
            signaturePadCanvasViewHandler.PlatformView.StrokeWidth =
               signaturePadCanvasViewHandler.VirtualView.StrokeWidth;
        }

        private static void MapStrokeColor(FreakySignatureCanvasViewHandler signaturePadCanvasViewHandler,
            FreakySignatureCanvasView signaturePadCanvasView)
        {
            signaturePadCanvasViewHandler.PlatformView.StrokeColor =
                signaturePadCanvasViewHandler.VirtualView.StrokeColor.ToPlatform();
        }

        private void OnCleared(object sender, EventArgs e)
        {
            VirtualView?.OnCleared();
        }

        private void PlatformView_StrokeCompleted(object sender, EventArgs e)
        {
            VirtualView?.OnStrokeCompleted();
        }

        private void OnIsBlankRequested(object sender, IsBlankRequestedEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                e.IsBlank = ctrl.IsBlank;
            }
        }

        private void OnPointsRequested(object sender, PointsEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                e.Points = ctrl.Points.Select(p => new Point(p.X, p.Y));
            }
        }

        private void OnPointsSpecified(object sender, PointsEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                ctrl.LoadPoints(e.Points.Select(p => new NativePoint((float)p.X, (float)p.Y)).ToArray());
            }
        }

        private void OnStrokesRequested(object sender, StrokesEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                e.Strokes = ctrl.Strokes.Select(s => s.Select(p => new Point(p.X, p.Y)));
            }
        }

        private void OnStrokesSpecified(object sender, StrokesEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                ctrl.LoadStrokes(e.Strokes.Select(s => s.Select(p => new NativePoint((float)p.X, (float)p.Y)).ToArray()).ToArray());
            }
        }

        private void OnClearRequested(object sender, EventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                ctrl.Clear();
            }
        }
    }
#else
#endif
}