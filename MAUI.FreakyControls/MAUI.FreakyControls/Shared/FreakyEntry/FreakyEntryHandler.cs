using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace MAUI.FreakyControls
{
    public partial class FreakyEntryHandler : EntryHandler
    {
        public FreakyEntryHandler()
        {
            Mapper.AppendToMapping("ImageCustomization", MapImageDrawableCustomization);
        }

        private void MapImageDrawableCustomization(IEntryHandler entryHandler, IEntry entry)
        {
            if (entry is FreakyEntry freakyEntry && entryHandler is FreakyEntryHandler freakyEntryHandler)
            {
                if (freakyEntry.ImageSource != default(ImageSource))
                {
                    freakyEntryHandler.HandleAndAlignImageSource(freakyEntry);
                }
            }
        }
    }
}

