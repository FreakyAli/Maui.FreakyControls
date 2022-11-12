using System;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Maui.FreakyControls
{
    internal class FreakySKCanvasView : BaseSKCanvas
    {
        protected override void DoPaintSurface(SKPaintSurfaceEventArgs skPaintSurfaceEventArgs)
        {
        }
    }

    public abstract class BaseSKCanvas : SKCanvasView
    {
        protected BaseSKCanvas()
        {
            BackgroundColor = Colors.Transparent;
        }

        protected sealed override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            //TODO: Figure out why drawing on coachmark is leading to surface being null
            if (e.Surface == null)
            {
                return;
            }

            e.Surface.Canvas.Clear(SKColors.Transparent);

            // make sure no previous transforms still apply
            e.Surface.Canvas.ResetMatrix();

            base.OnPaintSurface(e);

            DoPaintSurface(e);
        }

        protected abstract void DoPaintSurface(SKPaintSurfaceEventArgs skPaintSurfaceEventArgs);

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName != nameof(IsVisible))
            {
                return;
            }

            InvalidateSurface();
        }
    }
}

