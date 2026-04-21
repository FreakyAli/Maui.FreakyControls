#if WINDOWS
using Maui.FreakyControls.Enums;
using System.Runtime.CompilerServices;
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
    private static readonly ConditionalWeakTable<FrameworkElement, EpochBox> _epochs = new();

    private sealed class PendingLoad { public RoutedEventHandler? Handler; }

    // Mutable reference box so ConditionalWeakTable can hold a per-view integer.
    private sealed class EpochBox { public int Value; }

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
        // Increment the epoch before any async work so that any in-flight call
        // that resumes after the GetPlatformImageAsync await knows it was superseded.
        var epochBox = _epochs.GetOrCreateValue(platformView);
        int capturedEpoch = ++epochBox.Value;

        if (mauiImageSource is null)
        {
            Remove(platformView);
            return;
        }

        var mauiContext = Microsoft.Maui.Controls.Application.Current?.Handler?.MauiContext
            ?? throw new InvalidOperationException("MauiContext is unavailable; ensure the application is fully initialized.");
        var winImageSource = (await mauiImageSource.GetPlatformImageAsync(mauiContext))?.Value;

        // Bail if a newer InjectAsync call incremented the epoch while we awaited,
        // or if the platform image could not be resolved.
        if (epochBox.Value != capturedEpoch || winImageSource is null)
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
                // Guard against a newer InjectAsync that superseded this one
                // while the view was still loading.
                if (epochBox.Value == capturedEpoch)
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
        var rootGrid = FindFirstGrid(platformView)
            ?? throw new InvalidOperationException(
                $"WindowsIconInjector: no Grid found in the visual tree of {platformView.GetType().Name}. " +
                "The control template must contain a Grid for icon injection to work.");

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
        if (FindFirstGrid(platformView) is WinGrid grid)
            RemoveFromGrid(grid);
    }

    // BFS walk of the visual subtree rooted at <paramref name="root"/> returning
    // the shallowest WinGrid found, or null when none exists.
    private static WinGrid? FindFirstGrid(DependencyObject root)
    {
        var queue = new Queue<DependencyObject>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int count = VisualTreeHelper.GetChildrenCount(current);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(current, i);
                if (child is WinGrid grid)
                    return grid;
                queue.Enqueue(child);
            }
        }
        return null;
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
