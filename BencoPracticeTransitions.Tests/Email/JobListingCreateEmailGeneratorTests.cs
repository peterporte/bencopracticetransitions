using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.ViewModels.JobListing;
using BencoPracticeTransitions.ViewModels.Practice;
using Microsoft.Extensions.Options;
using Moq;
using RazorLight;
using Xunit;

namespace BencoPracticeTransitions.Tests.Email
{
    public class JobListingCreateEmailGeneratorTests
    {
        [Fact]
        public void CanCreateJobListingCreatEmailGenerator()
        {
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock< IOptions < BencoEmailMessageSettings >>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            // ReSharper disable once UnusedVariable
            var sut = new JobListingCreateEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);
        }

        [Fact]
        public void CanGenerateEmailFromModel_WhenPassedModelThatIsNotOfTypeCreateModel_ReturnsFalse()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new JobListingCreateEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.False(sut.CanGenerateEmailFromModel(new SellPracticeModel()));
        }


        [Fact]
        public void CanGenerateEmailFromModel_WhenPassedModelThatIsOfTypeCreateModel_ReturnsTrue()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new JobListingCreateEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.True(sut.CanGenerateEmailFromModel(new CreateModel()));
        }


        [Fact]
        public void GenerateEmail_WhenPassedModelIsOfTypeCreateModel_Succeeds()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new JobListingCreateEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.NotNull(sut.GenerateEmail(new CreateModel()));
        }

        [Fact]
        public void Generate_WhenPassedModelIsNotOfTypeCreateModel_ThrowsArgumentException()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new JobListingCreateEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.Throws<ArgumentException>(() => sut.GenerateEmail(new SellPracticeModel()));
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

            var sut = new JobListingCreateEmailGenerator(razorEngine, attachmentGenerator.Object, mockMailSettingsOptions.Object);

            var request = sut.GenerateEmail(new CreateModel());

            Assert.NotNull(request.Attachments.Single(a => a.Filename == "Practice Transitions.csv"));
        }
    }
}
