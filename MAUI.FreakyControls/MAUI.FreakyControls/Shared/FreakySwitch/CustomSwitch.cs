using System.Diagnostics.CodeAnalysis;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Maui.FreakyControls
{
    [Experimental("Risky")]
    public class CustomSwitch : SKCanvasView
    {
        private bool isToggled = false;

        public bool IsToggled
        {
            get { return isToggled; }
            set
            {
                if (isToggled != value)
                {
                    isToggled = value;
                    AnimateSwitch();
                    InvalidateSurface();
                }
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            DrawSwitch(canvas);
        }

        private void DrawSwitch(SKCanvas canvas)
        {
            float width = CanvasSize.Width;
            float height = CanvasSize.Height;

            // Background track
            using (SKPaint trackPaint = new SKPaint())
            {
                trackPaint.Color = IsToggled ? SKColors.LightGreen : SKColors.Gray;
                canvas.DrawRoundRect(new SKRect(0, height / 4, width, 3 * height / 4), 10, 10, trackPaint);
            }

            // Thumb
            using (SKPaint thumbPaint = new SKPaint())
            {
                thumbPaint.Color = SKColors.White;
                float thumbX = IsToggled ? width - height / 2 : 0;
                canvas.DrawCircle(thumbX + height / 4, height / 2, height / 4, thumbPaint);
            }
        }

        protected override void OnTouch(SKTouchEventArgs e)
        {
            base.OnTouch(e);

            if (e.ActionType == SKTouchAction.Pressed || e.ActionType == SKTouchAction.Released)
            {
                if (e.InContact)
                {
                    IsToggled = !IsToggled;
                    e.Handled = true;
                }
            }
        }

        private async void AnimateSwitch()
        {
            await this.ScaleTo(0.8, 50, Easing.CubicOut);
            await this.ScaleTo(1, 50, Easing.CubicIn);
        }
    }
}