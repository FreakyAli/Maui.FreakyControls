using System;
using Maui.FreakyControls.Platforms.Android.NativeControls;

namespace Maui.FreakyControls
{
    public partial class FreakyAutoCompleteViewHandler
    {
        protected override DroidFreakyAutoBox CreatePlatformView()
        {
            return new DroidFreakyAutoBox(this.Context);
        }

        protected override void ConnectHandler(DroidFreakyAutoBox platformView)
        {
            platformView.SuggestionChosen += SuggestionChosen;
            platformView.TextChanged += TextChanged;
            platformView.QuerySubmitted += QuerySubmitted;
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(DroidFreakyAutoBox platformView)
        {
            base.DisconnectHandler(platformView);
            platformView.SuggestionChosen -= SuggestionChosen;
            platformView.TextChanged -= TextChanged;
            platformView.QuerySubmitted -= QuerySubmitted;
        }

        private void UpdateIsEnabled()
        {
            PlatformView.Enabled = VirtualView.IsEnabled;
        }

        private void UpdateClearButtonVisibility()
        {

        }
    }
}

