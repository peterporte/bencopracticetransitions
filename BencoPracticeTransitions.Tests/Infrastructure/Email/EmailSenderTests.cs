using System;
using System.IO;
using System.Linq;
using AutoFixture;
using BencoPracticeTransitions.Infrastructure.Email;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BencoPracticeTransitions.Tests.Infrastructure.Email
{
    public class EmailSenderTests
    {

        [Fact]
        public void CanCreateEmailSender()
        {
            var mailSettings = new BencoSmtpSettings();
            var mockOptions = new Mock<IOptions<BencoSmtpSettings>>();
            mockOptions.Setup(m => m.Value)
                .Returns(mailSettings);

            // ReSharper disable once UnusedVariable
            var sut = new EmailSender(mockOptions.Object);
        }


        [Fact]
        public void Create_WhenPassedNull_ThrowsArgumentNullException()
        {
            var mailSettings = new BencoSmtpSettings();
            var mockOptions = new Mock<IOptions<BencoSmtpSettings>>();
            mockOptions.Setup(m => m.Value)
                .Returns(mailSettings);

            var sut = new EmailSender(mockOptions.Object);

            Assert.Throws<ArgumentNullException>(() => sut.CreateEmail(null));
        }


        [Fact]
        public void Create_WhenOnProductionAndPassedValidEmailRequest_ReturnsMailMessageWithValidFrom()
        {
            var mailSettings = new BencoSmtpSettings();

            var mockOptions = new Mock<IOptions<BencoSmtpSettings>>();
            mockOptions.Setup(m => m.Value)
                .Returns(mailSettings);
            var fixture = new Fixture();
            var emailRequest = fixture
                .Build<EmailRequest>()
                .Without(s => s.Attachments)
                .With(s => s.From , "test@benco.com")
                .With(s => s.To, new [] {"anotherTest@benco.com", "yetAnotherTest@benco.com"})
                .With(s => s.Cc, new string[] {})
                .With(s => s.Bcc, new string[] { })
                .With(s => s.ReplyTo, new[] { "replyto@benco.com" })
                .Create();

            var sut = new EmailSender(mockOptions.Object);

            var actualResult = sut.CreateEmail(emailRequest);

            Assert.Equal(emailRequest.From, actualResult.From.Address);
        }


        [Fact]
        public void Create_WhenPassedValidEmailRequest_ReturnsMailMessageCorrectToCcBccAndReplyTo()
        {
            var mailSettings = new BencoSmtpSettings();

            var mockOptions = new Mock<IOptions<BencoSmtpSettings>>();
            mockOptions.Setup(m => m.Value)
                .Returns(mailSettings);
            var fixture = new Fixture();
            var emailRequest = fixture
                .Build<EmailRequest>()
                .Without(s => s.Attachments)
                .With(s => s.From, "test@benco.com")
                .With(s => s.To, new[] { "anotherTest@benco.com", "yetAnotherTest@benco.com" })
                .With(s => s.Cc, new[] { "cc@benco.com" })
                .With(s => s.Bcc, new[] { "bcc@benco.com" })
                .With(s => s.ReplyTo, new []{ "replyto@benco.com"})
                .Create();

            var sut = new EmailSender(mockOptions.Object);

            var actualResult = sut.CreateEmail(emailRequest);

            Assert.Equal(new[] { "anotherTest@benco.com", "yetAnotherTest@benco.com" }, actualResult.To.Select(s => s.Address));
            Assert.Equal(new[] { "cc@benco.com" }, actualResult.CC.Select(s => s.Address));
            Assert.Equal(new[] { "bcc@benco.com" }, actualResult.Bcc.Select(s => s.Address));
            Assert.Equal(new[] { "replyto@benco.com" }, actualResult.ReplyToList.Select(s => s.Address));

        }

        [Fact]
        public void Create_WhenPassedRequestWithNullReplyTo_CreatesMessageWithoutReplyTo()
        {
            var mailSettings = new BencoSmtpSettings();

            var mockOptions = new Mock<IOptions<BencoSmtpSettings>>();
            mockOptions.Setup(m => m.Value)
                .Returns(mailSettings);
            var fixture = new Fixture();
            var emailRequest = fixture
                .Build<EmailRequest>()
                .Without(s => s.Attachments)
                .With(s => s.From, "test@benco.com")
                .With(s => s.To, new[] { "anotherTest@benco.com", "yetAnotherTest@benco.com" })
                .With(s => s.Cc, new[] { "cc@benco.com" })
                .With(s => s.Bcc, new[] { "bcc@benco.com" })
                .With(s => s.ReplyTo, (string[])null)
                .Create();

            var sut = new EmailSender(mockOptions.Object);

            var actualResult = sut.CreateEmail(emailRequest);

            Assert.Equal(new[] { "anotherTest@benco.com", "yetAnotherTest@benco.com" }, actualResult.To.Select(s => s.Address));
            Assert.Equal(new[] { "cc@benco.com" }, actualResult.CC.Select(s => s.Address));
            Assert.Equal(new[] { "bcc@benco.com" }, actualResult.Bcc.Select(s => s.Address));
            Assert.Equal(Enumerable.Empty<string>(), actualResult.ReplyToList.Select(s => s.Address).ToArray());

        }


        [Fact]
        public void Create_WhenPassedRequestWithNullBcc_CreatesMessageWithoutBcc()
        {
            var mailSettings = new BencoSmtpSettings();

            var mockOptions = new Mock<IOptions<BencoSmtpSettings>>();
            mockOptions.Setup(m => m.Value)
                .Returns(mailSettings);
            var fixture = new Fixture();
            var emailRequest = fixture
                .Build<EmailRequest>()
                .Without(s => s.Attachments)
                .With(s => s.From, "test@benco.com")
                .With(s => s.To, new[] { "anotherTest@benco.com", "yetAnotherTest@benco.com" })
                .With(s => s.Cc, new[] { "cc@benco.com" })
                .With(s => s.Bcc, (string[]) null)
                .With(s => s.ReplyTo, new[] { "replyto@benco.com" })
                .Create();

            var sut = new EmailSender(mockOptions.Object);

            var actualResult = sut.CreateEmail(emailRequest);

            Assert.Equal(new[] { "anotherTest@benco.com", "yetAnotherTest@benco.com" }, actualResult.To.Select(s => s.Address));
            Assert.Equal(new[] { "cc@benco.com" }, actualResult.CC.Select(s => s.Address));
            Assert.Equal(Enumerable.Empty<string>(), actualResult.Bcc.Select(s => s.Address));
            Assert.Equal(new[] { "replyto@benco.com" }, actualResult.ReplyToList.Select(s => s.Address));

        }

        [Fact]
        public void Create_WhenPassedRequestWithNullCc_CreatesMessageWithoutCc()
        {
            var mailSettings = new BencoSmtpSettings();

            var mockOptions = new Mock<IOptions<BencoSmtpSettings>>();
            mockOptions.Setup(m => m.Value)
                .Returns(mailSettings);
            var fixture = new Fixture();
            var emailRequest = fixture
                .Build<EmailRequest>()
                .Without(s => s.Attachments)
                .With(s => s.From, "test@benco.com")
                .With(s => s.To, new[] { "anotherTest@benco.com", "yetAnotherTest@benco.com" })
                .With(s => s.Cc, (string[])null) 
                .With(s => s.Bcc, new[] { "bcc@benco.com" })
                .With(s => s.ReplyTo, new[] { "replyto@benco.com" })
                .Create();

            var sut = new EmailSender(mockOptions.Object);

            var actualResult = sut.CreateEmail(emailRequest);

            Assert.Equal(new[] { "anotherTest@benco.com", "yetAnotherTest@benco.com" }, actualResult.To.Select(s => s.Address));
            Assert.Equal(Enumerable.Empty<string>(), actualResult.CC.Select(s => s.Address));
            Assert.Equal(new[] { "bcc@benco.com" }, actualResult.Bcc.Select(s => s.Address));
            Assert.Equal(new[] { "replyto@benco.com" }, actualResult.ReplyToList.Select(s => s.Address));

        }

        [Fact]
        public void Create_WhenPassedRequestWithNullTo_CreatesMessageWithoutTo()
        {
            var mailSettings = new BencoSmtpSettings();

            var mockOptions = new Mock<IOptions<BencoSmtpSettings>>();
            mockOptions.Setup(m => m.Value)
                .Returns(mailSettings);
            var fixture = new Fixture();
            var emailRequest = fixture
                .Build<EmailRequest>()
                .Without(s => s.Attachments)
                .With(s => s.From, "test@benco.com")
                .With(s => s.To, (string[])null)
                .With(s => s.Cc, new[] { "cc@benco.com" })
                .With(s => s.Bcc, new[] { "bcc@benco.com" })
                .With(s => s.ReplyTo, new[] { "replyto@benco.com" })
                .Create();

            var sut = new EmailSender(mockOptions.Object);

            var actualResult = sut.CreateEmail(emailRequest);

            Assert.Equal(Enumerable.Empty<string>(), actualResult.To.Select(s => s.Address));
            Assert.Equal(new[] { "cc@benco.com" }, actualResult.CC.Select(s => s.Address));
            Assert.Equal(new[] { "bcc@benco.com" }, actualResult.Bcc.Select(s => s.Address));
            Assert.Equal(new[] { "replyto@benco.com" }, actualResult.ReplyToList.Select(s => s.Address));

        }

        [Fact]
        public void Create_WhenPassedRequestWithNullFrom_ThrowsArgumentNullException()
        {
            var mailSettings = new BencoSmtpSettings();

            var mockOptions = new Mock<IOptions<BencoSmtpSettings>>();
            mockOptions.Setup(m => m.Value)
                .Returns(mailSettings);
            var fixture = new Fixture();
            var emailRequest = fixture
                .Build<EmailRequest>()
                .Without(s => s.Attachments)
                .With(s => s.From, (string)null)
                .With(s => s.To, new[] { "anotherTest@benco.com", "yetAnotherTest@benco.com" })
                .With(s => s.Cc, new[] { "cc@benco.com" })
                .With(s => s.Bcc, new[] { "bcc@benco.com" })
                .With(s => s.ReplyTo, new[] { "replyto@benco.com" })
                .Create();

            var sut = new EmailSender(mockOptions.Object);

            var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateEmail(emailRequest));

            Assert.Equal("address", exception.ParamName);
        }



        [Fact]
        public void Create_WhenPassedRequestWithAttachments_CreatesMessageWithAttachments()
        {
            var mailSettings = new BencoSmtpSettings();

            var mockOptions = new Mock<IOptions<BencoSmtpSettings>>();
            mockOptions.Setup(m => m.Value)
                .Returns(mailSettings);

            var attachments = new[]
            {
                new EmailAttachment
                {
                    Filename = "spreadsheet.csv",
                    Data = new MemoryStream()
                },
                new EmailAttachment
                {
                    Filename = "Resume.pdf",
                    Data = new MemoryStream()
                }
            };

            var fixture = new Fixture();
            var emailRequest = fixture
                .Build<EmailRequest>()
                .With(s => s.From, "test@benco.com")
                .With(s => s.To, new[] { "anotherTest@benco.com", "yetAnotherTest@benco.com" })
                .With(s => s.Cc, (string[])null)
                .With(s => s.Bcc, (string[])null)
                .With(s => s.ReplyTo, (string[])null)
                .With(s => s.Attachments, attachments)
                .Create();

            var sut = new EmailSender(mockOptions.Object);

            var actualResult = sut.CreateEmail(emailRequest);

            Assert.Equal(2,actualResult.Attachments.Count);
        }

    }
}
