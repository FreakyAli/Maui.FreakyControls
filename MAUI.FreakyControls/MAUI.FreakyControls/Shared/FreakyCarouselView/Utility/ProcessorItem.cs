using System;
using System.Collections;
using System.Collections.Specialized;
using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls.Utility;

public struct ProcessorItem
{
    public bool IsFront { get; set; }
    public AnimationDirection Direction { get; set; }
    public IEnumerable<View> Views { get; set; }
    public IEnumerable<View> InactiveViews { get; set; }
}
