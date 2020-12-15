using Benco.Framework.UI.Tests.Core.Controls;
using BencoPracticeTransitions.UI.Tests.Framework.Helper;
using Benco.Framework.UI.Tests.Core.Factory;

namespace BencoPracticeTransitions.UI.Tests.Framework.Pages
{
    class JobListingCreatePage : Page
    {
        public JobListingCreatePage()
        {
            BaseUrl = $"{UrlHelper.GetPracticeTransitionsUrl()}/JobListing/Create";
        }

        public HtmlTextBox PracticeNameTextBox => ControlFactory.CreateHtmlTextBoxById("PracticeName");
        public HtmlTextBox PracticeLocationTextBox => ControlFactory.CreateHtmlTextBoxById("PracticeLocation");
        public HtmlTextBox ContactFirstNameTextBox => ControlFactory.CreateHtmlTextBoxById("ContactFirstName");
        public HtmlTextBox ContactLastNameTextBox => ControlFactory.CreateHtmlTextBoxById("ContactLastName");
        public HtmlTextBox ContactNumberTextBox => ControlFactory.CreateHtmlTextBoxById("ContactNumber");
        public HtmlTextBox ContactEmailTextBox => ControlFactory.CreateHtmlTextBoxById("ContactEmail");

        public HtmlSelect JobTypeSelect => ControlFactory.CreateHtmlSelectById("JobType");


        public HtmlCheckbox JobHours_0__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("JobHours_0__Checked");
        public HtmlTextBox JobHours_0__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("JobHours_0__Hours");
        public HtmlCheckbox JobHours_1__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("JobHours_1__Checked");
        public HtmlTextBox JobHours_1__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("JobHours_1__Hours");
        public HtmlCheckbox JobHours_2__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("JobHours_2__Checked");
        public HtmlTextBox JobHours_2__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("JobHours_2__Hours");
        public HtmlCheckbox JobHours_3__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("JobHours_3__Checked");
        public HtmlTextBox JobHours_3__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("JobHours_3__Hours");
        public HtmlCheckbox JobHours_4__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("JobHours_4__Checked");
        public HtmlTextBox JobHours_4__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("JobHours_4__Hours");
        public HtmlCheckbox JobHours_5__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("JobHours_5__Checked");
        public HtmlTextBox JobHours_5__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("JobHours_5__Hours");
        public HtmlCheckbox JobHours_6__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("JobHours_6__Checked");
        public HtmlTextBox JobHours_6__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("JobHours_6__Hours");


        public HtmlTextBox JobRequirementsTextBox => ControlFactory.CreateHtmlTextBoxById("JobRequirements");
        public HtmlTextBox LinkedInAccountTextBox => ControlFactory.CreateHtmlTextBoxById("LinkedInAccount");
        public HtmlTextBox AdditionalNotesTextBox => ControlFactory.CreateHtmlTextBoxById("AdditionalNotes");

        public HtmlSelect HowDidYouHearAboutUsSelect => ControlFactory.CreateHtmlSelectById("HowDidYouHearAboutUs");

        public HtmlButton SubmitButton => ControlFactory.CreateHtmlButtonById("submit");
    }
}
