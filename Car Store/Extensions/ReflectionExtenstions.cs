using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Store.Extensions
{
    public static class ReflectionExtenstions
    {
        public static string GetPropertyValue<T>(this T Item, string propertyName)
        {
            return Item.GetType().GetProperty(propertyName).GetValue(Item, null).ToString();
        }
    }
}
