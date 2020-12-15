using System;
using System.Collections.Generic;
using System.Linq;

namespace BencoPracticeTransitions.Framework.Utility
{
    public static class CsvColumnUtility
    {
        internal static IEnumerable<KeyValuePair<string, string>> CreateEmptyDayOfWeekColumns(string columnNameSuffix)
        {
            return Enumerable.Range(1, 7)
                .Select(dow => new KeyValuePair<string, string>(
                    $"{Enum.GetName(typeof(DayOfWeek), dow % 7).ToString()} {columnNameSuffix}", string.Empty));
        }
    }
}
