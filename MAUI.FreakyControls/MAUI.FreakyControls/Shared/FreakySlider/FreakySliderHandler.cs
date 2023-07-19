using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;

#if ANDROID || IOS
public partial class FreakySliderHandler : SliderHandler
{
    public FreakySliderHandler()
    {
        Mapper.AppendToMapping("FreakySliderCustomization", MapFreakySlider);
    }

    private void MapFreakySlider(ISliderHandler handler, ISlider slider)
    {
        if (slider is FreakySlider freakySlider && handler is FreakySliderHandler sliderHandler)
        {
            //ApplyBounds(sliderHandler);
            HandleSliderChanges(sliderHandler);
        }
    }
}
#else
public class FreakySliderHandler : FreakySlider
{

}
#endif