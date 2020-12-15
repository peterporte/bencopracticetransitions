using System;
using BencoPracticeTransitions.Framework.Enums;
using BencoPracticeTransitions.Framework.Extensions;

namespace BencoPracticeTransitions.Framework.Helper
{
    public class EnumHelper
    {

        public static string HowDidYouHearAboutUsEnumNameToDescription(string value)
        {
            return Enum.TryParse(value, true, out HowDidYouHearAboutUsEnum result) ? result.Description() : value;
        }
    }
}
