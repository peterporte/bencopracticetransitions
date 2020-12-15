using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BencoPracticeTransitions.Framework.Utility
{
    public static class SelectListItemUtility
    {
        public static IEnumerable<SelectListItem> ConvertEnumToSelectList(Type enumType, string selected, bool useIntValue, bool includeBlank)
        {
            var selectList = (from object item in Enum.GetValues(enumType)
                let fi = enumType.GetField(item.ToString())
                let attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault()
                let title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description
                select new SelectListItem
                {
                    Value = useIntValue ? ((int)item).ToString() : item.ToString(),
                    Text = title,
                    Selected = selected == item.ToString()
                }
            );  
            
            if (includeBlank)
            {
                selectList = new[] { new SelectListItem { Selected = false, Text = "", Value = null } }.Union(selectList);
            }

            return selectList;
        }
    }
}