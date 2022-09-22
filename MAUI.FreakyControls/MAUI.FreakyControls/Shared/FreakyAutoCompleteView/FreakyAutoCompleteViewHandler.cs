using System;
using Maui.FreakyControls.Shared.Enums;
using Maui.FreakyControls.Shared.Extensions;
using static Maui.FreakyControls.Extensions.Extensions;
using Maui.FreakyControls.Extensions;
#if ANDROID
using FreakyAutoBox = Maui.FreakyControls.Platforms.Android.NativeControls.DroidFreakyAutoBox;
using static Android.Provider.MediaStore;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
#endif
#if IOS
using Maui.FreakyControls.Platforms.iOS;
using FreakyAutoBox = Maui.FreakyControls.Platforms.iOS.NativeControls.IOSFreakyAutoBox;
#endif
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls
{
    public partial class FreakyAutoCompleteViewHandler : ViewHandler<FreakyAutoCompleteView, FreakyAutoBox>
    {
        static FreakyAutoCompleteViewHandler()
        {

        }

        public FreakyAutoCompleteViewHandler() : base(PropertyMapper)
        {

        }

        public static CommandMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler> CommandMapper = new(ViewCommandMapper)
        {

        };

        public static IPropertyMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler> PropertyMapper =
            new PropertyMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler>(ViewHandler.ViewMapper)
            {
                [nameof(IFreakyAutoCompleteView.CharacterSpacing)] = MapCharacterSpacing,
                [nameof(IFreakyAutoCompleteView.ClearButtonVisibility)] = MapClearButtonVisibility,
                [nameof(IFreakyAutoCompleteView.Font)] = MapFont,
                [nameof(IFreakyAutoCompleteView.IsPassword)] = MapIsPassword,
                [nameof(IFreakyAutoCompleteView.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
                [nameof(IFreakyAutoCompleteView.VerticalTextAlignment)] = MapVerticalTextAlignment,
                [nameof(IFreakyAutoCompleteView.IsReadOnly)] = MapIsReadOnly,
                [nameof(IFreakyAutoCompleteView.IsTextPredictionEnabled)] = MapIsTextPredictionEnabled,
                [nameof(IFreakyAutoCompleteView.Keyboard)] = MapKeyboard,
                [nameof(IFreakyAutoCompleteView.MaxLength)] = MapMaxLength,
                [nameof(IFreakyAutoCompleteView.Placeholder)] = MapPlaceholder,
                [nameof(IFreakyAutoCompleteView.PlaceholderColor)] = MapPlaceholderColor,
                [nameof(IFreakyAutoCompleteView.ReturnType)] = MapReturnType,
                [nameof(IFreakyAutoCompleteView.Text)] = MapText,
                [nameof(IFreakyAutoCompleteView.TextColor)] = MapTextColor,
                [nameof(IFreakyAutoCompleteView.CursorPosition)] = MapCursorPosition,
                [nameof(IFreakyAutoCompleteView.SelectionLength)] = MapSelectionLength
            };

        private void QuerySubmitted(object sender, FreakyAutoCompleteViewQuerySubmittedEventArgs e)
        {
            MessagingCenter.Send(VirtualView, "FreakyAutoCompleteView_" + nameof(FreakyAutoCompleteView.QuerySubmitted), (e.QueryText, e.ChosenSuggestion));
        }

        private void TextChanged(object sender, FreakyAutoCompleteViewTextChangedEventArgs e)
        {
            MessagingCenter.Send(VirtualView, "FreakyAutoCompleteView_" + nameof(FreakyAutoCompleteView.TextChanged), (PlatformView.Text, (TextChangeReason)e.Reason));
        }

        private void SuggestionChosen(object sender, FreakyAutoCompleteViewSuggestionChosenEventArgs e)
        {
            MessagingCenter.Send(VirtualView, "FreakyAutoCompleteView_" + nameof(FreakyAutoCompleteView.SuggestionChosen), e.SelectedItem);
        }

        #region Mappers

        private static void MapText(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView view)
        {
            handler?.UpdateText();
            MapFormatting(handler, view);
        }

        public static void MapFormatting(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateMaxLength(entry);

            // Update all of the attributed text formatting properties
            handler.PlatformView?.UpdateCharacterSpacing(entry);

            // Setting any of those may have removed text alignment settings,
            // so we need to make sure those are applied, too
            handler.PlatformView?.UpdateHorizontalTextAlignment(entry);
        }

        private static void MapKeyboard(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateKeyboard(entry);
        }

        private static void MapSelectionLength(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateSelectionLength(entry);
        }

        private static void MapCursorPosition(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateCursorPosition(entry);
        }

        private static void MapTextColor(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateTextColor(entry);
        }

        private static void MapReturnType(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateReturnType(entry);
        }

        private static void MapPlaceholderColor(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdatePlaceholder(entry);
        }

        private static void MapPlaceholder(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdatePlaceholder(entry);
        }

        private static void MapMaxLength(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateMaxLength(entry);
        }

        private static void MapIsTextPredictionEnabled(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateIsTextPredictionEnabled(entry);
        }

        private static void MapIsReadOnly(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateIsReadOnly(entry);
        }

        private static void MapVerticalTextAlignment(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler?.PlatformView?.UpdateVerticalTextAlignment(entry);
        }

        private static void MapHorizontalTextAlignment(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateHorizontalTextAlignment(entry);
        }

        private static void MapIsPassword(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateIsPassword(entry);
        }

        private static void MapFont(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateFont(entry, ServiceHelper.GetService<IFontManager>());

        }

        private static void MapClearButtonVisibility(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
#if ANDROID
            handler.PlatformView?.UpdateClearButtonVisibility(entry, (Android.Graphics.Drawables.Drawable)null);
#endif
#if IOS
            handler.PlatformView?.UpdateClearButtonVisibility(entry);

#endif
        }

        private static void MapCharacterSpacing(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView entry)
        {
            handler.PlatformView?.UpdateCharacterSpacing(entry);
        }

        #endregion

        private void UpdateTextOnSelect()
        {
            PlatformView.UpdateTextOnSelect = VirtualView.UpdateTextOnSelect;
        }

        private void UpdateIsSuggestionListOpen()
        {
            PlatformView.IsSuggestionListOpen = VirtualView.IsSuggestionListOpen;
        }

        private void UpdatePlaceholderText()
        {
            PlatformView.PlaceholderText = VirtualView.Placeholder;
        }

        private void UpdatePlaceholderTextColor()
        {
            var placeholderColor = VirtualView.PlaceholderColor;
            PlatformView.SetPlaceholderTextColor(placeholderColor);
        }

        private void UpdateText()
        {
            if (PlatformView.Text != VirtualView.Text)
                PlatformView.Text = VirtualView.Text ?? String.Empty;
        }

        private void UpdateTextColor()
        {
            var color = VirtualView.TextColor;
            PlatformView.SetTextColor(color);
        }

        private void UpdateItemsSource()
        {
            PlatformView.SetItems(VirtualView?.ItemsSource?.OfType<object>(), (o) => FormatType(o, VirtualView?.DisplayMemberPath), (o) => FormatType(o, VirtualView?.TextMemberPath));
        }
    }
}