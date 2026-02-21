namespace Maui.FreakyControls.Tests.EventArgs;

public class PointsEventArgsTests
{
    [Fact]
    public void Default_PointsIsEmpty()
    {
        var args = new PointsEventArgs();
        Assert.Empty(args.Points);
    }

    [Fact]
    public void Default_PointsIsNotNull()
    {
        var args = new PointsEventArgs();
        Assert.NotNull(args.Points);
    }

    [Fact]
    public void IsEventArgs()
    {
        var args = new PointsEventArgs();
        Assert.IsAssignableFrom<FreakyEventArgs>(args);
    }
}
