#if WINDOWS
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinButton = Microsoft.UI.Xaml.Controls.Button;
using WinColumnDefinition = Microsoft.UI.Xaml.Controls.ColumnDefinition;
using WinGrid = Microsoft.UI.Xaml.Controls.Grid;
using WinGridLength = Microsoft.UI.Xaml.GridLength;
using WinGridUnitType = Microsoft.UI.Xaml.GridUnitType;
using WinImage = Microsoft.UI.Xaml.Controls.Image;
using WinSolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;
using WinStretch = Microsoft.UI.Xaml.Media.Stretch;
using WinVerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment;

namespace Maui.FreakyControls.Platforms.Windows.NativeControls;

public class FreakyWindowsAutoCompleteView : WinGrid
{
    internal readonly AutoSuggestBox AutoSuggestBox;
    private WinButton? _iconButton;

    internal FreakyWindowsAutoCompleteView()
    {
        ColumnDefinitions.Add(new WinColumnDefinition { Width = WinGridLength.Auto });
        ColumnDefinitions.Add(new WinColumnDefinition { Width = new WinGridLength(1, WinGridUnitType.Star) });
        ColumnDefinitions.Add(new WinColumnDefinition { Width = WinGridLength.Auto });

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
            Background = new WinSolidColorBrush(Microsoft.UI.Colors.Transparent),
            BorderThickness = new Microsoft.UI.Xaml.Thickness(0),
            VerticalAlignment = WinVerticalAlignment.Center,
            Content = new WinImage
            {
                Source = imageSource,
                Stretch = WinStretch.Uniform
            }
        };

        _iconButton.Click += (_, _) => onTap?.Invoke();
        WinGrid.SetColumn(_iconButton, column);
        Children.Add(_iconButton);
    }
}
#endif
