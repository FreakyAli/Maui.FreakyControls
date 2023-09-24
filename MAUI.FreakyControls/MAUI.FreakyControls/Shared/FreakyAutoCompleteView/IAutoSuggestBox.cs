using Color = Microsoft.Maui.Graphics.Color;

namespace Maui.FreakyControls;

public interface IAutoSuggestBox : IView
{
    string Text { get; set; }
    Color TextColor { get; set; }
    string PlaceholderText { get; set; }
    Color PlaceholderTextColor { get; set; }
    string TextMemberPath { get; set; }
    string DisplayMemberPath { get; set; }
    bool IsSuggestionListOpen { get; set; }
    bool UpdateTextOnSelect { get; set; }
    System.Collections.IList ItemsSource { get; set; }

    void RaiseSuggestionChosen(AutoSuggestBoxSuggestionChosenEventArgs e);
    void NativeControlTextChanged(AutoSuggestBoxTextChangedEventArgs e);
    void RaiseQuerySubmitted(AutoSuggestBoxQuerySubmittedEventArgs e);
}
