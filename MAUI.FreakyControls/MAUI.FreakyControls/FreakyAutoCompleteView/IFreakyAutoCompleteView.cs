#nullable disable

﻿using System.Collections;

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
    double DropDownWidth { get; set; }
    double DropDownHeight { get; set; }
    Color DropDownBorderColor { get; set; }
    double DropDownBorderWidth { get; set; }
    double DropDownCornerRadius { get; set; }

    [Obsolete("Use DropDownWidth instead", false)]
    double SuggestionListWidth { get; set; }

    [Obsolete("Use DropDownHeight instead", false)]
    double SuggestionListHeight { get; set; }

    void RaiseSuggestionChosen(FreakyAutoCompleteViewSuggestionChosenEventArgs e);
    void NativeControlTextChanged(FreakyAutoCompleteViewTextChangedEventArgs e);
    void RaiseQuerySubmitted(FreakyAutoCompleteViewQuerySubmittedEventArgs e);
}