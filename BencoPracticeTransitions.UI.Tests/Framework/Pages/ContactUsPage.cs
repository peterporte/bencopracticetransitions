using Benco.Framework.UI.Tests.Core.Controls;
using Benco.Framework.UI.Tests.Core.Factory;
using BencoPracticeTransitions.UI.Tests.Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BencoPracticeTransitions.UI.Tests.Framework.Pages
{
    class ContactUsPage : Page
    {
        public ContactUsPage()
        {
            BaseUrl = $"{UrlHelper.GetPracticeTransitionsUrl()}/Home/ContactUs";
        }




        public HtmlTextBox FirstNameTextBox => ControlFactory.CreateHtmlTextBoxById("FirstName");
        public HtmlTextBox LastNameTextBox => ControlFactory.CreateHtmlTextBoxById("LastName");
        public HtmlTextBox PhoneNumberTextBox => ControlFactory.CreateHtmlTextBoxById("PhoneNumber");
        public HtmlTextBox EmailAddressTextBox => ControlFactory.CreateHtmlTextBoxById("EmailAddress");
        public HtmlTextBox MessageTextBox => ControlFactory.CreateHtmlTextBoxById("Message");

        public HtmlSelect HowDidYouHearAboutUsSelect => ControlFactory.CreateHtmlSelectById("HowDidYouHearAboutUs");

        public HtmlButton SubmitButton => ControlFactory.CreateHtmlButtonById("submit");

    }
}
