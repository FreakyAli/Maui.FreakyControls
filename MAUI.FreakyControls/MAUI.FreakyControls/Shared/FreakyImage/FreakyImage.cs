using System;
using System.Runtime.CompilerServices;

namespace Maui.FreakyControls.Shared.FreakyImage
{
	public class FreakyImage : Image
	{
        public event EventHandler ImageLoaded;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName.Equals(IsLoadingProperty.PropertyName))
            {
                if (IsLoading && Source != null)
                    ImageLoaded?.Invoke(this, null);
            }
        }
    }
}

