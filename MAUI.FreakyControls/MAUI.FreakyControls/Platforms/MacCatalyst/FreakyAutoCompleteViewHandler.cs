using Microsoft.Maui.Handlers;
using UIKit;
namespace Maui.FreakyControls;

public partial class FreakyAutoCompleteViewHandler : ViewHandler<View, UIView>
{
    public FreakyAutoCompleteViewHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) : base(mapper, commandMapper)
    {
    }

    protected override UIView CreatePlatformView()
    {
        return new UIView();
    }
}