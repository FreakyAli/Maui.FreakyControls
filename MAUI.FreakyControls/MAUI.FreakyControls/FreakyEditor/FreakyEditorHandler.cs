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
        try
        {
            if (editor is FreakyEditor feditor && editorHandler is FreakyEditorHandler freakyEditorHandler)
            {
                if (PlatformView is not null && VirtualView is not null)
                {
                    HandleAllowCopyPaste(feditor);
                }
            }
        }
        catch (InvalidOperationException ex) { }
    }
}
#else
public partial class FreakyEditorHandler : EditorHandler
{
}
#endif