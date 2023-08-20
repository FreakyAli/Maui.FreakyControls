using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

#if ANDROID || IOS
public sealed partial class FreakyEditorHandler : EditorHandler
{
    public FreakyEditorHandler()
    {
        Mapper.AppendToMapping("FreakyEntryCustomization", MapFreakyEditor);
    }

    private void MapFreakyEditor(IEditorHandler editorHandler, IEditor editor)
    {
        if (editor is FreakyEditor feditor && editorHandler is FreakyEditorHandler freakyEditorHandler)
        {
            if (PlatformView != null && VirtualView != null)
            {
                HandleAllowCopyPaste(feditor);
            }
        }
    }
}
#else
public partial class FreakyEditorHandler : EditorHandler
{
}
#endif