namespace Maui.FreakyControls;

public sealed partial class FileSvgImageSource : SvgImageSource
{
    public static readonly BindableProperty FileProperty = BindableProperty.Create(nameof(File), typeof(string), typeof(FileSvgImageSource), default(string));

    public override bool IsEmpty => string.IsNullOrEmpty(File);

    public string File
    {
        get { return (string)GetValue(FileProperty); }
        set { SetValue(FileProperty, value); }
    }

    public override Task<bool> Cancel()
    {
        return Task.FromResult(false);
    }

    public override string ToString()
    {
        return $"File: {File}";
    }

    public static implicit operator FileSvgImageSource(string file)
    {
        return (FileSvgImageSource)FromFile(file);
    }

    public static implicit operator string(FileSvgImageSource file)
    {
        return file != null ? file.File : null;
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        if (propertyName == FileProperty.PropertyName)
            OnSourceChanged();
        base.OnPropertyChanged(propertyName);
    }
}