using System;
using Foundation;
using UIKit;

namespace MAUI.FreakyControls
{
    internal class DisableCopyPasteDelegate : NSObject, IUITextFieldDelegate
    {
        public bool IsCopyPasteDisabled { get; set; }

        public DisableCopyPasteDelegate(bool isCopyPasteDisabled)
        {
            this.IsCopyPasteDisabled = isCopyPasteDisabled;
        }

        [Export("textField:shouldChangeCharactersInRange:replacementString:")]
        public bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString) =>
            IsCopyPasteDisabled;
    }
}

