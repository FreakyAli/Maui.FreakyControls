using Android.Text;
using Java.Lang;


namespace MAUI.FreakyControls.Platforms.Android.NativeControls
{
    internal class TextListener : Java.Lang.Object, ITextWatcher
    {
        public ClearableEditext objClearable { get; set; }
        public TextListener(ClearableEditext objRef)
        {
            objClearable = objRef;
        }

        public void AfterTextChanged(IEditable s)
        {

        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {

        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (objClearable != null)
                objClearable.manageClearButton();
        }
    }
}

