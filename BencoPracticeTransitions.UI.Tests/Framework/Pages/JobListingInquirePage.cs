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
    class JobListingInquirePage : Page
    {
        public JobListingInquirePage()
        {
            BaseUrl = $"{UrlHelper.GetPracticeTransitionsUrl()}/JobListing/Inquire";
        }

        public HtmlTextBox FirstNameTextBox => ControlFactory.CreateHtmlTextBoxById("FirstName");
        public HtmlTextBox LastNameTextBox => ControlFactory.CreateHtmlTextBoxById("LastName");
        public HtmlTextBox AddressTextBox => ControlFactory.CreateHtmlTextBoxById("Address");
        public HtmlTextBox ContactNumberTextBox => ControlFactory.CreateHtmlTextBoxById("ContactNumber");
        public HtmlTextBox ContactEmailTextBox => ControlFactory.CreateHtmlTextBoxById("ContactEmail");

        public HtmlSelect WorkExperienceSelect => ControlFactory.CreateHtmlSelectById("WorkExperience");
        public HtmlSelect JobTypeSelect => ControlFactory.CreateHtmlSelectById("JobType");

        public HtmlTextBox JobLocationTextBox => ControlFactory.CreateHtmlTextBoxById("JobLocation");


        public HtmlCheckbox Availability_0__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("Availability_0__Checked");
        public HtmlTextBox Availability_0__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("Availability_0__Hours");
        public HtmlCheckbox Availability_1__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("Availability_1__Checked");
        public HtmlTextBox Availability_1__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("Availability_1__Hours");
        public HtmlCheckbox Availability_2__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("Availability_2__Checked");
        public HtmlTextBox Availability_2__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("Availability_2__Hours");
        public HtmlCheckbox Availability_3__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("Availability_3__Checked");
        public HtmlTextBox Availability_3__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("Availability_3__Hours");
        public HtmlCheckbox Availability_4__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("Availability_4__Checked");
        public HtmlTextBox Availability_4__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("Availability_4__Hours");
        public HtmlCheckbox Availability_5__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("Availability_5__Checked");
        public HtmlTextBox Availability_5__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("Availability_5__Hours");
        public HtmlCheckbox Availability_6__CheckedCheckBox => ControlFactory.CreateHtmlCheckboxById("Availability_6__Checked");
        public HtmlTextBox Availability_6__HoursTextBox => ControlFactory.CreateHtmlTextBoxById("Availability_6__Hours");


        public HtmlTextBox LinkedInAccountTextBox => ControlFactory.CreateHtmlTextBoxById("LinkedInAccount");
        public HtmlTextBox AdditionalNotesTextBox => ControlFactory.CreateHtmlTextBoxById("AdditionalNotes");

        public HtmlSelect HowDidYouHearAboutUsSelect => ControlFactory.CreateHtmlSelectById("HowDidYouHearAboutUs");

        public HtmlButton SubmitButton => ControlFactory.CreateHtmlButtonById("submit");

 


    }
}

