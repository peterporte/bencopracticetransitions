using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BencoPracticeTransitions.Controllers;
using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.Tests.Helpers;
using BencoPracticeTransitions.ViewModels.Home;
using BencoPracticeTransitions.ViewModels.Practice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using reCAPTCHA.AspNetCore;
using Xunit;

namespace BencoPracticeTransitions.Tests.Controllers.UnitTest
{
    public class HomeControllerTests
    {

        [Fact]
        public void CanCreateHomeController()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;

            // ReSharper disable once UnusedVariable
            var sut = new HomeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha );
        }


        [Fact]
        public void Index_WhenDoingHttpGet_ReturnsView()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new HomeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);

        }



        [Theory]
        [InlineData("buy_a_practice", "Practice", "Buy")]
        [InlineData("sell_a_practice", "Practice", "Sell")]
        [InlineData("post_job_opening", "JobListing", "Create")]
        [InlineData("look_for_job", "JobListing", "Inquire")]
        public void Index_WhenDoingHttpPostForValidActionNotOnIndexController_RedirectsToTheExpectedControllerAndView(string action, string expectedControllerName, string expectedViewName)
        {
            var model = new PracticeOptionsModel
            {
                SelectedPracticeOption =action
            };

            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new HomeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Index(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(expectedControllerName, redirectToActionResult.ControllerName);
            Assert.Equal(expectedViewName, redirectToActionResult.ActionName);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("567")]
        [InlineData("dog")]
        public void Index_WhenDoingHttpPostForInvalidAction_ReturnsExpectedView(string action)
        {
            var model = new PracticeOptionsModel
            {
                SelectedPracticeOption = action
            };

            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new HomeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.Index(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }


        [Fact]
        public void Index_WhenModelStateIsInvalid_ReturnsExpectedView()
        {
            var model = new PracticeOptionsModel
            {
                SelectedPracticeOption = null
            };

            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new HomeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            sut.ModelState.AddModelError("Test", "Model state is invalid.");

            var result = sut.Index(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }


        [Fact]
        public void ContactUs_WhenDoingHttpGet_ReturnsView()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var sut = new HomeController(Enumerable.Empty<IGenerateEmail>(), emailSender, reCaptcha);

            var result = sut.ContactUs();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ContactUs", viewResult.ViewName);
        }


        [Fact]
        public void ContactUs_WhenReCaptchaIsNotValid_AddsErrorToModelState()
        {
            var emailSender = new Mock<ISendEmail>().Object;

            var sut = new HomeController(Enumerable.Empty<IGenerateEmail>(), emailSender, MockReCaptchaService.Create(false));
            Assert.True(sut.ModelState.IsValid);

            var fixture = new Fixture();
            var model = fixture.Build<ContactUsModel>()
                .With(m => m.PhoneNumber, RandomDataGenerator.RandomPhoneNumber())
                .Create();

            //Act
            // ReSharper disable once UnusedVariable
            var result = sut.ContactUs(model);

            // Assert
            Assert.False(sut.ModelState.IsValid);
            Assert.Contains(sut.ModelState.Keys, s => s == "reCaptchaError");

        }


        [Fact]
        public void ContactUs_WhenModelIsNotValid_ReturnsContactUsView()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var tempData = new Mock<ITempDataDictionary>().Object;

            var sut = new HomeController(Enumerable.Empty<IGenerateEmail>(), emailSender, MockReCaptchaService.Create(true));
            sut.ModelState.AddModelError("Test", "Test Error");
            sut.TempData = tempData;          

            Assert.False(sut.ModelState.IsValid);

            var fixture = new Fixture();
            var model = fixture.Build<ContactUsModel>()
                .With(m => m.PhoneNumber, RandomDataGenerator.RandomPhoneNumber())
                .Create();

            //Act
            // ReSharper disable once UnusedVariable
            var result = sut.ContactUs(model);

            // Assert
            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal("ContactUs", viewResult?.ViewName);

        }



        [Fact]
        public void ContactUs_WhenModelIsValid_ReturnsContactUsView()
        {
            var emailSender = new Mock<ISendEmail>().Object;

            var sut = new HomeController(Enumerable.Empty<IGenerateEmail>(), emailSender, MockReCaptchaService.Create(true));
            Assert.True(sut.ModelState.IsValid);

            var fixture = new Fixture();
            var model = fixture.Build<ContactUsModel>()
                .With(m => m.PhoneNumber, RandomDataGenerator.RandomPhoneNumber())
                .Create();

            //Act
            // ReSharper disable once UnusedVariable
            var result = sut.ContactUs(model);

            // Assert
            Assert.IsType<Task<IActionResult>>(result);
            var redirectToActionResult = result.Result as RedirectToActionResult;
            Assert.Equal(nameof(ThankYouController), redirectToActionResult?.ControllerName + "Controller") ;
            Assert.Equal(nameof(ThankYouController.Index), redirectToActionResult?.ActionName);
        }


        [Fact]
        public void ContactUs_WhenModelIsValid_GeneratesEmail()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var mockEmailGenerator = new Mock<IGenerateEmail>();
            mockEmailGenerator.Setup(m => m.CanGenerateEmailFromModel(It.IsAny<object>()))
                .Returns(true);
            mockEmailGenerator.Setup(m => m.GenerateEmail(It.IsAny<object>()))
                .Verifiable("Email was not generated.");

            var sut = new HomeController(new [] {mockEmailGenerator.Object}, emailSender, MockReCaptchaService.Create(true));
            Assert.True(sut.ModelState.IsValid);

            var fixture = new Fixture();
            var model = fixture.Build<ContactUsModel>()
                .With(m => m.PhoneNumber, RandomDataGenerator.RandomPhoneNumber())
                .Create();

            //Act
            // ReSharper disable once UnusedVariable
            var result = sut.ContactUs(model);

            // Assert -- Generally we don't want to test implementations but rather test results.
            //           The result we are testing for is to verify that the controller is
            //           generating an email not how it is generating the email.
            mockEmailGenerator.Verify(); 
        }

        [Fact]
        public void ContactUs_WhenThereAreMultipleEmailGenerators_OnlyGeneratesContactUsEmail()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var mockContactUsGenerator = new Mock<IGenerateEmail>();
            mockContactUsGenerator.Setup(m => m.CanGenerateEmailFromModel(It.IsAny<object>() ))
                .Returns<object>(o => o is ContactUsModel);
            mockContactUsGenerator.Setup(m => m.GenerateEmail(It.IsAny<object>()))
                .Verifiable("Email was not generated.");

            var mockSellNotificationGenerator = new Mock<IGenerateEmail>();
            mockSellNotificationGenerator.Setup(m => m.CanGenerateEmailFromModel(It.IsAny<object>()))
                .Returns<object>(o => o is SellPracticeModel);
            mockSellNotificationGenerator.Setup(m => m.GenerateEmail(It.IsAny<object>()))
                .Verifiable("Incorrect email was generated.");

            var mockBuyNotificationGenerator = new Mock<IGenerateEmail>();
            mockBuyNotificationGenerator.Setup(m => m.CanGenerateEmailFromModel(It.IsAny<object>()))
                .Returns<object>(o => o is BuyPracticeModel);
            mockBuyNotificationGenerator.Setup(m => m.GenerateEmail(It.IsAny<object>()))
                .Verifiable("Incorrect email was generated.");

            var emailGenerators = new[]
            {
                mockContactUsGenerator.Object,
                mockBuyNotificationGenerator.Object,
                mockSellNotificationGenerator.Object
            };

            var sut = new HomeController(emailGenerators, emailSender, MockReCaptchaService.Create(true));
            Assert.True(sut.ModelState.IsValid);

            var fixture = new Fixture();
            var model = fixture.Build<ContactUsModel>()
                .With(m => m.PhoneNumber, RandomDataGenerator.RandomPhoneNumber())
                .Create();

            //Act
            // ReSharper disable once UnusedVariable
            var result = sut.ContactUs(model);

            // Assert -- Generally we don't want to test implementations but rather test results.
            //           The result we are testing here is that the controller calls the correct
            //           generator and is not calling all the generators
            mockContactUsGenerator.Verify(m=> m.GenerateEmail(It.IsAny<object>()), Times.Exactly(1));
            mockBuyNotificationGenerator.Verify(m => m.GenerateEmail(It.IsAny<object>()), Times.Never);
            mockSellNotificationGenerator.Verify(m => m.GenerateEmail(It.IsAny<object>()), Times.Never);
        }

    }
}
