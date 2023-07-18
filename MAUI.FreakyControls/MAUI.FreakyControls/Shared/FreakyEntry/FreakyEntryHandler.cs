using Maui.FreakyControls.Extensions;
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

#if ANDROID || IOS

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
                freakyEntryHandler.HandleAndAlignImageSourceAsync(freakyEntry).RunConcurrently();
            HandleAllowCopyPaste(freakyEntry);
        }
    }
}

#else
public partial class FreakyEntryHandler : EntryHandler
{
}
#endif