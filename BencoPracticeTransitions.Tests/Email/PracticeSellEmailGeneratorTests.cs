using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.ViewModels.Practice;
using Microsoft.Extensions.Options;
using Moq;
using RazorLight;
using Xunit;

namespace BencoPracticeTransitions.Tests.Email
{
    public class PracticeSellEmailGeneratorTests
    {

        [Fact]
        public void CanCreatePracticeSellEmailGenerator()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            // ReSharper disable once UnusedVariable
            var sut = new PracticeSellEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);
        }


        [Fact]
        public void CanGenerateEmailFromModel_WhenPassedModelThatIsNotOfTypeSellPracticeModel_ReturnsFalse()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new PracticeSellEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.False(sut.CanGenerateEmailFromModel(new BuyPracticeModel()));
        }


        [Fact]
        public void CanGenerateEmailFromModel_WhenPassedModelThatIsOfTypeSellPracticeModel_ReturnsTrue()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new PracticeSellEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.True(sut.CanGenerateEmailFromModel(new SellPracticeModel()));
        }


        [Fact]
        public void GenerateEmail_WhenPassedModelThatIsNotOfTypeSellPracticeModel_ThrowsArgumentException()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new PracticeSellEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.Throws<ArgumentException>(() => sut.GenerateEmail(new BuyPracticeModel()));
        }


        [Fact]
        public void GenerateEmail_WhenPassedModelThatIsOfTypeSellPracticeModel_Succeeds()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new PracticeSellEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.NotNull(sut.GenerateEmail(new SellPracticeModel()));
        }


        [Fact]
        public void GenerateEmail_WhenPassedValidModel_CreatesAttachment()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>();
            attachmentGenerator.Setup(m => m.GenerateCsvEmailAttachment(It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()))
                .Returns(new EmailAttachment { Data = null, Filename = "Practice Transitions.csv" });

            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new PracticeSellEmailGenerator(razorEngine, attachmentGenerator.Object, mockMailSettingsOptions.Object);

            var request = sut.GenerateEmail(new SellPracticeModel());

            Assert.NotNull(request.Attachments.Single(a => a.Filename == "Practice Transitions.csv"));
        }
    }
}
