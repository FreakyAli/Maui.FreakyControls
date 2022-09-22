using System;
using System.Windows.Input;
using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls.Shared.FreakyEntry
{
    public interface IFreakyEntry : IEntry
    {
        bool AllowCopyPaste { get; }

        object ImageCommandParameter { get; }

        ICommand ImageCommand { get; }

        int ImageWidth { get; }

        int ImageHeight { get; }

        ImageSource ImageSource { get; }

        ImageAlignment ImageAlignment { get; }

    }
}

