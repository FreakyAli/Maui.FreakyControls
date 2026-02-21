namespace Maui.FreakyControls.Tests.EventArgs;

public class FreakyRadioButtonEventArgsTests
{
    [Fact]
    public void Constructor_SetsNameAndIndex()
    {
        var args = new FreakyRadioButtonEventArgs("Option A", 0);
        Assert.Equal("Option A", args.RadioButtonName);
        Assert.Equal(0, args.RadioButtonIndex);
    }

    [Theory]
    [InlineData("First", 0)]
    [InlineData("Second", 1)]
    [InlineData("Last", 99)]
    public void Constructor_StoresProvidedValues(string name, int index)
    {
        var args = new FreakyRadioButtonEventArgs(name, index);
        Assert.Equal(name, args.RadioButtonName);
        Assert.Equal(index, args.RadioButtonIndex);
    }

    [Fact]
    public void Constructor_AllowsNullName()
    {
        var args = new FreakyRadioButtonEventArgs(null, 0);
        Assert.Null(args.RadioButtonName);
    }

    [Fact]
    public void IsEventArgs()
    {
        var args = new FreakyRadioButtonEventArgs("A", 0);
        Assert.IsAssignableFrom<System.EventArgs>(args);
    }
}
