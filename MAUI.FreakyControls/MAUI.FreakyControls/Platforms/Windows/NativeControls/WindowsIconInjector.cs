#if WINDOWS
using Maui.FreakyControls.Enums;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using MauiImageSource = Microsoft.Maui.Controls.ImageSource;
using WinButton = Microsoft.UI.Xaml.Controls.Button;
using WinGrid = Microsoft.UI.Xaml.Controls.Grid;

namespace Maui.FreakyControls.Platforms.Windows.NativeControls;

/// <summary>
/// Shared helper that loads a MAUI ImageSource and injects a transparent icon Button
/// into the root Grid of any WinUI FrameworkElement's control template.
/// </summary>
internal static class WindowsIconInjector
{
    private const string IconTag = "FreakyIconOverlay";

    /// <summary>
    /// Loads the image source, creates the icon button and schedules injection
    /// once the platform view is loaded.
    /// </summary>
    internal static async Task InjectAsync(
        FrameworkElement platformView,
        MauiImageSource mauiImageSource,
        ImageAlignment alignment,
        int width,
        int height,
        int padding,
        Action onTap)
    {
        var services = IPlatformApplication.Current!.Services;
        var provider = services.GetRequiredService<IImageSourceServiceProvider>();
        var service = provider.GetImageSourceService(mauiImageSource);
        var result = await service.GetImageAsync(mauiImageSource);
        var winImageSource = result?.Value;

        if (winImageSource is null)
            return;

        var button = new WinButton
        {
            Tag = IconTag,
            Width = width,
            Height = height,
            Padding = new Thickness(padding),
            Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
            BorderThickness = new Thickness(0),
            VerticalAlignment = VerticalAlignment.Center,
            Content = new Image
            {
                Source = winImageSource,
                Stretch = Stretch.Uniform
            }
        };
        button.Click += (_, _) => onTap?.Invoke();

        if (platformView.IsLoaded)
            Inject(platformView, alignment, button, width, height, padding);
        else
            platformView.Loaded += (_, _) => Inject(platformView, alignment, button, width, height, padding);
    }

    private static void Inject(
        FrameworkElement platformView,
        ImageAlignment alignment,
        WinButton iconButton,
        int width,
        int height,
        int padding)
    {
        if (VisualTreeHelper.GetChildrenCount(platformView) == 0)
            return;

        var rootGrid = VisualTreeHelper.GetChild(platformView, 0) as WinGrid;
        if (rootGrid is null)
            return;

        // Remove any previously injected button so re-mapping is idempotent.
        var existing = rootGrid.Children
            .OfType<WinButton>()
            .FirstOrDefault(b => b.Tag is string t && t == IconTag);
        if (existing is not null)
            rootGrid.Children.Remove(existing);

        var totalPad = Math.Max(width, height) + padding * 2;

        switch (alignment)
        {
            case ImageAlignment.Left:
                platformView.Padding = new Thickness(totalPad, 0, 0, 0);
                iconButton.HorizontalAlignment = HorizontalAlignment.Left;
                break;

            default: // Right
                platformView.Padding = new Thickness(0, 0, totalPad, 0);
                iconButton.HorizontalAlignment = HorizontalAlignment.Right;
                break;
        }

        rootGrid.Children.Add(iconButton);
    }
}
#endif
