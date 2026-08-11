using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.Android.NativeControls;

namespace Maui.FreakyControls;

public class DrawableHandlerCallback : IDrawableClickListener
{
    private readonly IDrawableImageView frentry;

    public DrawableHandlerCallback(IDrawableImageView frentry)
    {
        this.frentry = frentry;
    }

    public void OnClick(DrawablePosition target)
    {
        switch (target)
        {
            case DrawablePosition.Left:
            case DrawablePosition.Right:
                frentry.ImageCommand?.ExecuteWhenAvailable(frentry.ImageCommandParameter);
                break;
        }
    }
}