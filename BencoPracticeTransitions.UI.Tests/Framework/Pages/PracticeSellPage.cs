using Benco.Framework.UI.Tests.Core.Controls;
using Benco.Framework.UI.Tests.Core.Factory;
using BencoPracticeTransitions.UI.Tests.Framework.Helper;


namespace BencoPracticeTransitions.UI.Tests.Framework.Pages
{
    public class PracticeSellPage : Page
    {
        public PracticeSellPage()
        {
            BaseUrl = $"{UrlHelper.GetPracticeTransitionsUrl()}/Practice/Sell";
        }

        public HtmlTextBox PracticeNameTextBox => ControlFactory.CreateHtmlTextBoxById("PracticeName");
        public HtmlTextBox CityTextBox => ControlFactory.CreateHtmlTextBoxById("City");
        public HtmlTextBox StateTextBox => ControlFactory.CreateHtmlTextBoxById("State");
        public HtmlTextBox ZipCodeTextBox => ControlFactory.CreateHtmlTextBoxById("ZipCode");
        public HtmlTextBox ContactFirstNameTextBox => ControlFactory.CreateHtmlTextBoxById("ContactFirstName");
        public HtmlTextBox ContactLastNameTextBox => ControlFactory.CreateHtmlTextBoxById("ContactLastName");
        public HtmlTextBox ContactPhoneTextBox => ControlFactory.CreateHtmlTextBoxById("ContactPhone");
        public HtmlTextBox ContactEmailTextBox => ControlFactory.CreateHtmlTextBoxById("ContactEmail");
        public HtmlNumericTextBox AskingPriceTextBox => ControlFactory.CreateHtmlNumericTextBoxById("AskingPrice");

        public HtmlSelect IsAppraisalNeededSelect => ControlFactory.CreateHtmlSelectById("IsAppraisalNeeded");
        public HtmlSelect CollectionAmountSelect => ControlFactory.CreateHtmlSelectById("CollectionAmount");
        public HtmlSelect RealEstateOptionSelect => ControlFactory.CreateHtmlSelectById("RealEstateOption");

        public HtmlNumericTextBox WorkingOperatoryCountNumber => ControlFactory.CreateHtmlNumericTextBoxById("WorkingOperatoryCount");
        public HtmlNumericTextBox ExpandableOperatoryCountNumber => ControlFactory.CreateHtmlNumericTextBoxById("ExpandableOperatoryCount");
       


        public HtmlSelect IsDoctorWillingToStayOnAfterTransitionSelect => ControlFactory.CreateHtmlSelectById("IsDoctorWillingToStayOnAfterTransition");

        public HtmlTextBox AdditionalNotesTextBox => ControlFactory.CreateHtmlTextBoxById("AdditionalNotes");

        public HtmlSelect HowDidYouHearAboutUsSelect => ControlFactory.CreateHtmlSelectById("HowDidYouHearAboutUs");

        public HtmlButton SubmitButton => ControlFactory.CreateHtmlButtonById("submit");
    }
}
