using System;
namespace Samples.SliderView;

public class SliderViewModel : MainViewModel
{
    private double label1;

    public double Label1
    {
        get => label1;
        set => SetProperty(ref label1 , value);
    }
}