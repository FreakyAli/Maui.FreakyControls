#if WINDOWS
using Maui.FreakyControls.Enums;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using MauiImageSource = Microsoft.Maui.Controls.ImageSource;
using WinButton = Microsoft.UI.Xaml.Controls.Button;
using WinGrid = Microsoft.UI.Xaml.Controls.Grid;
using WinHorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment;
using WinImage = Microsoft.UI.Xaml.Controls.Image;
using WinSolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;
using WinStretch = Microsoft.UI.Xaml.Media.Stretch;
using WinThickness = Microsoft.UI.Xaml.Thickness;
using WinVerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment;

namespace Maui.FreakyControls.Platforms.Windows.NativeControls;

/// <summary>
/// Shared helper that loads a MAUI ImageSource and injects a transparent icon Button
/// into the root Grid of any WinUI FrameworkElement's control template.
/// </summary>
internal static class WindowsIconInjector
{
    private const string IconTag = "FreakyIconOverlay";

    // Tracks a single pending Loaded handler per platform view so that rapid
    // re-injection (e.g. ImageSource property changes before the view loads)
    // cancels the previous subscription instead of accumulating handlers.
    private static readonly ConditionalWeakTable<FrameworkElement, PendingLoad> _pending = new();

    private sealed class PendingLoad { public RoutedEventHandler? Handler; }

    /// <summary>
    /// Removes any previously injected icon button from the platform view.
    /// Also cancels any pending Loaded subscription for that view.
    /// </summary>
    internal static void Remove(FrameworkElement platformView)
    {
        if (_pending.TryGetValue(platformView, out var cancelState) && cancelState.Handler is not null)
        {
            platformView.Loaded -= cancelState.Handler;
            _pending.Remove(platformView);
        }

        if (platformView.IsLoaded)
            RemoveFromGrid(platformView);
    }

    /// <summary>
    /// Loads the image source, creates the icon button and schedules injection
    /// once the platform view is loaded.
    /// When <paramref name="mauiImageSource"/> is <c>null</c> any previously
    /// injected icon is removed instead.
    /// </summary>
    internal static async Task InjectAsync(
        FrameworkElement platformView,
        MauiImageSource? mauiImageSource,
        ImageAlignment alignment,
        int width,
        int height,
        int padding,
        Action? onTap)
    {
        if (mauiImageSource is null)
        {
            Remove(platformView);
            return;
        }

        var mauiContext = Microsoft.Maui.Controls.Application.Current?.Handler?.MauiContext
            ?? throw new InvalidOperationException("MauiContext is unavailable; ensure the application is fully initialized.");
        var winImageSource = (await mauiImageSource.GetPlatformImageAsync(mauiContext))?.Value;

        if (winImageSource is null)
            return;

        var button = new WinButton
        {
            Tag = IconTag,
            Width = width,
            Height = height,
            Padding = new WinThickness(padding),
            Background = new WinSolidColorBrush(Microsoft.UI.Colors.Transparent),
            BorderThickness = new WinThickness(0),
            VerticalAlignment = WinVerticalAlignment.Center,
            Content = new WinImage
            {
                Source = winImageSource,
                Stretch = WinStretch.Uniform
            }
        };
        button.Click += (_, _) => onTap?.Invoke();

        if (platformView.IsLoaded)
        {
            Inject(platformView, alignment, button, width, height, padding);
        }
        else
        {
            // Cancel any previously queued Loaded handler for this view.
            if (_pending.TryGetValue(platformView, out var existing) && existing.Handler is not null)
                platformView.Loaded -= existing.Handler;

            var pending = new PendingLoad();
            _pending.Remove(platformView);
            _pending.Add(platformView, pending);

            RoutedEventHandler? loadedHandler = null;
            loadedHandler = (_, _) =>
            {
                platformView.Loaded -= loadedHandler;
                _pending.Remove(platformView);
                Inject(platformView, alignment, button, width, height, padding);
            };
            pending.Handler = loadedHandler;
            platformView.Loaded += loadedHandler;
        }
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
        RemoveFromGrid(rootGrid);

        var totalPad = width + padding * 2;

        // FrameworkElement doesn't expose Padding; only Control does.
        if (platformView is Control control)
        {
            switch (alignment)
            {
                case ImageAlignment.Left:
                    control.Padding = new WinThickness(totalPad, 0, 0, 0);
                    iconButton.HorizontalAlignment = WinHorizontalAlignment.Left;
                    break;

                default: // Right
                    control.Padding = new WinThickness(0, 0, totalPad, 0);
                    iconButton.HorizontalAlignment = WinHorizontalAlignment.Right;
                    break;
            }
        }

        rootGrid.Children.Add(iconButton);
    }

    private static void RemoveFromGrid(FrameworkElement platformView)
    {
        if (VisualTreeHelper.GetChildrenCount(platformView) == 0)
            return;
        if (VisualTreeHelper.GetChild(platformView, 0) is WinGrid grid)
            RemoveFromGrid(grid);
    }

    private static void RemoveFromGrid(WinGrid grid)
    {
        var existing = grid.Children
            .OfType<WinButton>()
            .FirstOrDefault(b => b.Tag is string t && t == IconTag);
        if (existing is not null)
            grid.Children.Remove(existing);
    }
}
#endif
