using System;

namespace Maui.FreakyControls.Shared.Extensions
{
	public static class ServiceHelper
	{
		public static TService GetService<TService>() =>
			Current.GetService<TService>();

		public static IServiceProvider Current =>
#if ANDROID
			MauiApplication.Current.Services;
#endif
#if IOS
			MauiUIApplicationDelegate.Current.Services;
#endif
	}
}

