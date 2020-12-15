using Benco.Framework.UI.Tests.Core.Controls;
using Benco.Framework.UI.Tests.Core.Factory;
using BencoPracticeTransitions.UI.Tests.Framework.Helper;

namespace BencoPracticeTransitions.UI.Tests.Framework.Pages
{
    public class PracticeBuyPage : Page
    {
        public PracticeBuyPage()
        {
            BaseUrl = $"{UrlHelper.GetPracticeTransitionsUrl()}/Practice/Buy";
        }

        
        public HtmlTextBox ContactFirstNameTextBox => ControlFactory.CreateHtmlTextBoxById("ContactFirstName");
        public HtmlTextBox ContactLastNameTextBox => ControlFactory.CreateHtmlTextBoxById("ContactLastName");
        public HtmlTextBox ContactPhoneNumberTextBox => ControlFactory.CreateHtmlTextBoxById("ContactPhoneNumber");
        public HtmlTextBox ContactEmailTextBox => ControlFactory.CreateHtmlTextBoxById("ContactEmail");
        public HtmlTextBox PracticeTypeTextBox => ControlFactory.CreateHtmlTextBoxById("PracticeType");
        public HtmlTextBox PracticeLocationTextBox => ControlFactory.CreateHtmlTextBoxById("PracticeLocation");

        public HtmlSelect CollectionAmountSelect => ControlFactory.CreateHtmlSelectById("CollectionAmount");

        public HtmlNumericTextBox MinPurchaseAmountNumber => ControlFactory.CreateHtmlNumericTextBoxById("MinPurchaseAmount");
        public HtmlNumericTextBox MaxPurchaseAmountNumber => ControlFactory.CreateHtmlNumericTextBoxById("MaxPurchaseAmount");
        public HtmlNumericTextBox MinOperatoryCountNumber => ControlFactory.CreateHtmlNumericTextBoxById("MinOperatoryCount");

        public HtmlSelect IsDoctorWillingToStayOnAfterTranstionSelect => ControlFactory.CreateHtmlSelectById("IsDoctorWillingToStayOnAfterTranstion");
        public HtmlSelect RealEstateOptionSelect => ControlFactory.CreateHtmlSelectById("RealEstateOption");

        public HtmlTextBox AdditionalNotesTextBox => ControlFactory.CreateHtmlTextBoxById("AdditionalNotes");

        public HtmlSelect HowDidYouHearAboutUsSelect => ControlFactory.CreateHtmlSelectById("HowDidYouHearAboutUs");




        public HtmlButton SubmitButton => ControlFactory.CreateHtmlButtonById("submit");
    }
}
