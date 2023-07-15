using Maui.FreakyControls.Shared.Enums;
using System.Windows.Input;

namespace Maui.FreakyControls;

public interface IDrawableImageView
{
    public ImageAlignment ImageAlignment
    {
        get;
    }

    public ICommand ImageCommand
    {
        get;
    }

    public object ImageCommandParameter
    {
        get;
    }

    public int ImageHeight
    {
        get;
    }

    public int ImagePadding
    {
        get;
    }

    public ImageSource ImageSource
    {
        get;
    }

    public int ImageWidth
    {
        get;
    }
}