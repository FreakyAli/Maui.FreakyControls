using System;
namespace MAUI.FreakyControls
{
    public interface IFreakyEditor : IEditor
    {
        bool HasUnderline { get; }
        Color UnderlineColor { get; }
    }
}

