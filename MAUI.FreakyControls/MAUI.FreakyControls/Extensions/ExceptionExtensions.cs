using System.Diagnostics;
using System.Text;

namespace Maui.FreakyControls.Extensions;

public static class ExceptionExtensions
{
    public static void TraceException(this Exception exception)
    {
        var stringBuilder = new StringBuilder();
        while (exception is not null)
        {
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(exception.StackTrace);

            exception = exception.InnerException;
        }
        Trace.TraceError(stringBuilder.ToString());
    }
}