# FreakyAutoCompleteView

A text field with a live suggestion dropdown. Supports custom item display/text member paths, a configurable activation threshold, and an optional side image with a tap command.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyAutoCompleteView
    Text="{Binding SearchText}"
    Placeholder="Search..."
    ItemsSource="{Binding Suggestions}"
    DisplayMemberPath="Name"
    TextMemberPath="Name"
    Threshold="1"
    TextChanged="OnTextChanged"
    QuerySubmitted="OnQuerySubmitted"
    SuggestionChosen="OnSuggestionChosen" />
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | Current input text |
| `Placeholder` | `string` | `""` | Placeholder text |
| `PlaceholderColor` | `Color` | `Colors.Gray` | Placeholder text colour |
| `TextColor` | `Color` | `Colors.Gray` | Input text colour |
| `TextMemberPath` | `string` | `""` | Property path used to populate the field when a suggestion is chosen |
| `DisplayMemberPath` | `string` | `""` | Property path used to display items in the suggestion list |
| `ItemsSource` | `IList` | `null` | Collection of suggestion items |
| `Threshold` | `int` | `1` | Minimum characters typed before suggestions appear |
| `IsSuggestionListOpen` | `bool` | `false` | Programmatically show or hide the suggestion list |
| `UpdateTextOnSelect` | `bool` | `true` | Whether selecting a suggestion updates the text field |
| `AllowCopyPaste` | `bool` | `false` | Enable or disable copy/paste context menu |
| `HorizontalTextAlignment` | `TextAlignment` | `Start` | |
| `VerticalTextAlignment` | `TextAlignment` | `Center` | |
| `FontFamily` | `string` | `null` | |
| `FontSize` | `double` | `14` | |
| `FontAttributes` | `FontAttributes` | `None` | |
| `TextTransform` | `TextTransform` | `None` | Apply uppercase, lowercase, or default transform to input text |
| `ImageSource` | `ImageSource` | `null` | Optional side icon |
| `ImageHeight` | `int` | `25` | |
| `ImageWidth` | `int` | `25` | |
| `ImageAlignment` | `ImageAlignment` | `Right` | `Left` or `Right` |
| `ImagePadding` | `int` | `5` | |
| `ImageCommand` | `ICommand` | `null` | Command fired when the icon is tapped |
| `ImageCommandParameter` | `object` | `null` | |
| `DropDownWidth` | `double` | `0` | Override dropdown width (`0` = match input width). *Not supported on Windows* |
| `DropDownHeight` | `double` | `0` | Override dropdown height (`0` = dynamic/wrap content). *Not supported on Windows* |
| `DropDownBorderColor` | `Color` | `Colors.Black` | Border colour of the dropdown list. *Not supported on Windows (WinUI 3 limitation)* |
| `DropDownBorderWidth` | `double` | `1.0` | Border width of the dropdown list (in DIPs). *Not supported on Windows* |
| `DropDownCornerRadius` | `double` | `0.0` | Corner radius of the dropdown list (in DIPs). *Not supported on Windows* |
| `SuggestionListWidth` | `double` | `0` | **Obsolete** — Use `DropDownWidth` instead |
| `SuggestionListHeight` | `double` | `0` | **Obsolete** — Use `DropDownHeight` instead |

---

## Events

| Event | Args | Description |
| --- | --- | --- |
| `TextChanged` | `FreakyAutoCompleteViewTextChangedEventArgs` | Fires on user input or programmatic text change. `Reason` indicates `UserInput`, `ProgrammaticChange`, or `SuggestionChosen` |
| `QuerySubmitted` | `FreakyAutoCompleteViewQuerySubmittedEventArgs` | Fires when the user presses return or selects a suggestion |
| `SuggestionChosen` | `FreakyAutoCompleteViewSuggestionChosenEventArgs` | Fires when a suggestion row is tapped |

---

## Platform Limitations

### Windows (WinUI 3)

The native `AutoSuggestBox` control in WinUI 3 has limitations that affect dropdown customization:

- **Dropdown Sizing** — `DropDownWidth` and `DropDownHeight` cannot be customized; the underlying `AutoSuggestBox` dropdown sizing is not exposed via public APIs
- **Dropdown Styling** — `DropDownBorderColor`, `DropDownBorderWidth`, and `DropDownCornerRadius` are not supported; the internal popup control is not directly accessible for customization

These are platform limitations imposed by the WinUI 3 framework and cannot be worked around in the control implementation.
