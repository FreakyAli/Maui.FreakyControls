using System;
using System.Reflection;

namespace Samples
{
	public static class Constants
	{
		public static readonly Assembly SvgAssembly = typeof(Constants).Assembly;
        public static readonly string ResourcePath = "Samples.Resources.Images.";
        public static readonly string DotnetBot = ResourcePath+ "dotnet_bot.svg";
		public static readonly string Profile = "https://images.pexels.com/photos/1704488/pexels-photo-1704488.jpeg?cs=srgb&dl=pexels-suliman-sallehi-1704488.jpg&fm=jpg";
		public static readonly string Profile2 = "https://i.stack.imgur.com/HOUoK.jpg?s=256&g=1";
	}
}

