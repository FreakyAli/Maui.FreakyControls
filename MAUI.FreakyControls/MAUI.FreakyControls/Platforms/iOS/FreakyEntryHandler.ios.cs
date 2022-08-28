using System;
using Microsoft.Maui.Handlers;
using UIKit;

namespace MAUI.FreakyControls
{
    public partial class FreakyEntryHandler : ViewHandler<IFreakyEntry, UITextField>
    {
        public FreakyEntryHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) : base(mapper, commandMapper)
        {
        }

        protected override UITextField CreatePlatformView() =>
           new UITextField
           {
               BorderStyle = UITextBorderStyle.RoundedRect,
               ClipsToBounds = true
           };
    }
}

