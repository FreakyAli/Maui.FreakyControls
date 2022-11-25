using System;
namespace Maui.FreakyControls.Extensions;

public static class StreamExtensions
{
    public static MemoryStream GetMemoryStream(this Stream stream)
    {
        MemoryStream memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }
}