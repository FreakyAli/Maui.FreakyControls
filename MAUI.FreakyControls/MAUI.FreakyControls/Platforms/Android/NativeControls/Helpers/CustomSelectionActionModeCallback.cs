using Android.Views;

namespace Maui.FreakyControls.Platforms.Android.NativeControls
{
    internal class CustomSelectionActionModeCallback : Java.Lang.Object, ActionMode.ICallback
    {
        public bool OnActionItemClicked(ActionMode m, IMenuItem i) => false;

        public bool OnCreateActionMode(ActionMode mode, IMenu menu) => false;

        public void OnDestroyActionMode(ActionMode mode)
        { }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu) => true;
    }
}