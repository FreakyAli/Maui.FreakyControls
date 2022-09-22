using Maui.FreakyControls.Platforms.iOS.NativeControls;
using UIKit;

namespace Maui.FreakyControls
{
    public partial class FreakyAutoCompleteViewHandler
    {
        static readonly int baseHeight = 15;

        protected override IOSFreakyAutoBox CreatePlatformView()
        {
            var view = new IOSFreakyAutoBox();
            return view;
        }

        protected override void ConnectHandler(IOSFreakyAutoBox platformView)
        {
            platformView.SuggestionChosen += SuggestionChosen;
            platformView.TextChanged += TextChanged;
            platformView.QuerySubmitted += QuerySubmitted;
            platformView.EditingDidBegin += Control_EditingDidBegin;
            platformView.EditingDidEnd += Control_EditingDidEnd;
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(IOSFreakyAutoBox platformView)
        {
            platformView.EditingDidBegin -= Control_EditingDidBegin;
            platformView.EditingDidEnd -= Control_EditingDidEnd;
            platformView.SuggestionChosen -= SuggestionChosen;
            platformView.TextChanged -= TextChanged;
            platformView.QuerySubmitted -= QuerySubmitted;
            base.DisconnectHandler(platformView);
        }

        private void Control_EditingDidBegin(object sender, EventArgs e)
        {
            VirtualView?.SetValue(VisualElement.IsFocusedPropertyKey, true);
        }

        private void Control_EditingDidEnd(object sender, EventArgs e)
        {
            VirtualView?.SetValue(VisualElement.IsFocusedPropertyKey, false);
        }

        public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            var baseResult = base.GetDesiredSize(widthConstraint, heightConstraint);
            var testString = new Foundation.NSString("Tj");
            var testSize = testString.GetSizeUsingAttributes(new UIStringAttributes { Font = PlatformView.Font });
            double height = baseHeight + testSize.Height;
            height = Math.Round(height);

            return new Size(baseResult.Width, height);
        }

        private void UpdateIsEnabled()
        {
            PlatformView.UserInteractionEnabled = VirtualView.IsEnabled;
        }
    }
}

