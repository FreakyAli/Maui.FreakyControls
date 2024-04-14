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

    // Todo: Remove try-catch added as a quickfix for https://github.com/FreakyAli/Maui.FreakyControls/issues/76
    private void MapFreakyEntry(IEntryHandler entryHandler, IEntry entry)
    {
        try
        {
            if (entry is FreakyEntry freakyEntry && entryHandler is FreakyEntryHandler freakyEntryHandler)
            {
                if (PlatformView is not null && VirtualView is not null)
                {
                    if (freakyEntry.ImageSource != default(ImageSource))
                    {
                        freakyEntryHandler.HandleAndAlignImageSourceAsync(freakyEntry).RunConcurrently();
                    }
                    HandleAllowCopyPaste(freakyEntry);
                }
            }
        }
        catch (Exception ex)
        {
            ex.TraceException();
        }
    }
}

#else
public partial class FreakyEntryHandler : EntryHandler
{
}
#endif