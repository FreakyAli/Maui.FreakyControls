using System.Collections;
using ArrayList = Java.Util.ArrayList;

namespace Maui.FreakyControls.Platforms.Android
{
    public static  class NativeExtensions
    {
        public static ArrayList ToArrayList(this ICollection input)
        {
            return input.Count == 0 ? new ArrayList() : new ArrayList(input);
        }
    }
}

