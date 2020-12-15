using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benco.Framework.UI.Tests.Core;
using BencoPracticeTransitions.UI.Tests.Framework.Pages;
using Xunit;
using Xunit.Sdk;

namespace BencoPracticeTransitions.UI.Tests.Tests.Page_Tests
{
    [Collection("WebDriverCollection")]
    public class PracticeTransitionPracticeBuyRequiredFieldTests
    {
        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        //[InlineData(WebDriver.Browser.InternetExplorer)]

        public void NavigateToPracticeBuyPage_UserClickSubmit_RequiredFieldsValidates(
            WebDriver.Browser browser)
        {
            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.BuyPracticeLink.Click();
            Assert.True(Pages.PracticeBuyPage.IsLoaded);
            Pages.PracticeBuyPage.SubmitButton.Click();

            Assert.True(Pages.PracticeBuyPage.Contains("Contact Email is required."));
            Assert.True(Pages.PracticeBuyPage.Contains("Practice Type is required."));
            Assert.True(Pages.PracticeBuyPage.Contains("Contact First Name is required."));
            Assert.True(Pages.PracticeBuyPage.Contains("Purchase Location is required."));
            Assert.True(Pages.PracticeBuyPage.Contains("Real Estate Option is required."));
            Assert.True(Pages.PracticeBuyPage.Contains("Amount of Collections is required."));
            Assert.True(Pages.PracticeBuyPage.Contains("Maximum Purchase Amount is required."));
            Assert.True(Pages.PracticeBuyPage.Contains("Number of working operatories seeking is required."));
            Assert.True(Pages.PracticeBuyPage.Contains("Minimum Purchase Amount is required."));
            Assert.True(Pages.PracticeBuyPage.Contains("Contact Phone Number is required."));
            Assert.True(Pages.PracticeBuyPage.Contains("Please select if Yes or No"));
            Assert.True(Pages.PracticeBuyPage.Contains("There was an error validating the reCaptcha. Please check the box and try again."));



        }
    }
}
