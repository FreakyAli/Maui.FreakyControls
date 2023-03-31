using System.ComponentModel;
using System.Globalization;

namespace Maui.FreakyControls;

public sealed class SvgImageSourceConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(string);

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        => destinationType == typeof(string);

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        var strValue = value?.ToString();
        if (strValue != null)
            return Uri.TryCreate(strValue, UriKind.Absolute, out Uri uri) && uri.Scheme != "file" ? SvgImageSource.FromUri(uri) : SvgImageSource.FromFile(strValue);

        throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", strValue, typeof(SvgImageSource)));
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (value is FileSvgImageSource fis)
            return fis.File;
        if (value is UriSvgImageSource uis)
            return uis.Uri.ToString();
        throw new NotSupportedException();
    }
}
