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
            if(TypeName == null)
            {
                return null;
            }
            return System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(x => x.FullName == TypeName);
        }


        static Dictionary<string, int> NameTable = new Dictionary<string, int>();
        public static string GetNewNameForObject(object obj)
        {
            string TypeName = obj.GetType().Name;

            int val = NameTable.GetValueOrDefault(TypeName);
            val++;
            NameTable[TypeName] = val;

            return string.Format("{0}_{1}", TypeName, val);
        }
    }
}
