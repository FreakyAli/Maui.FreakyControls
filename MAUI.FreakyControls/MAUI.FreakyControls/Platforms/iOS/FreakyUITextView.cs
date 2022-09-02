using CoreGraphics;
using Microsoft.Maui.Platform;
using CoreAnimation;

namespace MAUI.FreakyControls
{
    public partial class FreakyEditorHandler
    {
        public class FreakyUITextView : MauiTextView
        {
            public bool IsFrameSet { get; set; }

            public event EventHandler OnFrameSetCompelete;

            public override CGRect Frame
            {
                get => base.Frame;
                set
                {
                    if (Frame.Height > 0 && Frame.Width > 0)
                    {
                        IsFrameSet = true;
                        OnFrameSetCompelete?.Invoke(this, EventArgs.Empty);
                    }
                    base.Frame = value;
                }
            }
        }
    }
}