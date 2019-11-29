using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Watertight.Util
{
    public static class Utils
    {
        public static Type FindTypeFromString(string TypeName)
        {
            return System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(x => x.FullName == TypeName);
        }
    }
}
