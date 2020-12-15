using Benco.Framework.UI.Tests.Core;
using BencoPracticeTransitions.UI.Tests.Framework.Pages;
using Xunit;

namespace BencoPracticeTransitions.UI.Tests.Tests.Page_Tests
{
    [Collection("WebDriverCollection")]
    public class PracticeTransitionHomePageTests
    {
        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        [InlineData(WebDriver.Browser.InternetExplorer)]
        public void NavigateToPracticeTransistionHomePage_WhenUserClicksSellAPracticeLink_RedirectedToSellAPracticePage(
            WebDriver.Browser browser)
        {
            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.SellPracticeLink.Click();
            Assert.True(Pages.PracticeSellPage.IsAt);
        }

        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        [InlineData(WebDriver.Browser.InternetExplorer)]
        public void NavigateToPracticeTransistionHomePage_WhenUserClicksBuyAPracticeLink_RedirectedToBuyAPracticePage(
            WebDriver.Browser browser)
        {
            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.BuyPracticeLink.Click();
            Assert.True(Pages.PracticeBuyPage.IsAt);
        }

        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        [InlineData(WebDriver.Browser.InternetExplorer)]
        public void NavigateToPracticeTransistionHomePage_WhenUserClicksPostAJobLink_RedirectedToPostAJobOpeningPage(
            WebDriver.Browser browser)
        {
            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.PostJobLink.Click();
            Assert.True(Pages.JobListingCreatePage.IsAt);
        }

        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        [InlineData(WebDriver.Browser.InternetExplorer)]
        public void NavigateToPracticeTransistionHomePage_WhenUserClicksFindAJobLink_RedirectedToLookForAJobPage(
            WebDriver.Browser browser)
        {
            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.FindJobLink.Click();
            Assert.True(Pages.JobListingInquirePage.IsAt);
        }

        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        [InlineData(WebDriver.Browser.InternetExplorer)]
        public void
            NavigateToPracticeTransistionHomePage_WhenUserClicksPracticeTransitionTeamLink_RedirectedToContactUsPage(
                WebDriver.Browser browser)
        {
            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.ContactUsLink.Click();
            Assert.True(Pages.ContactUsPage.IsAt);
        }
    }
}