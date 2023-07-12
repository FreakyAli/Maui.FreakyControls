namespace Maui.FreakyControls.TouchPress;

public interface ICustomAnimation
{
    Task SetAnimation(View view);

    Task RestoreAnimation(View view);
}