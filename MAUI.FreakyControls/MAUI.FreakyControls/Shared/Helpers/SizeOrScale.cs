namespace Maui.FreakyControls;

public struct SizeOrScale
{
    public SizeOrScale(float xy, SizeOrScaleType type)
    {
        X = xy;
        Y = xy;
        Type = type;
        KeepAspectRatio = true;
    }

    public SizeOrScale(float xy, SizeOrScaleType type, bool keepAspectRatio)
    {
        X = xy;
        Y = xy;
        Type = type;
        KeepAspectRatio = keepAspectRatio;
    }

    public SizeOrScale(Size size, SizeOrScaleType type)
    {
        X = (float)size.Width;
        Y = (float)size.Height;
        Type = type;
        KeepAspectRatio = true;
    }

    public SizeOrScale(Size size, SizeOrScaleType type, bool keepAspectRatio)
    {
        X = (float)size.Width;
        Y = (float)size.Height;
        Type = type;
        KeepAspectRatio = keepAspectRatio;
    }

    public SizeOrScale(float x, float y, SizeOrScaleType type)
    {
        X = x;
        Y = y;
        Type = type;
        KeepAspectRatio = true;
    }

    public SizeOrScale(float x, float y, SizeOrScaleType type, bool keepAspectRatio)
    {
        X = x;
        Y = y;
        Type = type;
        KeepAspectRatio = keepAspectRatio;
    }

    public float X { get; set; }

    public float Y { get; set; }

    public SizeOrScaleType Type { get; set; }

    public bool KeepAspectRatio { get; set; }

    public bool IsValid => X > 0 && Y > 0;

    public Size GetScale(float width, float height)
    {
        if (Type == SizeOrScaleType.Scale)
            return new Size(X, Y);
        return new Size(X / width, Y / height);
    }

    public Size GetSize(float width, float height)
    {
        if (Type == SizeOrScaleType.Scale)
            return new Size(width * X, height * Y);
        return new Size(X, Y);
    }

    public static implicit operator SizeOrScale(float scale)
    {
        return new SizeOrScale(scale, SizeOrScaleType.Scale);
    }

    public static implicit operator SizeOrScale(Size size)
    {
        return new SizeOrScale((float)size.Width, (float)size.Height, SizeOrScaleType.Size);
    }
}