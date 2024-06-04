using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Wrappers;

namespace Maui.FreakyControls.Helpers;

public static class DownloadHelper
{
    internal static async Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        Stream stream = await DownloadStreamAsync(uri, cancellationToken).ConfigureAwait(false);
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
            ex.TraceException();
            return null;
        }
    }
}