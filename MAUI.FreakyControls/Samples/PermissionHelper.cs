using static Microsoft.Maui.ApplicationModel.Permissions;
namespace Samples;

public static class PermissionHelper
{
    public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(Func<Task<bool>> rationaleTask = null)
        where T : Permissions.BasePermission, new()
    {
        var status = await CheckStatusAsync<T>();
        if (status == PermissionStatus.Granted)
        {
            return status;
        }
#if IOS
        if (status == PermissionStatus.Denied)
        {
            var settingsString = UIKit.UIApplication.OpenSettingsUrlString;
            var url = new Foundation.NSUrl(settingsString);
            var options = new UIKit.UIApplicationOpenUrlOptions
            {
                OpenInPlace = true
            };
            UIKit.UIApplication.SharedApplication.OpenUrl(url, options, null);
            return status;
        }
#endif
        if (Permissions.ShouldShowRationale<T>())
        {
            // Prompt the user with additional information as to why the permission is needed
            if (rationaleTask != null)
            {
                await rationaleTask.Invoke().ConfigureAwait(false);
            }
        }
        status = await Permissions.RequestAsync<T>();
        return status;
    }
}