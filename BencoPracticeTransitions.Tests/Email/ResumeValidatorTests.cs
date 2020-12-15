using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace BencoPracticeTransitions.Tests.Email
{
    public class ResumeValidatorTests
    {
        [Fact]
        public void CanCreateResumeValidator()
        {

            // ReSharper disable once UnusedVariable
            var sut = new ResumeValidator();
        }

        [Theory]
        [InlineData("txt")]
        [InlineData("csv")]
        [InlineData("png")]
        [InlineData("jpg")]
        public void IsFileExtensionAllowedAsEmailAttachment_WhenFileExtensionPassedIsNotAllowed_ReturnFalse(string fileExtension)
        {
            var sut = new ResumeValidator();
            Assert.False(sut.IsFileExtensionAllowedAsEmailAttachment(fileExtension));
        }

        [Theory]
        [InlineData("doc")]
        [InlineData("docx")]
        [InlineData("pdf")]
        public void IsFileExtensionAllowedAsEmailAttachment_WhenFileExtensionPassedIsAllowed_ReturnTrue(string fileExtension)
        {
            var sut = new ResumeValidator();
            Assert.True(sut.IsFileExtensionAllowedAsEmailAttachment(fileExtension));
        }

        [Theory]
        [InlineData("Test.txt")]
        [InlineData("Test.png")]
        [InlineData("Test.csv")]
        [InlineData("Test.jpg")]
        public void UploadedResumeIsValid_WhenFileExtensionNotAllowed_ReturnFalse(string fileName)
        {
            var sut = new ResumeValidator();
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(m => m.FileName)
                .Returns(fileName);
            mockFile.Setup(m => m.Length)
                .Returns(RandomDataGenerator.RandomLong(1, 2100000));
            var file = mockFile.Object;

            Assert.False(sut.UploadedResumeIsValid(file, out _));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1000)]
        public void UploadedResumeIsValid_WhenFileSizeIsNotValid_ReturnTrue(long fileSize)
        {
            var sut = new ResumeValidator();
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(m => m.FileName)
                .Returns("Test.doc");
            mockFile.Setup(m => m.Length)
                .Returns(fileSize);
            var file = mockFile.Object;

            Assert.True(sut.UploadedResumeIsValid(file, out _)); //should it be returning false?
        }


        [Theory]
        [InlineData("test.doc")]
        [InlineData("t3$+.docx")]
        [InlineData("123.pdf")]
        [InlineData("test123..123.pdf")]
        public void UploadedResumeIsValid_WhenFileExtensionAllowedAndFileSizeGreaterThanFileSizeLimit_ReturnFalse(string fileName)
        {
            var sut = new ResumeValidator();
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(m => m.FileName)
                .Returns(fileName);
            mockFile.Setup(m => m.Length)
                .Returns(RandomDataGenerator.RandomLong(2100001, long.MaxValue));
            var file = mockFile.Object;

            Assert.False(sut.UploadedResumeIsValid(file, out _));
        }

        [Theory]
        [InlineData("test.doc")]
        [InlineData("t3$+.docx")]
        [InlineData("123.pdf")]
        [InlineData("test123..123.pdf")]
        public void UploadedResumeIsValid_WhenFileExtensionAllowedAndFileSizeLessThanOrEqualToFileSizeLimit_ReturnTrue(string fileName)
        {
            var sut = new ResumeValidator();
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(m => m.FileName)
                .Returns(fileName);
            mockFile.Setup(m => m.Length)
                .Returns(RandomDataGenerator.RandomLong(1, 2100000));
            var file = mockFile.Object;

            Assert.True(sut.UploadedResumeIsValid(file, out _));
        }
    }
}
