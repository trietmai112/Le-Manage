using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class StringExtendsion
    {
        public static string vReplace(this string model, string oldValue, string newValue)
        {
            if (model == null) return null;
            model = model.Replace(oldValue, newValue);
            return model;
        }

        public static string vReplace(this string model, string newValue, params string[] oldValues)
        {
            foreach(var item in oldValues)
            {
                model.vReplace(item, newValue);
            }
            return model;
        }

        public static string vRegexReplace(this string model, string pattern, string value)
        {
            if (model == null) return null;
            model = Regex.Replace(model, pattern, value);
            return model;
        }
    }
}
