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
    }
}
