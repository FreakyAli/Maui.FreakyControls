using System.Runtime.CompilerServices;

namespace Maui.FreakyControls;

public class FreakyImage : Image
{
    /// <summary>
    /// Called when image is loaded in your image control's viewport
    /// </summary>
    public event EventHandler ImageLoaded;

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName.Equals(IsLoadingProperty.PropertyName))
        {
            if (IsLoading && Source != null)
                ImageLoaded?.Invoke(this, EventArgs.Empty);
        }
    }
}