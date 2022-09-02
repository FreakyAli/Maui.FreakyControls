using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace MAUI.FreakyControls
{
    public partial class FreakyEditorHandler : EditorHandler
    {
        public FreakyEditorHandler()
        {
            Mapper.AppendToMapping("HasUnderline", MapHasUnderlineWithColor);
        }

        private static void MapHasUnderlineWithColor(IEditorHandler editorHandler, IEditor editor)
        {
            if (editor is FreakyEditor freakyEditor)
            {
                (editorHandler as FreakyEditorHandler)?.HandleNativeHasUnderline(freakyEditor.HasUnderline, freakyEditor.UnderlineColor);
            }
        }
    }
}

