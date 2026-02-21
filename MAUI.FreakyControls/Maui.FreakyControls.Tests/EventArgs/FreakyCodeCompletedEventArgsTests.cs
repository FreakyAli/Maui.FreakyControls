namespace Maui.FreakyControls.Tests.EventArgs;

public class FreakyCodeCompletedEventArgsTests
{
    [Fact]
    public void Constructor_SetsCode()
    {
        var args = new FreakyCodeCompletedEventArgs("1234");
        Assert.Equal("1234", args.Code);
    }

    [Fact]
    public void Constructor_AllowsNullCode()
    {
        var args = new FreakyCodeCompletedEventArgs(null);
        Assert.Null(args.Code);
    }

    [Fact]
    public void Constructor_AllowsEmptyCode()
    {
        var args = new FreakyCodeCompletedEventArgs(string.Empty);
        Assert.Equal(string.Empty, args.Code);
    }

    [Fact]
    public void IsEventArgs()
    {
        var args = new FreakyCodeCompletedEventArgs("0000");
        Assert.IsAssignableFrom<System.EventArgs>(args);
    }
}
