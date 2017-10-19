using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class IListExtendsion
    {
        public static List<T> vAdd<T>(this List<T> list, T model)
        {
            list.Add(model);
            return list;
        }


        public static List<T> vRemove<T>(this List<T> list, Func<T, bool> condition)
        {
            if (list.vIsEmpty()) return list;
            var model = list.Where(condition).FirstOrDefault();
            if (model != null) list.Remove(model);
            return list;
        }

        public static List<T> vRemoveRange<T>(this List<T> list, Func<T, bool> condition)
        {
            if (list.vIsEmpty()) return list;
            var models = list.Where(condition);
            foreach (var item in models) list.Remove(item);
            return list;
        }

        public static bool vIsEmpty<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                return true;
            return false;
        }

        public static bool vIsEmpty<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                return true;
            return false;
        }
    }
}
