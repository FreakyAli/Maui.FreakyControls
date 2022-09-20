using System;
using Maui.FreakyControls.Shared.Enums;
using static Maui.FreakyControls.Extensions.Extensions;
#if ANDROID
using FreakyAutoBox = Maui.FreakyControls.Platforms.Android.NativeControls.DroidFreakyAutoBox;
using static Android.Provider.MediaStore;
#endif
#if IOS
using FreakyAutoBox= Maui.FreakyControls.Platforms.iOS.NativeControls.IOSFreakyAutoBox;
#endif
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls
{
    public partial class FreakyAutoCompleteViewHandler : ViewHandler<FreakyAutoCompleteView, FreakyAutoBox>
    {
        public static IPropertyMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler> PropertyMapper =
            new PropertyMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler>(ViewHandler.ViewMapper)
            {
                    //[nameof(IEntry.Background)] = MapBackground,
                    //[nameof(IEntry.CharacterSpacing)] = MapCharacterSpacing,
                    //[nameof(IEntry.ClearButtonVisibility)] = MapClearButtonVisibility,
                    //[nameof(IEntry.Font)] = MapFont,
                    //[nameof(IEntry.IsPassword)] = MapIsPassword,
                    //[nameof(IEntry.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
                    //[nameof(IEntry.VerticalTextAlignment)] = MapVerticalTextAlignment,
                    //[nameof(IEntry.IsReadOnly)] = MapIsReadOnly,
                    //[nameof(IEntry.IsTextPredictionEnabled)] = MapIsTextPredictionEnabled,
                    //[nameof(IEntry.Keyboard)] = MapKeyboard,
                    //[nameof(IEntry.MaxLength)] = MapMaxLength,
                    //[nameof(IEntry.Placeholder)] = MapPlaceholder,
                    //[nameof(IEntry.PlaceholderColor)] = MapPlaceholderColor,
                    //[nameof(IEntry.ReturnType)] = MapReturnType,
                    //[nameof(IEntry.Text)] = MapText,
                    //[nameof(IEntry.TextColor)] = MapTextColor,
                    //[nameof(IEntry.CursorPosition)] = MapCursorPosition,
                    //[nameof(IEntry.SelectionLength)] = MapSelectionLength,
            };

        private static void MapText(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView view)
        {
            if (handler.PlatformView != null && handler.VirtualView != null)
            {
                handler.UpdateText();
            }
        }

        public static CommandMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler> CommandMapper = new(ViewCommandMapper)
        {

        };

        static FreakyAutoCompleteViewHandler()
        {

        }

        public FreakyAutoCompleteViewHandler() : base(PropertyMapper)
        {

        }

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
            PlatformView.PlaceholderText = VirtualView.PlaceholderText;
        }

        private void UpdatePlaceholderTextColor()
        {
            var placeholderColor = VirtualView.PlaceholderTextColor;
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