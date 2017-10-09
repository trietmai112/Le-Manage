using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class ObjectExtentsion
    {
        public static Dictionary<string, object> vGetDictionary(this object model) 
        {
            if (model == null) return new Dictionary<string, object>();
            var properties = model.GetType().GetProperties();
            var dic = new Dictionary<string, object>();
            foreach(var prop in properties)
            {
                dic.Add(prop.Name, prop.GetValue(model));
            }
            return dic;
        }

        public static IDictionary vAdd(this IDictionary dictionary, object key, object value)
        {
            dictionary.Add(key, value);
            return dictionary;
        }

        public static string vToString(this object model, string defaultValue = "")
        {
            if (model == null) return defaultValue;
            return model.ToString();
        }
    }
}
