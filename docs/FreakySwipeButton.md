# FreakySwipeButton

A swipe-to-confirm control with fully customisable thumb, track, and fill views.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakySwipeButton SlideCompleteCommand="{Binding ConfirmCommand}">
    <freaky:FreakySwipeButton.Thumb>
        <Border BackgroundColor="White" StrokeShape="RoundRectangle 30" Padding="10">
            <Image Source="arrow_right.png" HeightRequest="24" WidthRequest="24" />
        </Border>
    </freaky:FreakySwipeButton.Thumb>
    <freaky:FreakySwipeButton.TrackBar>
        <Border BackgroundColor="LightGray" StrokeShape="RoundRectangle 30" />
    </freaky:FreakySwipeButton.TrackBar>
    <freaky:FreakySwipeButton.FillBar>
        <Border BackgroundColor="SeaGreen" StrokeShape="RoundRectangle 30" />
    </freaky:FreakySwipeButton.FillBar>
</freaky:FreakySwipeButton>
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Thumb` | `View` | `null` | The draggable handle — any `View` |
| `TrackBar` | `View` | `null` | The static background track — any `View` |
| `FillBar` | `View` | `null` | The progress fill that follows the thumb — any `View` |
| `SlideCompleteCommand` | `ICommand` | `null` | Fired when the thumb reaches the end |

---

## Events

| Event | Description |
| --- | --- |
| `SlideCompleted` | Fires when the swipe gesture reaches 100% |
