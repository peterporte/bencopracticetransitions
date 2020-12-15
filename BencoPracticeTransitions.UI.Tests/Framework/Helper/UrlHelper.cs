using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BencoPracticeTransitions.UI.Tests.Framework.Helper
{
    internal class UrlHelper
    {
        internal static string GetPracticeTransitionsUrl()
        {
            return ConfigurationManager.AppSettings["practicetransitions.benco.com.url"];

        }
    }
}
