using System;

namespace BencoPracticeTransitions.Framework.CustomAttributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class ColumnNameAttribute : Attribute
    {
        public ColumnNameAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        public string ColumnName { get; }
    }
}
