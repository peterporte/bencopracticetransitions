using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoFixture;
using BencoPracticeTransitions.Controllers;
using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.Tests.Helpers;
using BencoPracticeTransitions.ViewModels.Home;
using BencoPracticeTransitions.ViewModels.JobListing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using reCAPTCHA.AspNetCore;
using Xunit;

namespace BencoPracticeTransitions.Tests.Controllers.UnitTest
{
    public class JobListingControllerTests
    {
        [Fact]
        public void CanCreateJobController()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;

            // ReSharper disable once UnusedVariable
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
        }



        [Fact]
        public void Create_WhenModelIsValid_ReturnsRedirectToActionResultThankYouPage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);

            var fixture = new Fixture();
            var model = fixture.Build<CreateModel>()
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);

            var result = sut.Create(model);

            Assert.IsType<Task<IActionResult>>(result);
            var redirectToActionResult = result.Result as RedirectToActionResult;
            Assert.Equal("ThankYou", redirectToActionResult?.ControllerName);
            Assert.Equal("Index", redirectToActionResult?.ActionName);
        }


        [Fact]
        public void Create_WhenModelIsInvalid_ReturnViewResultCreatePracticePage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var tempData = new Mock<ITempDataDictionary>().Object;
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.ModelState.AddModelError("ModelInvalid", "Test Invalid Model Error");
            sut.TempData = tempData;

            var result = sut.Create(new CreateModel());

            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Create), viewResult?.ViewName);
        }


        [Fact]
        public void Create_WhenReCaptchaFails_ReturnViewResultCreatePracticePage()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(false);
            var tempData = new Mock<ITempDataDictionary>().Object;
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.TempData = tempData;
            var result = sut.Create(new CreateModel());

            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Create), viewResult?.ViewName);
        }



        [Fact]
        public void Inquire_WhenModelIsValid_ReturnsRedirectToActionResultThankYouPage()
        {
            // Arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(It.IsAny<IFormFile>(), out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);

            // Act
            var result = sut.Inquire(model, resumeUploader);

            // Assert
            Assert.IsType<Task<IActionResult>>(result);
            var redirectToActionResult = result.Result as RedirectToActionResult;
            Assert.Equal("ThankYou", redirectToActionResult?.ControllerName);
            Assert.Equal("Index", redirectToActionResult?.ActionName);
        }


        [Fact]
        public void Inquire_WhenModelIsInvalid_ReturnViewResultInquirePracticePage()
        {
            // Arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(It.IsAny<IFormFile>(), out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var tempData = new Mock<ITempDataDictionary>().Object;
            var resumeUploader = new Mock<IFormFile>().Object;

            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.ModelState.AddModelError("ModelInvalid", "Test Invalid Model Error");
            sut.TempData = tempData;
            // Act 
            var result = sut.Inquire(model, resumeUploader);

            // Assert
            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Inquire), viewResult?.ViewName);
        }


        [Fact]
        public void Create_WhenNoJobHoursSelected_ReturnViewResultCreatePracticePage()
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var tempData = new Mock<ITempDataDictionary>().Object;

            var fixture = new Fixture();
            var model = fixture
                .Build<CreateModel>()
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any()); // Make sure model is valid
            model.JobHours.ForEach(j => j.Checked = false);

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.TempData = tempData;
            Assert.True(sut.ModelState.IsValid);

            // Act 
            var result = sut.Create(model);

            // Assert
            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Create), viewResult?.ViewName);
        }

        [Fact]
        public void Create_WhenNoJobHoursSelected_ModelStateErrorContainsJobHours()
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);

            var fixture = new Fixture();
            var model = fixture
                .Build<CreateModel>()
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any()); // Make sure model is valid
            model.JobHours.ForEach(j => j.Checked = false);

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            Assert.True(sut.ModelState.IsValid);

            // Act 
            var unused = sut.Create(model);

            // Assert
            Assert.Contains(sut.ModelState, x => x.Key == nameof(CreateModel.JobHours));
        }


        [Fact]
        public void Create_WhenNoJobHoursSelected_ModelStateIsNotValid()
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);

            var fixture = new Fixture();
            var model = fixture
                .Build<CreateModel>()
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any()); // Make sure model is valid
            model.JobHours.ForEach(j => j.Checked = false);

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            Assert.True(sut.ModelState.IsValid);

            // Act 
            var unused = sut.Create(model);

            // Assert
            Assert.False(sut.ModelState.IsValid);
        }


        [Theory]
        [InlineData("Monday", null)]
        [InlineData("Monday", "")]
        [InlineData("Monday", " ")]
        [InlineData("Tuesday", null)]
        [InlineData("Tuesday", "")]
        [InlineData("Tuesday", " ")]
        [InlineData("Wednesday", null)]
        [InlineData("Wednesday", "")]
        [InlineData("Wednesday", " ")]
        [InlineData("Thursday", null)]
        [InlineData("Thursday", "")]
        [InlineData("Thursday", " ")]
        [InlineData("Friday", null)]
        [InlineData("Friday", "")]
        [InlineData("Friday", " ")]
        [InlineData("Saturday", null)]
        [InlineData("Saturday", "")]
        [InlineData("Saturday", " ")]
        [InlineData("Sunday", null)]
        [InlineData("Sunday", "")]
        [InlineData("Sunday", " ")]
        public void Create_WhenJobHoursSelectedHoursNotSpecified_ReturnViewResultCreatePracticePage(string day, string hours)
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var tempData = new Mock<ITempDataDictionary>().Object;

            var fixture = new Fixture();
            var model = fixture
                .Build<CreateModel>()
                .With(m => m.JobHours, new List<CreateJobHourModel>(
                    Enumerable.Range(1, 7)
                        .Select(dow => new CreateJobHourModel
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = fixture.Create<string>(),
                            Checked = true
                        })
                ))
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any()); // Make sure model is valid
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume, reCaptcha);
            sut.TempData = tempData;
            Assert.True(sut.ModelState.IsValid); // Make sure model is valid

            model.JobHours.Single(x => x.Day.Equals(day)).Hours = hours; //add invalid data

            // Act 
            var result = sut.Create(model);

            // Assert
            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Create), viewResult?.ViewName);
        }


        [Theory]
        [InlineData("Monday", null)]
        [InlineData("Monday", "")]
        [InlineData("Monday", " ")]
        [InlineData("Tuesday", null)]
        [InlineData("Tuesday", "")]
        [InlineData("Tuesday", " ")]
        [InlineData("Wednesday", null)]
        [InlineData("Wednesday", "")]
        [InlineData("Wednesday", " ")]
        [InlineData("Thursday", null)]
        [InlineData("Thursday", "")]
        [InlineData("Thursday", " ")]
        [InlineData("Friday", null)]
        [InlineData("Friday", "")]
        [InlineData("Friday", " ")]
        [InlineData("Saturday", null)]
        [InlineData("Saturday", "")]
        [InlineData("Saturday", " ")]
        [InlineData("Sunday", null)]
        [InlineData("Sunday", "")]
        [InlineData("Sunday", " ")]
        public void Create_WhenJobHoursSelectedHoursNotSpecified_ModelStateIsNotValid(string day, string hours)
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var tempData = new Mock<ITempDataDictionary>().Object;

            var fixture = new Fixture();
            var model = fixture
                .Build<CreateModel>()
                .With(m => m.JobHours, new List<CreateJobHourModel>(
                    Enumerable.Range(1, 7)
                        .Select(dow => new CreateJobHourModel
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = fixture.Create<string>(),
                            Checked = true
                        })
                ))
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any()); // Make sure model is valid
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume, reCaptcha);
            sut.TempData = tempData;
            Assert.True(sut.ModelState.IsValid); // Make sure model is valid

            model.JobHours.Single(x => x.Day.Equals(day)).Hours = hours; //add invalid data

            // Act 
            var unused = sut.Create(model);

            // Assert
            Assert.False(sut.ModelState.IsValid);
        }


        [Theory]
        [InlineData("Monday", null)]
        [InlineData("Monday", "")]
        [InlineData("Monday", " ")]
        [InlineData("Tuesday", null)]
        [InlineData("Tuesday", "")]
        [InlineData("Tuesday", " ")]
        [InlineData("Wednesday", null)]
        [InlineData("Wednesday", "")]
        [InlineData("Wednesday", " ")]
        [InlineData("Thursday", null)]
        [InlineData("Thursday", "")]
        [InlineData("Thursday", " ")]
        [InlineData("Friday", null)]
        [InlineData("Friday", "")]
        [InlineData("Friday", " ")]
        [InlineData("Saturday", null)]
        [InlineData("Saturday", "")]
        [InlineData("Saturday", " ")]
        [InlineData("Sunday", null)]
        [InlineData("Sunday", "")]
        [InlineData("Sunday", " ")]
        public void Create_WhenJobHoursSelectedHoursNotSpecified_ModelStateErrorContainsJobHours(string day, string hours)
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);

            var fixture = new Fixture();
            var model = fixture
                .Build<CreateModel>()
                .With(m => m.JobHours, new List<CreateJobHourModel>(
                    Enumerable.Range(1, 7)
                        .Select(dow => new CreateJobHourModel
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = fixture.Create<string>(),
                            Checked = true
                        })
                ))
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any()); // Make sure model is valid
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume, reCaptcha);
            Assert.True(sut.ModelState.IsValid);

            model.JobHours.Single(x => x.Day.Equals(day)).Hours = hours; //add invalid data

            // Act 
            var unused = sut.Create(model);

            // Assert
            Assert.Contains(sut.ModelState, x => x.Key.Equals(nameof(CreateModel.JobHours)));
        }


        [Fact]
        public void Create_WhenCreateViewGet_ReturnsViewResult()
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);

            // Act 
            var result = sut.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Equal(nameof(JobListingController.Create), viewResult?.ViewName);
        }


        [Fact]
        public void Create_WhenCreateViewGet_JobHoursContainsDaysOfTheWeek()
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);

            // Act 
            var result = sut.Create() as ViewResult;
            var model = result.ViewData.Model as CreateModel;

            //Assert
            var expectedResult = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            Assert.Equal(expectedResult, model.JobHours.Select(h => h.Day));
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_WhenDoctorSpecialtyIsNotSpecified_ReturnViewResultCreatePracticePage(string specialty)
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var tempData = new Mock<ITempDataDictionary>().Object;

            var fixture = new Fixture();
            var model = fixture
                .Build<CreateModel>()
                .With(m => m.JobType, JobTypeEnum.Doctor.ToString())
                .With(m => m.Specialty, specialty)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any()); // Make sure model is valid

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.TempData = tempData;
            Assert.True(sut.ModelState.IsValid);

            // Act 
            var result = sut.Create(model);

            // Assert
            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Create), viewResult?.ViewName);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_WhenDoctorSpecialtyIsNotSpecified_ModelStateIsNotValid(string specialty)
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);

            var fixture = new Fixture();
            var model = fixture
                .Build<CreateModel>()
                .With(m => m.JobType, JobTypeEnum.Doctor.ToString())
                .With(m => m.Specialty, specialty)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any()); // Make sure model is valid

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            Assert.True(sut.ModelState.IsValid);

            // Act 
            var unused = sut.Create(model);

            // Assert
            Assert.False(sut.ModelState.IsValid);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_WhenDoctorSpecialtyIsNotSpecified_ModelStateErrorContainsSpecialty(string specialty)
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);

            var fixture = new Fixture();
            var model = fixture
                .Build<CreateModel>()
                .With(m => m.JobType, JobTypeEnum.Doctor.ToString())
                .With(m => m.Specialty, specialty)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any()); // Make sure model is valid

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            Assert.True(sut.ModelState.IsValid);

            // Act 
            var unused = sut.Create(model);

            // Assert
            Assert.True(sut.ModelState.ContainsKey(nameof(CreateModel.Specialty)));
        }


        [Fact]
        public void Inquire_WhenReCaptchaFails_ReturnViewResultInquirePracticePage()
        {
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(It.IsAny<IFormFile>(), out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;
            var reCaptcha = MockReCaptchaService.Create(false);
            var resumeUploader = new Mock<IFormFile>().Object;
            var tempData = new Mock<ITempDataDictionary>().Object;

            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.TempData = tempData;
            var result = sut.Inquire(model, resumeUploader);

            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;

            Assert.Equal(nameof(JobListingController.Inquire), viewResult?.ViewName);
        }

        [Fact]
        public void Inquire_WhenResumeIsNotValid_ModelStateIsNotValid()
        {
            //arrange
            var errorMessage = "Invalid Email Error";
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(false);
            var validateResume = mockResumeValidator.Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var unused = sut.Inquire(model, resumeUploader);

            //assert
            Assert.False(sut.ModelState.IsValid);
        }

        [Fact]
        public void Inquire_WhenResumeIsNotValid_ReturnViewResultInquirePracticePage()
        {
            //arrange
            var errorMessage = "Invalid Email Error";
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(false);
            var validateResume = mockResumeValidator.Object;
            var tempData = new Mock<ITempDataDictionary>().Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.TempData = tempData;
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var result = sut.Inquire(model, resumeUploader);

            //assert
            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Inquire), viewResult?.ViewName);
        }

        [Fact]
        public void Inquire_WhenResumeIsNotValid_ModelStateErrorContainsResume()
        {
            //arrange
            var errorMessage = "Invalid Email Error";
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(false);
            var validateResume = mockResumeValidator.Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var unused = sut.Inquire(model, resumeUploader);

            //assert
            Assert.Contains(sut.ModelState, x => x.Key == nameof(InquireModel.Resume));
        }


        [Fact]
        public void Inquire_WhenAvailabilityIsNotSelected_ModelStateIsNotValid()
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.Availability, new List<InquireAvailabilityModel>(
                    Enumerable.Range(1, 7)
                        .Select(dow => new InquireAvailabilityModel()
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = null,
                            Checked = false
                        })
                ))
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var unused = sut.Inquire(model, resumeUploader);

            //assert
            Assert.False(sut.ModelState.IsValid);
        }

        [Fact]
        public void Inquire_WhenAvailabilityIsNotSelected_ModelStateErrorContainsAvailability()
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.Availability, new List<InquireAvailabilityModel>(
                    Enumerable.Range(1, 7)
                        .Select(dow => new InquireAvailabilityModel()
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = null,
                            Checked = false
                        })
                ))
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var unused = sut.Inquire(model, resumeUploader);

            //assert
            Assert.Contains(sut.ModelState, x => x.Key == nameof(InquireModel.Availability));
        }

        [Fact]
        public void Inquire_WhenAvailabilityIsNotSelected_ReturnViewResultInquirePracticePage()
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;
            var tempData = new Mock<ITempDataDictionary>().Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.TempData = tempData;
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.Availability, new List<InquireAvailabilityModel>(
                    Enumerable.Range(1, 7)
                        .Select(dow => new InquireAvailabilityModel()
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = null,
                            Checked = false
                        })
                ))
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var result = sut.Inquire(model, resumeUploader);

            //assert
            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Inquire), viewResult?.ViewName);
        }

        [Theory]
        [InlineData("Monday", null)]
        [InlineData("Monday", "")]
        [InlineData("Monday", " ")]
        [InlineData("Tuesday", null)]
        [InlineData("Tuesday", "")]
        [InlineData("Tuesday", " ")]
        [InlineData("Wednesday", null)]
        [InlineData("Wednesday", "")]
        [InlineData("Wednesday", " ")]
        [InlineData("Thursday", null)]
        [InlineData("Thursday", "")]
        [InlineData("Thursday", " ")]
        [InlineData("Friday", null)]
        [InlineData("Friday", "")]
        [InlineData("Friday", " ")]
        [InlineData("Saturday", null)]
        [InlineData("Saturday", "")]
        [InlineData("Saturday", " ")]
        [InlineData("Sunday", null)]
        [InlineData("Sunday", "")]
        [InlineData("Sunday", " ")]
        public void Inquire_WhenAvailabilitySelectedHoursNotSpecified_ModelStateIsNotValid(string day, string hour)
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;
            var tempData = new Mock<ITempDataDictionary>().Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.TempData = tempData;
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.Availability, new List<InquireAvailabilityModel>(
                    Enumerable.Range(1, 7)
                        .Select(dow => new InquireAvailabilityModel()
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = fixture.Create<string>(),
                            Checked = true
                        })
                ))
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid); //make sure model is valid

            model.Availability.Single(x => x.Day.Equals(day)).Hours = hour; //add invalid data

            //act
            var unused = sut.Inquire(model, resumeUploader);

            //assert
            Assert.False(sut.ModelState.IsValid);
        }

        [Theory]
        [InlineData("Monday", null)]
        [InlineData("Monday", "")]
        [InlineData("Monday", " ")]
        [InlineData("Tuesday", null)]
        [InlineData("Tuesday", "")]
        [InlineData("Tuesday", " ")]
        [InlineData("Wednesday", null)]
        [InlineData("Wednesday", "")]
        [InlineData("Wednesday", " ")]
        [InlineData("Thursday", null)]
        [InlineData("Thursday", "")]
        [InlineData("Thursday", " ")]
        [InlineData("Friday", null)]
        [InlineData("Friday", "")]
        [InlineData("Friday", " ")]
        [InlineData("Saturday", null)]
        [InlineData("Saturday", "")]
        [InlineData("Saturday", " ")]
        [InlineData("Sunday", null)]
        [InlineData("Sunday", "")]
        [InlineData("Sunday", " ")]
        public void Inquire_WhenAvailabilitySelectedHoursNotSpecified_ModelStateErrorContainsAvailability(string day, string hour)
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.Availability, new List<InquireAvailabilityModel>(
                    Enumerable.Range(1, 7)
                        .Select(dow => new InquireAvailabilityModel()
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = fixture.Create<string>(),
                            Checked = true
                        })
                ))
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid); //make sure model is valid

            model.Availability.Single(x => x.Day.Equals(day)).Hours = hour; //add invalid data

            //act
            var unused = sut.Inquire(model, resumeUploader);

            //assert
            Assert.Contains(sut.ModelState, x => x.Key == nameof(InquireModel.Availability));
        }


        [Theory]
        [InlineData("Monday", null)]
        [InlineData("Monday", "")]
        [InlineData("Monday", " ")]
        [InlineData("Tuesday", null)]
        [InlineData("Tuesday", "")]
        [InlineData("Tuesday", " ")]
        [InlineData("Wednesday", null)]
        [InlineData("Wednesday", "")]
        [InlineData("Wednesday", " ")]
        [InlineData("Thursday", null)]
        [InlineData("Thursday", "")]
        [InlineData("Thursday", " ")]
        [InlineData("Friday", null)]
        [InlineData("Friday", "")]
        [InlineData("Friday", " ")]
        [InlineData("Saturday", null)]
        [InlineData("Saturday", "")]
        [InlineData("Saturday", " ")]
        [InlineData("Sunday", null)]
        [InlineData("Sunday", "")]
        [InlineData("Sunday", " ")]
        public void Inquire_WhenAvailabilitySelectedHoursNotSpecified_ReturnViewResultInquirePracticePage(string day, string hour)
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;
            var tempData = new Mock<ITempDataDictionary>().Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.TempData = tempData;
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.Availability, new List<InquireAvailabilityModel>(
                    Enumerable.Range(1, 7)
                        .Select(dow => new InquireAvailabilityModel()
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = fixture.Create<string>(),
                            Checked = true
                        })
                ))
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid); //make sure model is valid

            model.Availability.Single(x => x.Day.Equals(day)).Hours = hour; //add invalid data

            //act
            var result = sut.Inquire(model, resumeUploader);

            //assert
            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Inquire), viewResult?.ViewName);
        }


        [Fact]
        public void Inquire_WhenInquireViewGet_ReturnsViewResult()
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);

            // Act 
            var result = sut.Inquire();

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Equal(nameof(JobListingController.Inquire), viewResult?.ViewName);
        }


        [Fact]
        public void Inquire_WhenInquireViewGet_AvailabilityContainsDaysOfTheWeek()
        {
            // Arrange
            var emailSender = new Mock<ISendEmail>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);

            // Act 
            var result = sut.Inquire() as ViewResult;
            var model = result.ViewData.Model as InquireModel;

            //Assert
            var expectedResult = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            Assert.Equal(expectedResult, model.Availability.Select(h => h.Day));
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Inquire_WhenDoctorSpecialtyIsNotSpecified_ModelStateIsNotValid(string specialty)
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.JobType, JobTypeEnum.Doctor.ToString())
                .With(m => m.Specialty, specialty)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var unused = sut.Inquire(model, resumeUploader);

            //assert
            Assert.False(sut.ModelState.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Inquire_WhenDoctorSpecialtyIsNotSpecified_ModelStateErrorContainsSpecialty(string specialty)
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.JobType, JobTypeEnum.Doctor.ToString())
                .With(m => m.Specialty, specialty)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var unused = sut.Inquire(model, resumeUploader);

            //assert
            Assert.Contains(sut.ModelState, x => x.Key == nameof(InquireModel.Specialty));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Inquire_WhenDoctorSpecialtyIsNotSpecified_ReturnViewResultInquirePracticePage(string specialty)
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;
            var tempData = new Mock<ITempDataDictionary>().Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.TempData = tempData;
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.JobType, JobTypeEnum.Doctor.ToString())
                .With(m => m.Specialty, specialty)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var result = sut.Inquire(model, resumeUploader);

            //assert
            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Inquire), viewResult?.ViewName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Inquire_WhenOtherWorkExperienceNotSpecified_ModelStateIsNotValid(string otherWorkExperience)
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.WorkExperience, WorkExperienceEnum.Other.ToString())
                .With(m => m.WorkExperienceOther, otherWorkExperience)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var unused = sut.Inquire(model, resumeUploader);

            //assert
            Assert.False(sut.ModelState.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Inquire_WhenOtherWorkExperienceNotSpecified_ModelStateErrorContainsWorkExperienceOther(string otherWorkExperience)
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.WorkExperience, WorkExperienceEnum.Other.ToString())
                .With(m => m.WorkExperienceOther, otherWorkExperience)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var unused = sut.Inquire(model, resumeUploader);

            //assert
            Assert.Contains(sut.ModelState, x => x.Key == nameof(InquireModel.WorkExperienceOther));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Inquire_WhenOtherWorkExperienceNotSpecified_ReturnViewResultInquirePracticePage(string otherWorkExperience)
        {
            //arrange
            string errorMessage;
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = MockReCaptchaService.Create(true);
            var resumeUploader = new Mock<IFormFile>().Object;
            var mockResumeValidator = new Mock<IValidateResume>();
            mockResumeValidator
                .Setup(m => m.UploadedResumeIsValid(resumeUploader, out errorMessage))
                .Returns(true);
            var validateResume = mockResumeValidator.Object;
            var tempData = new Mock<ITempDataDictionary>().Object;

            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume,
                reCaptcha);
            sut.TempData = tempData;
            var fixture = new Fixture();
            var model = fixture
                .Build<InquireModel>()
                .Without(m => m.Resume)
                .With(m => m.WorkExperience, WorkExperienceEnum.Other.ToString())
                .With(m => m.WorkExperienceOther, otherWorkExperience)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.False(ModelValidator.Validate(model).Any());
            Assert.True(sut.ModelState.IsValid);

            //act
            var result = sut.Inquire(model, resumeUploader);

            //assert
            Assert.IsType<Task<IActionResult>>(result);
            var viewResult = result.Result as ViewResult;
            Assert.Equal(nameof(JobListingController.Inquire), viewResult?.ViewName);
        }

        [Fact]
        public void Index_WhenDoingHttpGet_ReturnsView()
        {
            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume, reCaptcha);

            var result = sut.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(nameof(JobListingController.Index), viewResult.ViewName);

        }


        [Theory]
        [InlineData("post_job_opening", "JobListing", "Create")]
        [InlineData("look_for_job", "JobListing", "Inquire")]
        public void Index_WhenDoingHttpPostForValidActionNotOnIndexController_RedirectsToTheExpectedControllerAndView(string action, string expectedControllerName, string expectedViewName)
        {
            var model = new PracticeOptionsModel
            {
                SelectedPracticeOption = action
            };

            var emailSender = new Mock<ISendEmail>().Object;
            var reCaptcha = new Mock<IRecaptchaService>().Object;
            var validateResume = new Mock<IValidateResume>().Object;
            var sut = new JobListingController(Enumerable.Empty<IGenerateEmail>(), emailSender, validateResume, reCaptcha);

            var result = sut.Index(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(expectedControllerName, redirectToActionResult.ControllerName);
            Assert.Equal(expectedViewName, redirectToActionResult.ActionName);
        }
    }
}
