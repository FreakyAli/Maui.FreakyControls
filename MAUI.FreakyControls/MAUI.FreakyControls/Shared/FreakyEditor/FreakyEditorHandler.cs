using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace MAUI.FreakyControls
{
    public partial class FreakyEditorHandler : EditorHandler
    {
        public FreakyEditorHandler()
        {
            Mapper.AppendToMapping("FreakyEntryCustomization", MapFreakyEditor);
        }

        private void MapFreakyEditor(IEditorHandler editorHandler, IEditor editor)
        {
            if (editor is FreakyEditor feditor && editorHandler is FreakyEditorHandler freakyEditorHandler)
            {
                HandleAllowCopyPaste(feditor);
            }
        }
    }
}

