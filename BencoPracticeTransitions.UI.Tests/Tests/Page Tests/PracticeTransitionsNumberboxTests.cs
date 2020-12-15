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
    public class PracticeTransitionsNumberboxTests
    {  

        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        public void AccessPracticeTransistionHomePage_SellPracticeLink_EnterNumber_ConfirmedEnteredValue(
           WebDriver.Browser browser)
        {

            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.SellPracticeLink.Click();
            Pages.PracticeSellPage.AskingPriceTextBox.Enter("1");


            var confirmValue = Pages.PracticeSellPage.AskingPriceTextBox.GetAttribute("value");

            Assert.Equal("1", confirmValue);
        }


        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        public void AccessPracticeTransistionHomePage_SellPracticeLink_EnterNonNumber_VerifiedNonNumber(
            WebDriver.Browser browser)
        {

            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.SellPracticeLink.Click();
            Pages.PracticeSellPage.AskingPriceTextBox.Enter("e");



            var confirmValue = Pages.PracticeSellPage.AskingPriceTextBox.GetAttribute("value");
            var isDigit = int.TryParse(confirmValue, out int result);

            Assert.False(isDigit);
        }

        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        public void AccessPracticeTransistionHomePage_SellPracticeLink_EnterNumber_VerifiedNumber(
            WebDriver.Browser browser)
        {

            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.SellPracticeLink.Click();
            Pages.PracticeSellPage.AskingPriceTextBox.Enter("1");



            var confirmValue = Pages.PracticeSellPage.AskingPriceTextBox.GetAttribute("value");
            var isDigit = int.TryParse(confirmValue, out int result);

            Assert.True(isDigit);
        }


        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        public void AccessPracticeTransistionHomePage_BuyPracticeLink_EnterNumber_ConfirmedEnteredValue(
            WebDriver.Browser browser)
        {

            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.BuyPracticeLink.Click();
            Pages.PracticeBuyPage.MinPurchaseAmountNumber.Enter("1");


            var confirmValue = Pages.PracticeBuyPage.MinPurchaseAmountNumber.GetAttribute("value");

            Assert.Equal("1", confirmValue);
        }

        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        public void AccessPracticeTransistionHomePage_BuyPracticeLink_EnterNonNumber_VerifiedNonNumber(
            WebDriver.Browser browser)
        {

            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.BuyPracticeLink.Click();
            Pages.PracticeBuyPage.MinPurchaseAmountNumber.Enter("e");


            var confirmValue = Pages.PracticeBuyPage.MinPurchaseAmountNumber.GetAttribute("value");
            var isDigit = int.TryParse(confirmValue, out int result);

            Assert.False(isDigit);
        }


        [Theory]
        [InlineData(WebDriver.Browser.Chrome)]
        public void AccessPracticeTransistionHomePage_BuyPracticeLink_EnterNumber_VerifiedNumber(
            WebDriver.Browser browser)
        {

            Pages.SetBrowser(browser);
            WebDriver.Driver.Manage().Window.Maximize();
            Pages.PracticeTransistionsHomePage.GoTo();
            Pages.PracticeTransistionsHomePage.BuyPracticeLink.Click();
            Pages.PracticeBuyPage.MinPurchaseAmountNumber.Enter("1");


            var confirmValue = Pages.PracticeBuyPage.MinPurchaseAmountNumber.GetAttribute("value");
            var isDigit = int.TryParse(confirmValue, out int result);

            Assert.True(isDigit);
        }
    }
}
