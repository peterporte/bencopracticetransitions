using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benco.Framework.UI.Tests.Core;
using BencoPracticeTransitions.UI.Tests.Framework.Helper;
using Benco.Framework.UI.Tests.Core.Factory;
using Benco.Framework.UI.Tests.Core.Controls;

namespace BencoPracticeTransitions.UI.Tests.Framework.Pages
{
    public class PracticeTransistionsHomePage : Page
    {

        public PracticeTransistionsHomePage()
        {
            BaseUrl = UrlHelper.GetPracticeTransitionsUrl();
        }

        public override bool IsAt => WebDriver.Driver.Url.StartsWith($"{BaseUrl}");

        public HtmlLink SellPracticeLink => ControlFactory.CreateHtmlLinkById("sellAPractice");
        public HtmlLink BuyPracticeLink => ControlFactory.CreateHtmlLinkById("buyAPractice");
        public HtmlLink PostJobLink => ControlFactory.CreateHtmlLinkById("postAJob");
        public HtmlLink FindJobLink => ControlFactory.CreateHtmlLinkById("findAJob");
        public HtmlLink ContactUsLink => ControlFactory.CreateHtmlLinkById("contactUs");

    }
}
