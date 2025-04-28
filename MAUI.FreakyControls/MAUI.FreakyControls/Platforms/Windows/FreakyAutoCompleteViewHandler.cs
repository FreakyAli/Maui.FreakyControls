using Microsoft.Maui.Handlers;
#if WINDOWS
using Microsoft.UI.Xaml.Controls; 
#endif

namespace Maui.FreakyControls;

public partial class FreakyAutoCompleteViewHandler : ViewHandler<FreakyAutoCompleteView, AutoSuggestBox>
{
#if WINDOWS
    protected override AutoSuggestBox CreatePlatformView()
    {
        return new AutoSuggestBox
        {
            IsSuggestionListOpen = false
            // You can configure it further if needed
        };
    }

    protected override void ConnectHandler(AutoSuggestBox platformView)
    {
        base.ConnectHandler(platformView);

        // Hook up events if needed
        platformView.TextChanged += OnTextChanged;
        platformView.SuggestionChosen += OnSuggestionChosen;
        platformView.QuerySubmitted += OnQuerySubmitted;
    }

    protected override void DisconnectHandler(AutoSuggestBox platformView)
    {
        base.DisconnectHandler(platformView);

        // Unhook events to avoid memory leaks
        platformView.TextChanged -= OnTextChanged;
        platformView.SuggestionChosen -= OnSuggestionChosen;
        platformView.QuerySubmitted -= OnQuerySubmitted;
    }

    private void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        // Handle text changed
    }

    private void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        // Handle suggestion chosen
    }

    private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        // Handle query submitted
    }
#endif
}
