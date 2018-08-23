using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PContextus.Infrastructure.MongoDb
{
    public class Constants
    {
        public struct Operators
        {
            public static string And = "And";

            public static string Or = "Or";

        }

        public struct Comparator
        {
            public static string NotEqual = "NotEqual";

            public static string Equal = "Equal";

            public static string GreaterThan = "GreaterThan";

            public static string LessThan = "LessThan";

            public static string GreaterEqualTo = "GreaterEqualTo";

            public static string LessEqualTo = "LessEqualTo";

            public static string CompareTo = "CompareTo";
        }
    }
}
