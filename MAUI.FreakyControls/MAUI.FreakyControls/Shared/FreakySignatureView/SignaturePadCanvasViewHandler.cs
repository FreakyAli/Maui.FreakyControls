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
    public partial class SignaturePadCanvasViewHandler : ViewHandler<SignaturePadCanvasView, NativeView>
    {
        public static PropertyMapper<SignaturePadCanvasView, SignaturePadCanvasViewHandler> customMapper =
            new(ViewHandler.ViewMapper)
            {

            };

        public static CommandMapper<SignaturePadCanvasView, SignaturePadCanvasViewHandler> customCommandMapper =
            new(ViewHandler.ViewCommandMapper)
            {

            };

        public SignaturePadCanvasViewHandler(PropertyMapper mapper = null) : base(mapper ?? customMapper)
        {
        }

        public SignaturePadCanvasViewHandler() : base(customMapper, customCommandMapper)
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
            UpdateAll();
        }


        protected override void DisconnectHandler(NativeView platformView)
        {
            base.DisconnectHandler(platformView);
            VirtualView.ImageStreamRequested -= OnImageStreamRequested;
            VirtualView.IsBlankRequested -= OnIsBlankRequested;
            VirtualView.PointsRequested -= OnPointsRequested;
            VirtualView.PointsSpecified -= OnPointsSpecified;
            VirtualView.StrokesRequested -= OnStrokesRequested;
            VirtualView.StrokesSpecified -= OnStrokesSpecified;
            VirtualView.ClearRequested -= OnClearRequested;
        }

        private void OnCleared(object sender, EventArgs e)
        {
            VirtualView?.OnCleared();
        }

        private void PlatformView_StrokeCompleted(object sender, EventArgs e)
        {
            VirtualView?.OnStrokeCompleted();
        }

        private void OnIsBlankRequested(object sender, SignaturePadCanvasView.IsBlankRequestedEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                e.IsBlank = ctrl.IsBlank;
            }
        }

        private void OnPointsRequested(object sender, SignaturePadCanvasView.PointsEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                e.Points = ctrl.Points.Select(p => new Point(p.X, p.Y));
            }
        }

        private void OnPointsSpecified(object sender, SignaturePadCanvasView.PointsEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                ctrl.LoadPoints(e.Points.Select(p => new NativePoint((float)p.X, (float)p.Y)).ToArray());
            }
        }

        private void OnStrokesRequested(object sender, SignaturePadCanvasView.StrokesEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                e.Strokes = ctrl.Strokes.Select(s => s.Select(p => new Point(p.X, p.Y)));
            }
        }

        private void OnStrokesSpecified(object sender, SignaturePadCanvasView.StrokesEventArgs e)
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

        /// <summary>
        /// Update all the properties on the native view.
        /// </summary>
        private void UpdateAll()
        {
            if (this.PlatformView == null || VirtualView == null)
            {
                return;
            }

            if (VirtualView.StrokeColor != Colors.Black)
            {
                this.PlatformView.StrokeColor = VirtualView.StrokeColor.ToPlatform();
            }
            if (VirtualView.StrokeWidth > 0)
            {
                this.PlatformView.StrokeWidth = VirtualView.StrokeWidth;
            }
        }

        /// <summary>
        /// Update a specific property on the native view.
        /// </summary>
        private void Update(string property)
        {
            if (this.PlatformView == null || VirtualView == null)
            {
                return;
            }

            if (property == SignaturePadCanvasView.StrokeColorProperty.PropertyName)
            {
                this.PlatformView.StrokeColor = VirtualView.StrokeColor.ToPlatform();
            }
            else if (property == SignaturePadCanvasView.StrokeWidthProperty.PropertyName)
            {
                this.PlatformView.StrokeWidth = VirtualView.StrokeWidth;
            }
        }
    }
#else
#endif
}

