using System;
using AutoFixture;
using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.Tests.Helpers;
using BencoPracticeTransitions.ViewModels.Home;
using BencoPracticeTransitions.ViewModels.Practice;
using Microsoft.Extensions.Options;
using Moq;
using RazorLight;
using Xunit;

namespace BencoPracticeTransitions.Tests.Email
{
    public class ContactUsEmailGeneratorTests
    {

        [Fact]
        public void CanCreateContactUsEmailGenerator()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;

            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            // ReSharper disable once UnusedVariable
            var sut = new ContactUsEmailGenerator(razorEngine,mockMailSettingsOptions.Object);
        }


        [Fact]
        public void CanGenerateEmailFromModel_WhenPassedModelThatIsNotOfTypeContactUsModel_ReturnsFalse()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new ContactUsEmailGenerator(razorEngine, mockMailSettingsOptions.Object);

            var model = fixture.Build<BuyPracticeModel>()
                .With(m => m.ContactPhoneNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();
            Assert.False(sut.CanGenerateEmailFromModel(model));
        }


        [Fact]
        public void CanGenerateEmailFromModel_WhenPassedModelThatIsOfTypeContactUsModel_ReturnsTrue()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new ContactUsEmailGenerator(razorEngine, mockMailSettingsOptions.Object);

            var model = fixture.Build<ContactUsModel>()
                .With(m => m.PhoneNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            Assert.True(sut.CanGenerateEmailFromModel(model));
        }


        [Fact]
        public void GenerateEmail_WhenPassedModelThatIsNotOfTypeContactUsModel_ThrowsArgumentException()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new ContactUsEmailGenerator(razorEngine, mockMailSettingsOptions.Object);

            var model = fixture.Build<BuyPracticeModel>()
                .With(m => m.ContactPhoneNumber, RandomDataGenerator.RandomPhoneNumber)
                .Create();

            var exception = Assert.Throws<ArgumentException>(() => sut.GenerateEmail(model));
            Assert.Equal("model", exception.ParamName);
        }



        [Fact]
        public void GenerateEmail_WhenPassedValidModel_SetsFromAddressToContactEmailInModel()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new ContactUsEmailGenerator(razorEngine, mockMailSettingsOptions.Object);

            var model = fixture.Build<ContactUsModel>()
                .With(m => m.PhoneNumber, RandomDataGenerator.RandomPhoneNumber)
                .With(m => m.EmailAddress, "testuser@benco.com")
                .Create();

            var result = sut.GenerateEmail(model);
            Assert.Equal("testuser@benco.com", result.From);
        }



        [Fact]
        public void GenerateEmail_WhenPassedValidModel_IncludesFirstAndLastNameFromModelInEmail()
        {
            var razorEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(Program))
                .UseMemoryCachingProvider()
                .Build();
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new ContactUsEmailGenerator(razorEngine, mockMailSettingsOptions.Object);

            var model = fixture.Build<ContactUsModel>()
                .With(m => m.PhoneNumber, RandomDataGenerator.RandomPhoneNumber)
                .With(m => m.FirstName, "Sam")
                .With(m => m.LastName, "Paul")
                .Create();

            var result = sut.GenerateEmail(model);
            Assert.Contains("Sam", result.Body);
            Assert.Contains("Paul", result.Body);
        }


        [Fact]
        public void GenerateEmail_WhenPassedValidModel_IncludesPhoneNumberFromModelInEmail()
        {
            var razorEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(Program))
                .UseMemoryCachingProvider()
                .Build();

            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new ContactUsEmailGenerator(razorEngine,mockMailSettingsOptions.Object);

            var model = fixture.Build<ContactUsModel>()
                .With(m => m.PhoneNumber, "234-567-8901")
                .Create();

            var result = sut.GenerateEmail(model);
            Assert.Contains("234-567-8901", result.Body);
        }
    }
}
