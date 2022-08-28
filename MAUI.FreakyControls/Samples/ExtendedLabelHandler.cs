using System;
using MAUI.FreakyControls;
using Microsoft.Maui.Handlers;
#if ANDROID
using NativeView = AndroidX.AppCompat.Widget.AppCompatTextView;
#endif
#if IOS
using NativeView = UIKit.UILabel;
#endif
namespace Samples
{
	public partial class ExtendedLabelHandler : ViewHandler<IExtendedLabel,NativeView>
    {
        #region ctor 

        public static CommandMapper<IExtendedLabel, ExtendedLabelHandler> CommandMapper = new(ViewCommandMapper);


        public ExtendedLabelHandler() : base(FreakyEditorMapper)
        {

        }

        public ExtendedLabelHandler(IPropertyMapper mapper = null) : base(mapper ?? FreakyEditorMapper)
        {

        }

        #endregion

        #region Mappers

        public static IPropertyMapper<IExtendedLabel, ExtendedLabelHandler> FreakyEditorMapper = new PropertyMapper<IExtendedLabel, ExtendedLabelHandler>(ViewMapper)
        {
            [nameof(IExtendedLabel.HasUnderline)] = MapHasUnderlineWithColor,
            [nameof(IExtendedLabel.UnderlineColor)] = MapHasUnderlineWithColor
        };

        public static void MapHasUnderlineWithColor(ExtendedLabelHandler handler, IExtendedLabel entry)
        {

        }

        #endregion
    }
}

	