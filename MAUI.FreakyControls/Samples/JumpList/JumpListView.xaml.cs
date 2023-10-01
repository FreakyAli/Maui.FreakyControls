using Maui.FreakyControls;
using SkiaSharp.Views.Maui;

namespace Samples.JumpList;

public partial class JumpListView : ContentPage
{
    public static readonly IAlphabetProvider AlphabetProvider
        = new EnglishAlphabetProvider();

    private string CurrentAlphabet = string.Empty;

    public JumpListView()
    {
        InitializeComponent();
        BindingContext = new JumpListViewModel();
    }

    private void SkiaJumpList_OnSelectedCharacterChanged(object sender, FreakyCharacterChangedEventArgs e)
    {
        if (e.SelectedCharacter == CurrentAlphabet)
        {
            return;
        }

        var viewModel = BindingContext as JumpListViewModel;
        var firstItem = viewModel?.Names.FirstOrDefault(x => x.StartsWith(e.SelectedCharacter));
        if (firstItem != null)
        {
            var index = viewModel.Names.IndexOf(firstItem);

            collectionView.ScrollTo(index, position: ScrollToPosition.Start);
        }
        CurrentAlphabet = e.SelectedCharacter;
    }

    private async void JumpList_OnTouch(object sender, SKTouchEventArgs e)
    {
        switch (e.ActionType)
        {
            case SKTouchAction.Pressed:
                jumplistIdentifier.IsVisible = true;
                await Task.WhenAny<bool>
                (
                     jumplistIdentifier.FadeTo(1, length: 500, easing: Easing.BounceIn),
                     jumplistIdentifier.ScaleTo(1, length: 500, easing: Easing.BounceIn)
                );
                break;

            case SKTouchAction.Released:
            case SKTouchAction.Cancelled:
                await Task.WhenAny<bool>
                (
                    jumplistIdentifier.FadeTo(0.7, length: 500, easing: Easing.Linear),
                    jumplistIdentifier.ScaleTo(0.9, length: 500, easing: Easing.Linear)
                );
                jumplistIdentifier.IsVisible = false;
                break;

            default:
            case SKTouchAction.Moved:
            case SKTouchAction.Entered:
            case SKTouchAction.Exited:
            case SKTouchAction.WheelChanged:
                break;
        }
    }
}