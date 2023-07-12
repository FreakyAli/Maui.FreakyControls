namespace Maui.FreakyControls.TouchPress;

public static class TouchAndPressAnimation
{
    public static void Animate(View view, EventType gestureType)
    {
        var touchAndPressEffectConsumer = view as ITouchAndPressEffectConsumer;

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

    private static void SetAnimation(View view, ITouchAndPressEffectConsumer touchAndPressEffectConsumer)
    {
        if (touchAndPressEffectConsumer.Animation != AnimationTypes.None && touchAndPressEffectConsumer.IsEnabled)
        {
            Task.Run(async () =>
            {
                if (touchAndPressEffectConsumer.Animation == AnimationTypes.Fade)
                    await view.FadeTo(touchAndPressEffectConsumer.AnimationParameter.HasValue ? touchAndPressEffectConsumer.AnimationParameter.Value : 0.6, 100);
                else if (touchAndPressEffectConsumer.Animation == AnimationTypes.Scale)
                    await view.ScaleTo(touchAndPressEffectConsumer.AnimationParameter.HasValue ? touchAndPressEffectConsumer.AnimationParameter.Value : 0.95, 100);
                else if (touchAndPressEffectConsumer.Animation == AnimationTypes.Custom && touchAndPressEffectConsumer.CustomAnimation != null)
                    await touchAndPressEffectConsumer.CustomAnimation.SetAnimation(view);
            });
        }
    }

    private static void RestoreAnimation(View view, ITouchAndPressEffectConsumer touchAndPressEffectConsumer)
    {
        if (touchAndPressEffectConsumer.Animation != AnimationTypes.None)
        {
            Task.Run(async () =>
            {
                if (touchAndPressEffectConsumer.Animation == AnimationTypes.Fade)
                    await view.FadeTo(1, 100);
                else if (touchAndPressEffectConsumer.Animation == AnimationTypes.Scale)
                    await view.ScaleTo(1, 100);
                else if (touchAndPressEffectConsumer.Animation == AnimationTypes.Custom && touchAndPressEffectConsumer.CustomAnimation != null)
                    await touchAndPressEffectConsumer.CustomAnimation.RestoreAnimation(view);
            });
        }
    }
}