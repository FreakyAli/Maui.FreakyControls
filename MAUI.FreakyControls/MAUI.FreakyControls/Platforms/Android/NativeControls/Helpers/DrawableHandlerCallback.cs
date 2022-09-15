using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.Android.NativeControls;

namespace Maui.FreakyControls
{
    public class DrawableHandlerCallback : IDrawableClickListener
    {
        private readonly FreakyEntry frentry;

        public DrawableHandlerCallback(FreakyEntry frentry)
        {
            this.frentry = frentry;
        }

        public void OnClick(DrawablePosition target)
        {
            switch (target)
            {
                case DrawablePosition.Left:
                case DrawablePosition.Right:
                    frentry.ImageCommand?.ExecuteIfAvailable(frentry.ImageCommandParameter);
                    break;
            }
        }
    }
}

