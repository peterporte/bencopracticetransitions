using System;
using System.ComponentModel;
using System.Reflection;
using BencoPracticeTransitions.Framework.CustomAttributes;

namespace BencoPracticeTransitions.Framework.Helper
{
    public static class DisplayNameHelper
    {
        private static string GetDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            return atts.Length == 0 ? null : (atts[0] as DisplayNameAttribute)?.DisplayName;
        }

        private static string GetColumnName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(typeof(ColumnNameAttribute), true);
            return atts.Length == 0 ? GetDisplayName(property) : (atts[0] as ColumnNameAttribute)?.ColumnName;
        }

        public static string GetDisplayName(Type type, string propertyName)
        {
            return GetDisplayName(type.GetProperty(propertyName));
        }

        public static string GetColumnName(Type type, string propertyName)
        {
           return GetColumnName(type.GetProperty(propertyName));
        }
    }
}
