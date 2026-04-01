# FreakyCodeView / FreakyPinCodeControl

`FreakyCodeView` is an inline OTP / PIN entry field. `FreakyPinCodeControl` extends it with a built-in custom numeric keyboard.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<!-- Inline OTP field -->
<freaky:FreakyCodeView
    CodeLength="6"
    CodeValue="{Binding OtpCode, Mode=TwoWay}"
    IsPassword="true"
    Color="Black"
    ItemShape="Circle"
    CodeEntryCompletedCommand="{Binding VerifyCommand}" />

<!-- Full PIN entry with custom keyboard -->
<freaky:FreakyPinCodeControl
    CodeLength="4"
    CodeValue="{Binding Pin, Mode=TwoWay}"
    IsPassword="true"
    ShouldShowCancelButton="true"
    CodeEntryCompletedCommand="{Binding ConfirmCommand}" />
```

---

## FreakyCodeView Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `CodeValue` | `string` | `""` | TwoWay — current entered code |
| `CodeLength` | `int` | `4` | Number of input slots |
| `CodeInputType` | `KeyboardType` | `Numeric` | Keyboard type shown |
| `IsPassword` | `bool` | `true` | Mask digits with dots |
| `Color` | `Color` | `Colors.Black` | Default item colour |
| `ItemShape` | `ItemShape` | `Circle` | `Circle` or `Square` |
| `ItemSize` | `double` | `50` | Size of each item slot |
| `ItemSpacing` | `double` | `5` | Space between slots |
| `ItemBorderColor` | `Color` | `Colors.Black` | |
| `ItemBorderWidth` | `double` | `5` | |
| `ItemBackgroundColor` | `Color` | default | |
| `ItemFocusColor` | `Color` | `Colors.Black` | Colour of the focused slot |
| `ItemFocusAnimation` | `FocusAnimation` | default | Animation style on focus |
| `FontSize` | `double` | `ItemSize / 2` | |
| `FontFamily` | `string` | `null` | |
| `ShouldAutoDismissKeyboard` | `bool` | `true` | Dismiss keyboard after last digit |
| `CodeEntryCompletedCommand` | `ICommand` | `null` | |

### FreakyCodeView Events

| Event | Args | Description |
| --- | --- | --- |
| `CodeEntryCompleted` | `FreakyCodeCompletedEventArgs` | Fires when all digits are entered |

---

## Additional FreakyPinCodeControl Properties

All `FreakyCodeView` properties apply, plus:

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `ShouldShowCancelButton` | `bool` | `true` | Show/hide the cancel key |
| `CancelButtonText` | `string` | `"Cancel"` | Cancel key label |
| `CancelButtonImageSource` | `ImageSource` | `null` | Image for cancel key (replaces text) |
| `BackspaceButtonSource` | `ImageSource` | `null` | Image for backspace key |
| `KeyboardBackgroundColor` | `Color` | `Colors.White` | |
| `KeyboardTextColor` | `Color` | `Colors.Black` | |
| `KeyboardButtonCornerRadius` | `int` | `10` | |
| `KeyboardButtonHeightRequest` | `double` | default | |
| `KeyboardButtonWidthRequest` | `double` | default | |
| `KeyboardSpacing` | `double` | `10` | Spacing between keyboard rows |
| `CancelBackgroundColor` | `Color` | `Colors.White` | |
| `BackspaceBackgroundColor` | `Color` | `Colors.White` | |
| `CancelButtonPadding` | `Thickness` | `20` | |
| `CancelFontSize` | `double` | inherited | |

### FreakyPinCodeControl Events

| Event | Description |
| --- | --- |
| `CodeEntryCompleted` | Fires when all digits are entered |
| `KeyboardClicked` | Fires on each key tap |
| `CancelClicked` | Fires when the cancel key is tapped |
| `BackSpaceClicked` | Fires when backspace is tapped |
