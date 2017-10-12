using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class ObjectExtentsion
    {
        public static IDictionary<string, object> vGetDictionary(this object model) 
        {
            if (model == null) throw new NullReferenceException();
            if (model is Type)
            {
                return vGetDictionary((Type)model, null);
            }
            else
            {
                var type = model.GetType();
                return vGetDictionary(type, model);
            }
        }

        private static IDictionary<string, object> vGetDictionary(Type type, object model)
        {            
            var properties = type.GetProperties();
            if (model == null) model = Activator.CreateInstance(type);
            var dic = new Dictionary<string, object>();
            foreach (var prop in properties)
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
