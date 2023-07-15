namespace Maui.FreakyControls.TouchPress;

internal static class TouchAndPressAnimation
{
    public static void Animate(View view, EventType gestureType)
    {
        var touchAndPressEffectConsumer = view as ITouchPressEffect;

        switch (gestureType)
        {
            case EventType.Pressing:
                SetAnimation(view, touchAndPressEffectConsumer);
                break;
            case EventType.Cancelled:
            case EventType.Released:
                touchAndPressEffectConsumer.ExecuteAction();
                RestoreAnimation(view, touchAndPressEffectConsumer);
                break;
            case EventType.Ignored:
                RestoreAnimation(view, touchAndPressEffectConsumer);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gestureType), gestureType, null);
        }
    }

    private static void SetAnimation(View view, ITouchPressEffect touchAndPressEffectConsumer)
    {
        if (touchAndPressEffectConsumer.Animation != AnimationTypes.None && touchAndPressEffectConsumer.IsEnabled)
        {
            Task.Run(async () =>
            {
                if (touchAndPressEffectConsumer.Animation == AnimationTypes.Fade)
                    await view.FadeTo(0.7, 100);
                else if (touchAndPressEffectConsumer.Animation == AnimationTypes.Scale)
                    await view.ScaleTo(0.95, 100);
                else if (touchAndPressEffectConsumer.Animation == AnimationTypes.FadeAndScale)
                {
                    await Task.WhenAll(view.ScaleTo(0.95, 100), view.FadeTo(0.7, 100));
                }
            });
        }
    }

    private static void RestoreAnimation(View view, ITouchPressEffect touchAndPressEffectConsumer)
    {
        if (touchAndPressEffectConsumer.Animation != AnimationTypes.None)
        {
            Task.Run(async () =>
            {
                if (touchAndPressEffectConsumer.Animation == AnimationTypes.Fade)
                    await view.FadeTo(1, 500);
                else if (touchAndPressEffectConsumer.Animation == AnimationTypes.Scale)
                    await view.ScaleTo(1, 100);
                else if (touchAndPressEffectConsumer.Animation == AnimationTypes.FadeAndScale)
                {
                    await Task.WhenAll(view.ScaleTo(1, 100), view.FadeTo(1, 500));
                }
            });
        }
    }
}