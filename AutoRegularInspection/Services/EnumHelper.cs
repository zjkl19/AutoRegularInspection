using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public class EnumHelper
    {
        public static string GetEnumDesc(Enum en)
        {
            Type type = en.GetType();
            var memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false); if (attrs != null && attrs.Length > 0) return ((System.ComponentModel.DataAnnotations.DisplayAttribute)attrs[0]).Name;
            }
            return en.ToString();
        }
    }
}
