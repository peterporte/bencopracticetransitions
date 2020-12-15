using Benco.Framework.UI.Tests.Core;
using BencoPracticeTransitions.UI.Tests.Framework.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BencoPracticeTransitions.UI.Tests.Tests.Page_Tests
{
    public class PracticeTransitionsCheckboxTest
    {

        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        public void AccessPracticeTransistionHomePage_ClickPostJobLink_ClickJobHourskCheckbox_EnableHoursTextBox(
            WebDriver.Browser browser)
        {

            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.PostJobLink.Click();
            Pages.JobListingCreatePage.JobHours_0__CheckedCheckBox.Click();

            var attributeValue =
                Pages.JobListingCreatePage.JobHours_0__HoursTextBox.WebElement.GetAttribute("disabled");
            bool.TryParse(attributeValue, out var textBoxIsDisabled);

            Assert.False(textBoxIsDisabled);

        }


        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        public void AccessPracticeTransistionHomePage_ClickFindJobLink_ClickAvailabilitykCheckbox_EnableHoursTextBox(
            WebDriver.Browser browser)
        {

            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.FindJobLink.Click();
            Pages.JobListingInquirePage.Availability_0__CheckedCheckBox.Click();

            var attributeValue =
                Pages.JobListingInquirePage.Availability_0__HoursTextBox.WebElement.GetAttribute("disabled");
            bool.TryParse(attributeValue, out var textBoxIsDisabled);

            Assert.False(textBoxIsDisabled);

        }

        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        [InlineData(WebDriver.Browser.Firefox)]
        [InlineData(WebDriver.Browser.InternetExplorer)]
        public void AccessFindJobPage_ClickAvailabilitykCheckbox_EnableHoursTextBox(WebDriver.Browser browser)
        {

            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.FindJobLink.Click();
            Pages.JobListingInquirePage.Availability_0__CheckedCheckBox.Click();

            var attributeValue =
            Pages.JobListingInquirePage.Availability_0__HoursTextBox.WebElement.GetAttribute("disabled");
            bool.TryParse(attributeValue, out var textBoxIsDisabled);

            Assert.False(textBoxIsDisabled);

        }

        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        [InlineData(WebDriver.Browser.Firefox)]
        [InlineData(WebDriver.Browser.InternetExplorer)]
        public void MyChecAccessFindJobPage_SelectAndDeSelecAvailabilitytCheckbox_CheckboxBehaveAsExpected(WebDriver.Browser browser)
        {
            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.FindJobLink.Click();

            //checkbox is initially uncheck
            Assert.False(Pages.JobListingInquirePage.Availability_0__CheckedCheckBox.IsChecked());

            //select the checkbox
            Pages.JobListingInquirePage.Availability_0__CheckedCheckBox.Click();
            Assert.True(Pages.JobListingInquirePage.Availability_0__CheckedCheckBox.IsChecked());

            //deselect the checkbox
            Pages.JobListingInquirePage.Availability_0__CheckedCheckBox.Click();
            Assert.False(Pages.JobListingInquirePage.Availability_0__CheckedCheckBox.IsChecked());
        }


        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        [InlineData(WebDriver.Browser.Firefox)]
        [InlineData(WebDriver.Browser.InternetExplorer)]
        public void AccessPostJobPage_SelectAndDeSelectJobHoursCheckbox_CheckboxBehaveAsExpected(WebDriver.Browser browser)
        {
            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.PostJobLink.Click();

            //checkbox is initially uncheck
            Assert.False(Pages.JobListingCreatePage.JobHours_0__CheckedCheckBox.IsChecked());

            //select the checkbox
            Pages.JobListingCreatePage.JobHours_0__CheckedCheckBox.Click();
            Assert.True(Pages.JobListingCreatePage.JobHours_0__CheckedCheckBox.IsChecked());

            //deselect the checkbox
            Pages.JobListingCreatePage.JobHours_0__CheckedCheckBox.Click();
            Assert.False(Pages.JobListingCreatePage.JobHours_0__CheckedCheckBox.IsChecked());
        }

    }
}
