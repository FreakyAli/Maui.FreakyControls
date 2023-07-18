using Maui.FreakyControls.Shared.Wrappers;
using System.Diagnostics;

namespace Maui.FreakyControls.Shared.Helpers;

public static class DownloadHelper
{
    internal static async Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var stream = await DownloadStreamAsync(uri, cancellationToken).ConfigureAwait(false);
        return stream;
    }

    private static async Task<Stream> DownloadStreamAsync(Uri uri, CancellationToken cancellationToken)
    {
        try
        {
            using var client = new HttpClient();

            // Do not remove this await otherwise the client will dispose before
            // the stream even starts
            return await StreamWrapper.GetStreamAsync(uri, cancellationToken, client).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Trace.TraceError(ex.Message);
            Trace.TraceError(ex.StackTrace);
            Trace.TraceError(ex.Source);
            Trace.WriteLine(ex.InnerException);
            return null;
        }
    }
}