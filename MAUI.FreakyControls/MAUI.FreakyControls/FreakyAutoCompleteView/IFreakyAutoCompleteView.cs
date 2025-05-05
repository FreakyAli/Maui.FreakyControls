using System.Collections;

namespace Maui.FreakyControls;

public interface IFreakyAutoCompleteView : IView, IDrawableImageView
{
    string Text { get; set; }
    Color TextColor { get; set; }
    string Placeholder { get; set; }
    Color PlaceholderColor { get; set; }
    string TextMemberPath { get; set; }
    string DisplayMemberPath { get; set; }
    bool IsSuggestionListOpen { get; set; }
    bool UpdateTextOnSelect { get; set; }
    IList ItemsSource { get; set; }
    int Threshold { get; set; }
    bool AllowCopyPaste { get; set; }
    TextAlignment HorizontalTextAlignment { get; set; }
    TextAlignment VerticalTextAlignment { get; set; }
    string FontFamily { get; set; } 
    double FontSize { get; set; }
    FontAttributes FontAttributes { get; set; }
    double SuggestionListWidth { get; set; }
    double SuggestionListHeight { get; set; }
    void RaiseSuggestionChosen(FreakyAutoCompleteViewSuggestionChosenEventArgs e);
    void NativeControlTextChanged(FreakyAutoCompleteViewTextChangedEventArgs e);
    void RaiseQuerySubmitted(FreakyAutoCompleteViewQuerySubmittedEventArgs e);
}