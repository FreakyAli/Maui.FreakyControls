using System;
using Microsoft.Maui.Handlers;
#if ANDROID
using NativeView = AndroidX.AppCompat.Widget.AppCompatEditText;
#endif
#if IOS
using NativeView = UIKit.UITextView;
#endif

namespace MAUI.FreakyControls
{
    public partial class FreakyEditorHandler : ViewHandler<IFreakyEditor, NativeView>
    {
        #region ctor 

        public static CommandMapper<IFreakyEditor, FreakyEditorHandler> CommandMapper = new(ViewCommandMapper);


        public FreakyEditorHandler() : base(FreakyEditorMapper)
        {

        }

        public FreakyEditorHandler(IPropertyMapper pmapper, CommandMapper cmapper = null)
            : base(pmapper ?? FreakyEditorMapper, cmapper ?? CommandMapper)
        {

        }


        #endregion

        #region Mappers

        public static IPropertyMapper<IFreakyEditor, FreakyEditorHandler> FreakyEditorMapper = new PropertyMapper<IFreakyEditor, FreakyEditorHandler>(ViewMapper)
        {
            [nameof(IFreakyEditor.HasUnderline)] = MapHasUnderlineWithColor,
            [nameof(IFreakyEditor.UnderlineColor)] = MapHasUnderlineWithColor
        };

        public static void MapHasUnderlineWithColor(FreakyEditorHandler handler, IFreakyEditor entry)
        {
            handler.HandleNativeHasUnderline(entry.HasUnderline, entry.UnderlineColor);
        }

        #endregion

        protected override void ConnectHandler(NativeView platformView)
        {
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(NativeView platformView)
        {
            base.DisconnectHandler(platformView);
        }
    }
}

