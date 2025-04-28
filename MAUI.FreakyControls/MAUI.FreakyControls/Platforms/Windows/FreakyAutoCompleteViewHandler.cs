using Microsoft.Maui.Handlers;
#if WINDOWS
using Microsoft.UI.Xaml.Controls;
#endif

namespace Maui.FreakyControls
{
    public partial class FreakyAutoCompleteViewHandler : ViewHandler<FreakyAutoCompleteView, AutoSuggestBox>
    {
        public static IPropertyMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler> Mapper =
            new PropertyMapper<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler>(ViewHandler.ViewMapper);

        public FreakyAutoCompleteViewHandler() : base(Mapper)
        {
        }

#if WINDOWS
        protected override AutoSuggestBox CreatePlatformView()
        {
            return new AutoSuggestBox
            {
                PlaceholderText = "Type here...",
                IsSuggestionListOpen = false
            };
        }

        protected override void ConnectHandler(AutoSuggestBox platformView)
        {
            base.ConnectHandler(platformView);

            platformView.TextChanged += OnTextChanged;
            platformView.SuggestionChosen += OnSuggestionChosen;
            platformView.QuerySubmitted += OnQuerySubmitted;
        }

        protected override void DisconnectHandler(AutoSuggestBox platformView)
        {
            base.DisconnectHandler(platformView);

            platformView.TextChanged -= OnTextChanged;
            platformView.SuggestionChosen -= OnSuggestionChosen;
            platformView.QuerySubmitted -= OnQuerySubmitted;
        }

        private void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // TODO: Handle text change
        }

        private void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // TODO: Handle suggestion chosen
        }

        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            // TODO: Handle query submitted
        }
#endif
    }
}