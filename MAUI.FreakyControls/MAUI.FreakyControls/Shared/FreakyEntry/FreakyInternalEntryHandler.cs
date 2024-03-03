using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

#if ANDROID || IOS

internal sealed partial class FreakyInternalEntryHandler : EntryHandler
{
    public FreakyInternalEntryHandler()
    {
        Mapper.AppendToMapping("FreakyEntryCustomization", MapFreakyEntry);
    }

    private void MapFreakyEntry(IEntryHandler entryHandler, IEntry entry)
    {
        if (entry is FreakyEntry freakyEntry && entryHandler is FreakyInternalEntryHandler freakyEntryHandler)
        {
            if (PlatformView is not null && VirtualView is not null)
            {
                HandleAllowCopyPaste(freakyEntry);
            }
        }
    }
}

#else
internal partial class FreakyInternalEntryHandler : EntryHandler
{
}
#endif