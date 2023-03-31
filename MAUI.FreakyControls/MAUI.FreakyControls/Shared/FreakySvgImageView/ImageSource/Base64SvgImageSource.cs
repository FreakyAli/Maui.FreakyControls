using System;
namespace Maui.FreakyControls;

public class Base64SvgImageSource : SvgImageSource
{
    public static readonly BindableProperty Base64Property = BindableProperty.Create(nameof(Base64), typeof(string), typeof(FileSvgImageSource), default(string));

    public override bool IsEmpty => string.IsNullOrEmpty(Base64);

    public string Base64
    {
        get { return (string)GetValue(Base64Property); }
        set { SetValue(Base64Property, value); }
    }

    public override Task<bool> Cancel()
    {
        return Task.FromResult(false);
    }

    public override string ToString()
    {
        return $"Base64: {Base64}";
    }

    public static implicit operator Base64SvgImageSource(string base64)
    {
        return (Base64SvgImageSource)FromBase64(base64);
    }

    public static implicit operator string(Base64SvgImageSource source)
    {
        return source != null ? source.Base64 : null;
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        if (propertyName == Base64Property.PropertyName)
            OnSourceChanged();
        base.OnPropertyChanged(propertyName);
    }
}

