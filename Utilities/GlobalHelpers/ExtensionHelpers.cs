using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.GlobalHelpers
{
    public static class ExtensionHelpers
    {
        public static bool IsConvertToInt(this string content)
        {
            var number = default(decimal);
            return decimal.TryParse(content, out number);
        }

        public static bool IsConvertToGuid(this string content)
        {
            var guid = default(Guid);
            return Guid.TryParse(content, out guid);
        }
    }
}
