using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class IDictionaryExtendsion
    {
        private class _temple
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public static IDictionary<string, object> vMerge(this IDictionary<string, object> itemOne, params object[] param)
        {
            if (param == null && param.Length == 0) return itemOne;
            List<IDictionary<string, object>> dicArray = new List<IDictionary<string, object>>();
            foreach (var item in param) dicArray.Add(HtmlHelper.ObjectToDictionary(item));
            return vMerge(itemOne, dicArray.ToArray());
        }

        public static IDictionary<string, object> vMerge(this IDictionary<string, object> itemOne, params IDictionary<string, object>[] dicArray)
        {

            if (dicArray == null && dicArray.Length == 0)
                return itemOne;
            if (itemOne == null) itemOne = new Dictionary<string, object>();

            var liItemOne = itemOne.Select(m => new _temple { Key = m.Key.vToString(), Value = m.Value.vToString("").ToLower() }).ToList();
            foreach(var item in dicArray)
            {
                foreach(var key in item.Keys)
                {
                    var liTemp = liItemOne.FirstOrDefault(m=> string.Equals(m.Key.ToString(), key.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if(liTemp == null)
                    {
                        liItemOne.Add(new _temple { Key = key.vToString(), Value = item[key].vToString("").ToLower() });
                    }
                    else
                    {
                        liTemp.Value += item[key].vToString("").ToLower();
                    }
                }

            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            liItemOne.ForEach(m => result.Add(m.Key, m.Value));
            return result;
        }
    }
}
