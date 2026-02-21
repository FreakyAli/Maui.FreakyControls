using Maui.FreakyControls.Extensions;

namespace Maui.FreakyControls.Tests.Extensions;

public class AssemblyNotFoundExceptionTests
{
    [Fact]
    public void Constructor_SetsMessage()
    {
        var ex = new AssemblyNotFoundException("MyAssembly not found");
        Assert.Equal("MyAssembly not found", ex.Message);
    }

    [Fact]
    public void IsException()
    {
        var ex = new AssemblyNotFoundException("msg");
        Assert.IsType<Exception>(ex, exactMatch: false);
    }

    [Fact]
    public async Task CanBeThrownAndCaught()
    {
        await Assert.ThrowsAsync<AssemblyNotFoundException>(() =>
            throw new AssemblyNotFoundException("missing"));
    }
}
