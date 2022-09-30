// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SupportWidgetXF.iOS.Renderers.DropCombo
{
    [Register ("DropItemTitleDescription")]
    partial class DropItemTitleDescription
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton bttClick { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SupportWidgetXF.iOS.Renderers.DropCombo.SupportCheckBoxiOS cbxCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint NsHeightSeperator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint NSSizeOfCheckbox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint NSSpaceBetWeen { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel txtDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel txtSeperator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel txtTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ViewMain { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (bttClick != null) {
                bttClick.Dispose ();
                bttClick = null;
            }

            if (cbxCheckBox != null) {
                cbxCheckBox.Dispose ();
                cbxCheckBox = null;
            }

            if (NsHeightSeperator != null) {
                NsHeightSeperator.Dispose ();
                NsHeightSeperator = null;
            }

            if (NSSizeOfCheckbox != null) {
                NSSizeOfCheckbox.Dispose ();
                NSSizeOfCheckbox = null;
            }

            if (NSSpaceBetWeen != null) {
                NSSpaceBetWeen.Dispose ();
                NSSpaceBetWeen = null;
            }

            if (txtDescription != null) {
                txtDescription.Dispose ();
                txtDescription = null;
            }

            if (txtSeperator != null) {
                txtSeperator.Dispose ();
                txtSeperator = null;
            }

            if (txtTitle != null) {
                txtTitle.Dispose ();
                txtTitle = null;
            }

            if (ViewMain != null) {
                ViewMain.Dispose ();
                ViewMain = null;
            }
        }
    }
}