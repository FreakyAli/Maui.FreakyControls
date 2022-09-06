using System;
using System.Reflection;

namespace Samples
{
	public static class Constants
	{
		public static readonly Assembly SvgAssembly = typeof(Constants).Assembly;
        public static readonly string ResourcePath = "Samples.Resources.Images.";
        public static readonly string DotnetBot = ResourcePath+ "dotnet_bot.svg";
	}
}

