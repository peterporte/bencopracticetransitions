using System.Collections.Generic;
using BencoPracticeTransitions.Controllers;
using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.ViewModels.Practice;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BencoPracticeTransitions.Tests.Helpers;
using BencoPracticeTransitions.ViewModels.Home;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using reCAPTCHA.AspNetCore;
using Xunit;

namespace BencoPracticeTransitions.Tests.Controllers.UnitTest
{
    public class PracticeControllerTest
    {
        [Fact]
        public void CanCreatePracticeController()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;

            // ReSharper disable once UnusedVariable
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);
        }

        [Fact]
        public void Buy_WhenModelIsValid_ReturnsRedirectToActionResultThankyouPage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Buy(new BuyPracticeModel());

            Assert.IsType<Task<IActionResult>>(result);
            var redirectToActionResult = result.Result as RedirectToActionResult;
            Assert.Equal("ThankYou", redirectToActionResult?.ControllerName);
            Assert.Equal("Index", redirectToActionResult?.ActionName);
        }


        [Fact]
        public void Buy_WhenModelIsInvalid_ReturnViewResultBuyPracticePage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var tempData = new Mock<ITempDataDictionary>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);          

            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);
            sut.ModelState.AddModelError("ModelInvalid", "Test Invalid Model Error");
            sut.TempData = tempData;

            var result = sut.Buy(new BuyPracticeModel());

            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal("Buy", viewResult?.ViewName);
        }


        [Fact]
        public void Buy_WhenReCaptchaFails_ReturnViewResultBuyPracticePage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var tempData = new Mock<ITempDataDictionary>().Object;
            var reCaptcha = MockReCaptchaService.Create(false);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);
            sut.TempData = tempData;

            var result = sut.Buy(new BuyPracticeModel());

            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal("Buy", viewResult?.ViewName);
        }

        [Fact]
        public void Buy_WhenModelMinPurchaseAmountIsLessThanMaxPurchaseAmount_ReturnsRedirectToActionResultThankyouPage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var fixture = new Fixture();
            var model = fixture
                .Build<BuyPracticeModel>()
                .Without(m => m.ContactPhoneNumber)
                .With(m => m.MinPurchaseAmount, 99)
                .With(m => m.MaxPurchaseAmount, 100)
                .Create();

            var result = sut.Buy(model);

            Assert.IsType<Task<IActionResult>>(result);
            var redirectToActionResult = result.Result as RedirectToActionResult;
            Assert.Equal("ThankYou", redirectToActionResult?.ControllerName);
            Assert.Equal("Index", redirectToActionResult?.ActionName);
        }


        [Fact]
        public void Buy_WhenModelMinPurchaseAmountIsLessThanMaxPurchaseAmount_ModelStateIsValid()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var fixture = new Fixture();
            var model = fixture
                .Build<BuyPracticeModel>()
                .Without(m => m.ContactPhoneNumber)
                .With(m => m.MinPurchaseAmount, 99)
                .With(m => m.MaxPurchaseAmount, 100)
                .Create();

            Assert.True(sut.ModelState.IsValid);

            var unused = sut.Buy(model);

            Assert.True(sut.ModelState.IsValid);
        }


        [Fact]
        public void Buy_WhenModelMinPurchaseAmountIsEqualToMaxPurchaseAmount_ReturnViewResultBuyPracticePage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var tempData = new Mock<ITempDataDictionary>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);
            sut.TempData = tempData;

            var fixture = new Fixture();
            var model = fixture
                .Build<BuyPracticeModel>()
                .Without(m => m.ContactPhoneNumber)
                .Create();
            model.MinPurchaseAmount = 100;
            model.MaxPurchaseAmount = 100;

            var result = sut.Buy(model);

            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal("Buy", viewResult?.ViewName);
        }


        [Fact]
        public void Buy_WhenModelMinPurchaseAmountIsEqualToMaxPurchaseAmount_ModelStateIsNotValid()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var fixture = new Fixture();
            var model = fixture
                .Build<BuyPracticeModel>()
                .Without(m => m.ContactPhoneNumber)
                .Create();
            model.MinPurchaseAmount = 100;
            model.MaxPurchaseAmount = 100;

            Assert.True(sut.ModelState.IsValid);

            var unused = sut.Buy(model);

            Assert.False(sut.ModelState.IsValid);
        }


        [Fact]
        public void Buy_WhenModelMinPurchaseAmountIsGreaterThanMaxPurchaseAmount_ReturnViewResultBuyPracticePage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var tempData = new Mock<ITempDataDictionary>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);
            sut.TempData = tempData;

            var fixture = new Fixture();
            var model = fixture
                .Build<BuyPracticeModel>()
                .Without(m => m.ContactPhoneNumber)
                .Create();
            model.MinPurchaseAmount = 100;
            model.MaxPurchaseAmount = 99;

            var result = sut.Buy(model);

            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal("Buy", viewResult?.ViewName);
        }


        [Fact]
        public void Buy_WhenModelMinPurchaseAmountIsGreaterThanMaxPurchaseAmount_ModelStateIsNotValid()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var fixture = new Fixture();
            var model = fixture
                .Build<BuyPracticeModel>()
                .Without(m => m.ContactPhoneNumber)
                .Create();
            model.MinPurchaseAmount = 100;
            model.MaxPurchaseAmount = 99;

            Assert.True(sut.ModelState.IsValid);

            var unused = sut.Buy(model);

            Assert.False(sut.ModelState.IsValid);
        }


        [Fact]
        public void Sell_WhenModelIsValid_ReturnsRedirectToActionResultThankYouPage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Sell(new SellPracticeModel());

            Assert.IsType<Task<IActionResult>>(result);
            var redirectToActionResult = result.Result as RedirectToActionResult;
            Assert.Equal("ThankYou", redirectToActionResult?.ControllerName);
            Assert.Equal("Index", redirectToActionResult?.ActionName);
        }


        [Fact]
        public void Sell_WhenModelIsInvalid_ReturnViewResultSellPracticePage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var tempData = new Mock<ITempDataDictionary>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);
            sut.ModelState.AddModelError("ModelInvalid", "Test Invalid Model Error");
            sut.TempData = tempData;

            var result = sut.Sell(new SellPracticeModel());

            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal("Sell", viewResult?.ViewName);
        }


        [Fact]
        public void Sell_WhenReCaptchaFails_ReturnViewResultSellPracticePage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var tempData = new Mock<ITempDataDictionary>().Object;
            var reCaptcha = MockReCaptchaService.Create(false);
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);
            sut.TempData = tempData;

            var result = sut.Sell(new SellPracticeModel());

            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal("Sell", viewResult?.ViewName);
        }


        [Fact]
        public void Index_WhenDoingHttpGet_ReturnsView()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(nameof(PracticeController.Index), viewResult.ViewName);

        }



        [Theory]
        [InlineData("buy_a_practice", "Practice", "Buy")]
        [InlineData("sell_a_practice", "Practice", "Sell")]
        public void Index_WhenDoingHttpPostForValidActionNotOnIndexController_RedirectsToTheExpectedControllerAndView(string action, string expectedControllerName, string expectedViewName)
        {
            var model = new PracticeOptionsModel
            {
                SelectedPracticeOption = action
            };

            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Index(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(expectedControllerName, redirectToActionResult.ControllerName);
            Assert.Equal(expectedViewName, redirectToActionResult.ActionName);
        }


        [Fact]
        public void Sell_WhenDoingHttpGet_ReturnsView()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Sell();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(nameof(PracticeController.Sell), viewResult.ViewName);

        }


        [Fact]
        public void Sell_WhenDoingHttpGet_ContainsCorrectCollectionsAmountOptions()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Sell();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualCollectionsList = viewResult.ViewData["CollectionsAmount"] as SelectList;
            var actualCollectionsAmountOptions = actualCollectionsList.Items as IEnumerable<SelectListItem>;

            var expectedCollectionsAmountOptions = new [] { null, "$0 - $500,000", "$500,000 - $1,000,000", "$1,500,000 - $2,000,000", "$2,000,000 +"};
            Assert.Equal(expectedCollectionsAmountOptions, actualCollectionsAmountOptions.Select(x => x.Value));
        }


        [Fact]
        public void Buy_WhenDoingHttpGet_ReturnsView()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Buy();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(nameof(PracticeController.Buy), viewResult.ViewName);

        }


        [Fact]
        public void Buy_WhenDoingHttpGet_ContainsCorrectCollectionsAmountOptions()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new PracticeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Buy();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualCollectionsList = viewResult.ViewData["CollectionsAmount"] as SelectList;
            var actualCollectionsAmountOptions = actualCollectionsList.Items as IEnumerable<SelectListItem>;

            var expectedCollectionsAmountOptions = new[] { null, "$0 - $500,000", "$500,000 - $1,000,000", "$1,500,000 - $2,000,000", "$2,000,000 +" };
            Assert.Equal(expectedCollectionsAmountOptions, actualCollectionsAmountOptions.Select(x => x.Value));
        }
    }
}
