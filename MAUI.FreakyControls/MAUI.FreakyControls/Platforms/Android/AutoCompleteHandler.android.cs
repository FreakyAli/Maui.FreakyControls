using System;
using System.Drawing;
using Maui.FreakyControls.Platforms.Android.NativeControls;
using Size = Microsoft.Maui.Graphics.Size;

namespace Maui.FreakyControls
{
    public partial class AutoCompleteHandler
    {  
        protected override AndroidAutoSuggestBox CreatePlatformView()
        {
            var autoSuggest = new AndroidAutoSuggestBox(this.Context);
            return autoSuggest;
        }

        protected override void ConnectHandler(AndroidAutoSuggestBox platformView)
        {
            base.ConnectHandler(platformView);
            platformView.SuggestionChosen += AutoSuggestBox_SuggestionChosen;
            platformView.TextChanged += AutoSuggestBox_TextChanged;
            platformView.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
        }

        protected override void DisconnectHandler(AndroidAutoSuggestBox platformView)
        {
            base.DisconnectHandler(platformView);
            platformView.SuggestionChosen -= AutoSuggestBox_SuggestionChosen;
            platformView.TextChanged -= AutoSuggestBox_TextChanged;
            platformView.QuerySubmitted -= AutoSuggestBox_QuerySubmitted;
        }
    }
}

