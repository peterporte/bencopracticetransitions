using System;
using System.ComponentModel;
using System.Linq;

namespace BencoPracticeTransitions.Framework.Extensions
{
    public static class EnumExtensions
    {
        private static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes.FirstOrDefault();
        }

        public static string Description(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}