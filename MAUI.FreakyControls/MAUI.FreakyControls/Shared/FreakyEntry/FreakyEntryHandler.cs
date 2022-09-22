using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls
{
    public sealed partial class FreakyEntryHandler : EntryHandler
    {
        public FreakyEntryHandler()
        {
            Mapper.AppendToMapping("FreakyEntryCustomization", MapFreakyEntry);
        }

        private void MapFreakyEntry(IEntryHandler entryHandler, IEntry entry)
        {
            if (entry is FreakyEntry freakyEntry && entryHandler is FreakyEntryHandler freakyEntryHandler)
            {
                if (freakyEntry.ImageSource != default(ImageSource))
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    freakyEntryHandler.HandleAndAlignImageSourceAsync(freakyEntry);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }

                HandleAllowCopyPaste(freakyEntry);
            }
        }
    }
}

