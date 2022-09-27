using System;
using Maui.FreakyControls.Shared.Enums;
using System.Windows.Input;

namespace Maui.FreakyControls;

public interface IDrawableImageView
{
    public object ImageCommandParameter
    {
        get;
    }

    public ICommand ImageCommand
    {
        get;
    }

    public int ImagePadding
    {
        get;
    }

    public int ImageWidth
    {
        get;
    }

    public int ImageHeight
    {
        get;
    }

    public ImageSource ImageSource
    {
        get;
    }

    public ImageAlignment ImageAlignment
    {
        get;
    }
}

