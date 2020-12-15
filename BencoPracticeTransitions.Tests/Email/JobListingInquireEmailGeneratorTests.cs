using System;
using System.Collections.Generic;
using System.IO;
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
    public class JobListingInquireEmailGeneratorTests
    {
        [Fact]
        public void CanCreateJobListingInquireEmailGenerator()
        {
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            // ReSharper disable once UnusedVariable
            var sut = new JobListingInquireEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);
        }

        [Fact]
        public void CanGenerateEmailFromModel_WhenPassedModelThatIsNotOfTypeInquireModel_ReturnsFalse()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new JobListingInquireEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.False(sut.CanGenerateEmailFromModel(new SellPracticeModel()));
        }


        [Fact]
        public void CanGenerateEmailFromModel_WhenPassedModelThatIsOfTypeInquireModel_ReturnsTrue()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new JobListingInquireEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.True(sut.CanGenerateEmailFromModel(new InquireModel()));
        }


        [Fact]
        public void GenerateEmail_WhenPassedModelIsOfTypeInquireModel_Succeeds()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new JobListingInquireEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

            Assert.NotNull(sut.GenerateEmail(new InquireModel()));
        }

        [Fact]
        public void Generate_WhenPassedModelIsNotOfTypeInquireModel_ThrowsArgumentException()
        {
            var razorEngine = new Mock<IRazorLightEngine>().Object;
            var attachmentGenerator = new Mock<IGenerateEmailAttachment>().Object;
            var fixture = new Fixture();
            var mailSettings = fixture
                .Build<BencoEmailMessageSettings>()
                .Create();
            var mockMailSettingsOptions = new Mock<IOptions<BencoEmailMessageSettings>>();
            mockMailSettingsOptions.Setup(m => m.Value).Returns(mailSettings);

            var sut = new JobListingInquireEmailGenerator(razorEngine, attachmentGenerator, mockMailSettingsOptions.Object);

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

            var sut = new JobListingInquireEmailGenerator(razorEngine, attachmentGenerator.Object, mockMailSettingsOptions.Object);

            var request = sut.GenerateEmail(new InquireModel());

            Assert.NotNull(request.Attachments.Single(a => a.Filename == "Practice Transitions.csv"));
        }


        [Fact]
        public void GenerateEmail_WhenModelWithResume_AttachesResumeToEmail()
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

            var model = fixture.Build<InquireModel>()
                .Without(m => m.ContactNumber)
                .With(m => m.Resume, new EmailAttachment { Data = new MemoryStream(), Filename = "Resume.pdf"})
                .Create();
            var sut = new JobListingInquireEmailGenerator(razorEngine, attachmentGenerator.Object, mockMailSettingsOptions.Object);

            var request = sut.GenerateEmail(model);
            var resume = request.Attachments.Single(a => a.Filename == "Resume.pdf");

            Assert.NotNull(resume);
            Assert.Equal("Resume.pdf", resume.Filename);
            Assert.NotNull(resume.Data);
        }

    }
}
