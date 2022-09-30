using System;
namespace Maui.FreakyControls.Shared.Controls;

public class SupportViewBase : View
{
    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create("CornerRadius", typeof(double), typeof(SupportViewBase), 0d);
    public double CornerRadius
    {
        get { return (double)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly BindableProperty CornerWidthProperty = BindableProperty.Create("CornerWidth", typeof(double), typeof(SupportViewBase), 0d);
    public double CornerWidth
    {
        get { return (double)GetValue(CornerWidthProperty); }
        set { SetValue(CornerWidthProperty, value); }
    }

    public static readonly BindableProperty CornerColorProperty = BindableProperty.Create("CornerColor", typeof(Color), typeof(SupportViewBase), Colors.Black);
    public Color CornerColor
    {
        get { return (Color)GetValue(CornerColorProperty); }
        set { SetValue(CornerColorProperty, value); }
    }

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(double), typeof(SupportViewBase), 13d);
    public double FontSize
    {
        get { return (double)GetValue(FontSizeProperty); }
        set { SetValue(FontSizeProperty, value); }
    }

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create("FontFamily", typeof(string), typeof(SupportViewBase),default(string));
    public string FontFamily
    {
        get { return (string)GetValue(FontFamilyProperty); }
        set { SetValue(FontFamilyProperty, value); }
    }

    public static readonly BindableProperty IsFocusProperty = BindableProperty.Create("IsFocus", typeof(bool), typeof(SupportViewBase), false);
    public bool IsFocus
    {
        get { return (bool)GetValue(IsFocusProperty); }
        set { SetValue(IsFocusProperty, value); }
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(SupportViewBase), "", BindingMode.TwoWay);
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public SupportViewBase()
    {
    }
}

