using System;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Handlers;

namespace MAUI.FreakyControls
{
    public partial class FreakyEntryHandler : ViewHandler<IFreakyEntry, AppCompatEditText>
    {
        public FreakyEntryHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) :
            base(mapper, commandMapper)
        {
        }

        protected override AppCompatEditText CreatePlatformView()
        {
            var nativeEntry = new AppCompatEditText(Context);
            return nativeEntry;
        }
    }
}

