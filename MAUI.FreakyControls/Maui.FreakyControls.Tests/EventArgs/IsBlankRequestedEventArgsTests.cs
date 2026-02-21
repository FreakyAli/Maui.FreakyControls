namespace Maui.FreakyControls.Tests.EventArgs;

public class IsBlankRequestedEventArgsTests
{
    [Fact]
    public void Default_IsBlankIsTrue()
    {
        var args = new IsBlankRequestedEventArgs();
        Assert.True(args.IsBlank);
    }

    [Fact]
    public void IsBlank_CanBeSetToFalse()
    {
        var args = new IsBlankRequestedEventArgs { IsBlank = false };
        Assert.False(args.IsBlank);
    }

    [Fact]
    public void IsEventArgs()
    {
        var args = new IsBlankRequestedEventArgs();
        Assert.IsAssignableFrom<FreakyEventArgs>(args);
    }
}
