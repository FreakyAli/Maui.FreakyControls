using System;
namespace Samples
{
    public interface IExtendedLabel : ILabel
    {
        bool HasUnderline { get; }
        Color UnderlineColor { get; }
    }
}

