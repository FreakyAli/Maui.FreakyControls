using System;
using UIKit;
using Foundation;
using CoreGraphics;
using CoreAnimation;
using System.Runtime.InteropServices;
using Microsoft.Maui.Platform;
using Maui.FreakyControls.Platforms.iOS.NativeControls;

namespace Maui.FreakyControls.Platforms.iOS
{
    public static class NativeExtensions
    {
        public static void MakeCircular(this UIView uIView)
        {
            uIView.ClipsToBounds = true;
            uIView.Layer.CornerRadius = (uIView.Frame.Width + uIView.Frame.Height) / 4;
        }

        public static UIColor FromHex(this UIColor color, string hex)
        {
            int r = 0, g = 0, b = 0, a = 0;

            if (hex.Contains("#"))
                hex = hex.Replace("#", "");

            switch (hex.Length)
            {
                case 2:
                    r = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                    a = 255;
                    break;
                case 3:
                    r = int.Parse(hex.Substring(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex.Substring(1, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex.Substring(2, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    a = 255;
                    break;
                case 4:
                    r = int.Parse(hex.Substring(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex.Substring(1, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex.Substring(2, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    a = int.Parse(hex.Substring(3, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    break;
                case 6:
                    r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    a = 255;
                    break;
                case 8:
                    r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    a = int.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    break;
            }

            return UIColor.FromRGBA(r, g, b, a);
        }
    }

    public static class FreakyAutoBoxiOSExtensions
    {
        public static void UpdateText(this IOSFreakyAutoBox textField, IEntry entry)
        {
            textField.Text = entry.Text;
        }

        public static void UpdateTextColor(this IOSFreakyAutoBox textField, ITextStyle textStyle)
        {
            var textColor = textStyle.TextColor;
            if (textColor != null)
                textField.InputTextField.TextColor = textColor.ToPlatform(UIColor.Black);
        }

        public static void UpdateIsPassword(this IOSFreakyAutoBox textField, IEntry entry)
        {
            if (entry.IsPassword && textField.InputTextField.IsFirstResponder)
            {
                textField.InputTextField.Enabled = false;
                textField.InputTextField.SecureTextEntry = true;
                textField.InputTextField.Enabled = entry.IsEnabled;
                textField.InputTextField.BecomeFirstResponder();
            }
            else
                textField.InputTextField.SecureTextEntry = entry.IsPassword;
        }

        public static void UpdateHorizontalTextAlignment(this IOSFreakyAutoBox textField, ITextAlignment textAlignment)
        {
            textField.InputTextField.TextAlignment = textAlignment.HorizontalTextAlignment.ToPlatformHorizontal();
        }

        public static void UpdateVerticalTextAlignment(this IOSFreakyAutoBox textField, ITextAlignment textAlignment)
        {
            textField.InputTextField.VerticalAlignment = textAlignment.VerticalTextAlignment.ToPlatformVertical();
        }

        public static void UpdateIsTextPredictionEnabled(this IOSFreakyAutoBox textField, IEntry entry)
        {
            if (entry.IsTextPredictionEnabled)
                textField.InputTextField.AutocorrectionType = UITextAutocorrectionType.Yes;
            else
                textField.InputTextField.AutocorrectionType = UITextAutocorrectionType.No;
        }

        public static void UpdateMaxLength(this IOSFreakyAutoBox textField, IEntry entry)
        {
            var newText = textField.InputTextField.AttributedText.TrimToMaxLength(entry.MaxLength);
            if (newText != null && textField.InputTextField.AttributedText != newText)
                textField.InputTextField.AttributedText = newText;
        }

        public static void UpdatePlaceholder(this IOSFreakyAutoBox textField, IEntry entry, Color? defaultPlaceholderColor = null)
        {
            var placeholder = entry.Placeholder;
            if (placeholder == null)
            {
                textField.InputTextField.AttributedPlaceholder = null;
                return;
            }

            var placeholderColor = entry.PlaceholderColor;
            var foregroundColor = placeholderColor ?? defaultPlaceholderColor;

            textField.InputTextField.AttributedPlaceholder = foregroundColor == null
                 ? new NSAttributedString(placeholder)
                 : new NSAttributedString(str: placeholder, foregroundColor: foregroundColor.ToPlatform());

            textField.InputTextField.AttributedPlaceholder.WithCharacterSpacing(entry.CharacterSpacing);
        }

        public static void UpdateIsReadOnly(this IOSFreakyAutoBox textField, IEntry entry)
        {
            textField.InputTextField.UserInteractionEnabled = !(entry.IsReadOnly || entry.InputTransparent);
        }

        public static void UpdateFont(this IOSFreakyAutoBox textField, ITextStyle textStyle, IFontManager fontManager)
        {
            var uiFont = fontManager.GetFont(textStyle.Font, UIFont.LabelFontSize);
            textField.InputTextField.Font = uiFont;
        }

        public static void UpdateReturnType(this IOSFreakyAutoBox textField, IEntry entry)
        {
            textField.InputTextField.ReturnKeyType = entry.ReturnType.ToPlatform();
        }

        public static void UpdateCharacterSpacing(this IOSFreakyAutoBox textField, ITextStyle textStyle)
        {
            var textAttr = textField.InputTextField.AttributedText?.WithCharacterSpacing(textStyle.CharacterSpacing);
            if (textAttr != null)
                textField.InputTextField.AttributedText = textAttr;

            textAttr = textField.InputTextField.AttributedPlaceholder?.WithCharacterSpacing(textStyle.CharacterSpacing);
            if (textAttr != null)
                textField.InputTextField.AttributedPlaceholder = textAttr;
        }

        public static void UpdateKeyboard(this IOSFreakyAutoBox textField, IEntry entry)
        {
            var keyboard = entry.Keyboard;

            textField.InputTextField.ApplyKeyboard(keyboard);

            if (keyboard is not CustomKeyboard)
                textField.InputTextField.UpdateIsTextPredictionEnabled(entry);

            textField.InputTextField.ReloadInputViews();
        }

        public static void UpdateCursorPosition(this IOSFreakyAutoBox textField, IEntry entry)
        {
            var selectedTextRange = textField.InputTextField.SelectedTextRange;
            if (selectedTextRange == null)
                return;
            if (textField.InputTextField.GetOffsetFromPosition(textField.InputTextField.BeginningOfDocument, selectedTextRange.Start) != entry.CursorPosition)
                UpdateCursorSelection(textField, entry);
        }

        public static void UpdateSelectionLength(this IOSFreakyAutoBox textField, IEntry entry)
        {
            var selectedTextRange = textField.InputTextField.SelectedTextRange;
            if (selectedTextRange == null)
                return;
            if (textField.InputTextField.GetOffsetFromPosition(selectedTextRange.Start, selectedTextRange.End) != entry.SelectionLength)
                UpdateCursorSelection(textField, entry);
        }

        /* Updates both the IEntry.CursorPosition and IEntry.SelectionLength properties. */
        static void UpdateCursorSelection(this IOSFreakyAutoBox textField, IEntry entry)
        {
            if (!entry.IsReadOnly)
            {
                UITextPosition start = GetSelectionStart(textField, entry, out int startOffset);
                UITextPosition end = GetSelectionEnd(textField, entry, start, startOffset);

                textField.InputTextField.SelectedTextRange = textField.InputTextField.GetTextRange(start, end);
            }
        }

        static UITextPosition GetSelectionStart(IOSFreakyAutoBox textField, IEntry entry, out int startOffset)
        {
            int cursorPosition = entry.CursorPosition;

            UITextPosition start = textField.InputTextField.GetPosition(textField.InputTextField.BeginningOfDocument, cursorPosition) ?? textField.InputTextField.EndOfDocument;
            startOffset = Math.Max(0, (int)textField.InputTextField.GetOffsetFromPosition(textField.InputTextField.BeginningOfDocument, start));

            if (startOffset != cursorPosition)
                entry.CursorPosition = startOffset;

            return start;
        }

        static UITextPosition GetSelectionEnd(IOSFreakyAutoBox textField, IEntry entry, UITextPosition start, int startOffset)
        {
            int selectionLength = entry.SelectionLength;
            int textFieldLength = textField.Text == null ? 0 : textField.Text.Length;
            // Get the desired range in respect to the actual length of the text we are working with
            UITextPosition end = textField.InputTextField.GetPosition(start, Math.Min(textFieldLength - entry.CursorPosition, selectionLength)) ?? start;
            int endOffset = Math.Max(startOffset, (int)textField.InputTextField.GetOffsetFromPosition(textField.InputTextField.BeginningOfDocument, end));

            int newSelectionLength = Math.Max(0, endOffset - startOffset);
            if (newSelectionLength != selectionLength)
                entry.SelectionLength = newSelectionLength;

            return end;
        }

        public static void UpdateClearButtonVisibility(this IOSFreakyAutoBox textField, IEntry entry)
        {
            textField.InputTextField.ClearButtonMode = entry.ClearButtonVisibility == ClearButtonVisibility.WhileEditing ? UITextFieldViewMode.WhileEditing : UITextFieldViewMode.Never;
        }
    }
}

