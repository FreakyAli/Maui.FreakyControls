using MAUI.FreakyControls.Platforms.Android.NativeControls;

namespace MAUI.FreakyControls
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
                    if (frentry.ImageCommand?.CanExecute(frentry.ImageCommandParameter) == true)
                    {
                        frentry.ImageCommand.Execute(frentry.ImageCommandParameter);
                    }
                    break;
                case DrawablePosition.Right:
                    if (frentry.ImageCommand?.CanExecute(frentry.ImageCommandParameter) == true)
                    {
                        frentry.ImageCommand.Execute(frentry.ImageCommandParameter);
                    }
                    break;
            }
        }
    }
}

