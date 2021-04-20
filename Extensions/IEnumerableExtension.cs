
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.Extensions
{
    public  static class IEnumerableExtension
    {
        // 1. Создаём метод, который по умолчанию будет возвращать тип IEnumerable<SelectListItem>
        // А принимать в качестве параметров дженерик коллекцию IEnumerable и int selectedValue
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)
        {
            // 2. Рассмотрим другой синтаксис запросов LINQ
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString())
                   };
        }
        public static IEnumerable<SelectListItem>ToSelectListItem<T>(this IEnumerable<T>items,string selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectedValue)
                   };
        }
    }
}
