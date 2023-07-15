using SkiaSharp;

namespace Maui.FreakyControls.Shared.Extensions;

public static class SkPathCheckExtensions
{
    public static void DrawCenteredLine(this SKPath checkPath, SKImageInfo imageInfo)
    {
        checkPath.MoveTo(.2f * imageInfo.Width, .5f * imageInfo.Height);
        checkPath.LineTo(.8f * imageInfo.Width, .5f * imageInfo.Height);
    }

    public static void DrawCross(this SKPath checkPath, SKImageInfo imageInfo)
    {
        checkPath.MoveTo(.70f * imageInfo.Width, .30f * imageInfo.Height);
        checkPath.LineTo(.30f * imageInfo.Width, .70f * imageInfo.Height);
        checkPath.MoveTo(.70f * imageInfo.Width, .70f * imageInfo.Height);
        checkPath.LineTo(.30f * imageInfo.Width, .30f * imageInfo.Height);
    }

    public static void DrawHeart(this SKPath checkPath, SKImageInfo imageInfo)
    {
        checkPath.MoveTo(.5f * imageInfo.Width, .25f * imageInfo.Height);

        checkPath.CubicTo(
            .35f * imageInfo.Width,
             0,
            .1f * imageInfo.Width,
            .1f * imageInfo.Height,
            .1f * imageInfo.Width,
            .3f * imageInfo.Height);

        checkPath.CubicTo(
           .1f * imageInfo.Width,
           .3f * imageInfo.Height,
           .1f * imageInfo.Width,
           .6f * imageInfo.Height,
           .5f * imageInfo.Width,
           .9f * imageInfo.Height);

        checkPath.CubicTo(
           .5f * imageInfo.Width,
           .9f * imageInfo.Height,
           .9f * imageInfo.Width,
           .6f * imageInfo.Height,
           .9f * imageInfo.Width,
           .3f * imageInfo.Height);

        checkPath.CubicTo(
           .9f * imageInfo.Width,
           .1f * imageInfo.Height,
           .65f * imageInfo.Width,
            0,
           .5f * imageInfo.Width,
           .25f * imageInfo.Height);

        checkPath.Close();
    }

    public static void DrawNativeAndroidCheck(this SKPath checkPath, SKImageInfo imageInfo)
    {
        checkPath.MoveTo(.2f * imageInfo.Width, .5f * imageInfo.Height);
        checkPath.LineTo(.425f * imageInfo.Width, .7f * imageInfo.Height);
        checkPath.LineTo(.8f * imageInfo.Width, .275f * imageInfo.Height);
    }

    public static void DrawNativeiOSCheck(this SKPath checkPath, SKImageInfo imageInfo)
    {
        checkPath.MoveTo(.2f * imageInfo.Width, .5f * imageInfo.Height);
        checkPath.LineTo(.375f * imageInfo.Width, .675f * imageInfo.Height);
        checkPath.LineTo(.75f * imageInfo.Width, .3f * imageInfo.Height);
    }

    public static void DrawNativeRadioButtonCheck(this SKPath checkPath, SKImageInfo imageInfo, double radii)
    {
        checkPath.AddCircle(.5f * imageInfo.Width, .5f * imageInfo.Height, (float)radii);
        checkPath.Close();
    }

    public static void DrawSquare(this SKPath checkPath, SKImageInfo imageInfo)
    {
        checkPath.MoveTo(.2f * imageInfo.Width, .8f * imageInfo.Height);
        checkPath.LineTo(.8f * imageInfo.Width, .8f * imageInfo.Height);
        checkPath.LineTo(.8f * imageInfo.Width, .2f * imageInfo.Height);
        checkPath.LineTo(.2f * imageInfo.Width, .2f * imageInfo.Height);
        checkPath.LineTo(.2f * imageInfo.Width, .8f * imageInfo.Height);
        checkPath.Close();
    }

    public static void DrawStar(this SKPath checkPath, SKImageInfo imageInfo)
    {
        float mid = imageInfo.Width / 2;
        float min = Math.Min(imageInfo.Width, imageInfo.Height);
        float half = min / 2;
        mid = mid - half;

        checkPath.MoveTo(mid + half * 0.5f, half * 0.84f);
        checkPath.LineTo(mid + half * 1.5f, half * 0.84f);
        checkPath.LineTo(mid + half * 0.68f, half * 1.45f);
        checkPath.LineTo(mid + half * 1.0f, half * 0.5f);
        checkPath.LineTo(mid + half * 1.32f, half * 1.45f);
        checkPath.LineTo(mid + half * 0.5f, half * 0.84f);

        checkPath.Close();
    }

    public static void DrawUnifiedCheck(this SKPath checkPath, SKImageInfo imageInfo)
    {
        checkPath.MoveTo(.275f * imageInfo.Width, .5f * imageInfo.Height);
        checkPath.LineTo(.425f * imageInfo.Width, .65f * imageInfo.Height);
        checkPath.LineTo(.725f * imageInfo.Width, .375f * imageInfo.Height);
    }
}