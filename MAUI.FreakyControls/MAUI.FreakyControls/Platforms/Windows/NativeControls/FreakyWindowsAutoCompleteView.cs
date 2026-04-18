#if WINDOWS
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinButton = Microsoft.UI.Xaml.Controls.Button;
using WinGrid = Microsoft.UI.Xaml.Controls.Grid;

namespace Maui.FreakyControls.Platforms.Windows.NativeControls;

public class FreakyWindowsAutoCompleteView : WinGrid
{
    internal readonly AutoSuggestBox AutoSuggestBox;
    private WinButton? _iconButton;

    internal FreakyWindowsAutoCompleteView()
    {
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });                        // col 0: left icon
        ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });  // col 1: input
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });                        // col 2: right icon

        AutoSuggestBox = new AutoSuggestBox { PlaceholderText = string.Empty };
        WinGrid.SetColumn(AutoSuggestBox, 1);
        Children.Add(AutoSuggestBox);
    }

    /// <summary>
    /// Attaches an icon button to the left (column 0) or right (column 2) of the AutoSuggestBox.
    /// Pass null imageSource to remove the current icon.
    /// </summary>
    internal void SetIcon(
        Microsoft.UI.Xaml.Media.ImageSource? imageSource,
        int width,
        int height,
        int padding,
        int column,
        Action? onTap)
    {
        if (_iconButton is not null)
        {
            Children.Remove(_iconButton);
            _iconButton = null;
        }

        if (imageSource is null)
            return;

        _iconButton = new WinButton
        {
            Width = width,
            Height = height,
            Padding = new Microsoft.UI.Xaml.Thickness(padding),
            Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
            BorderThickness = new Microsoft.UI.Xaml.Thickness(0),
            VerticalAlignment = VerticalAlignment.Center,
            Content = new Image
            {
                Source = imageSource,
                Stretch = Stretch.Uniform
            }
        };

        _iconButton.Click += (_, _) => onTap?.Invoke();
        WinGrid.SetColumn(_iconButton, column);
        Children.Add(_iconButton);
    }
}
#endif
