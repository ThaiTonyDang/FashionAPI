using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.GlobalHelpers
{
    public struct DISPLAY
    {
        public const string ENABLE = "ENABLE";
        public const string DISABLE = "DISABLE";
        public const string DEFAULT_IMAGE = "product-defaut.png";
    }

    public struct HTTTP
    {
        public const string SLUG = "resource";
    }

    public struct SIZE
    {
        public const Int32 Width = 500;
        public const Int32 Height = 666;
    }

    public struct CATEGORY
    {
        public const string MEN_FASHION = "MEN FASHION";
        public const string WOMEN_FASHION = "WOMEN FASHION";
        public const string KID_FASHION = "KID FASHION";
    }

    public enum CATEGORY_CODE
    {
        MEN = 1,
        WOMEN = 2,
        KID = 3,
    }
}
