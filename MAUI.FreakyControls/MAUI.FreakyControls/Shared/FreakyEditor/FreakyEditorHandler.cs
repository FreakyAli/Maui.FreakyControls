using Maui.FreakyControls.Extensions;
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

#if ANDROID || IOS
public sealed partial class FreakyEditorHandler : EditorHandler
{
    public FreakyEditorHandler()
    {
        Mapper.AppendToMapping("FreakyEntryCustomization", MapFreakyEditor);
    }

    // Todo: Remove try-catch added as a quickfix for https://github.com/FreakyAli/Maui.FreakyControls/issues/76
    private void MapFreakyEditor(IEditorHandler editorHandler, IEditor editor)
    {
        try
        {
            if (editor is FreakyEditor feditor && editorHandler is FreakyEditorHandler freakyEditorHandler)
            {
                if (PlatformView != null && VirtualView != null)
                {
                    HandleAllowCopyPaste(feditor);
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
public partial class FreakyEditorHandler : EditorHandler
{
}
#endif