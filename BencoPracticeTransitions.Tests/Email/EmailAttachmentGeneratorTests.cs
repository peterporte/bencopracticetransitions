using System;
using System.Collections.Generic;
using System.IO;
using BencoPracticeTransitions.Email;
using Xunit;

namespace BencoPracticeTransitions.Tests.Email
{
    public class EmailAttachmentGeneratorTests
    {

        [Fact]
        public void CanCreateGenerateCsvEmailAttachment()
        {
            // ReSharper disable once UnusedVariable
            var sut = new EmailAttachmentGenerator();
        }


        [Fact]
        public void GenerateCsvEmailAttachment_WhenPassedNullColumns_ThrowsArgumentNullException()
        {

            var sut = new EmailAttachmentGenerator();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.GenerateCsvEmailAttachment(null, "filename.txt"));
            Assert.Equal("columns", exception.ParamName);
        }


        [Fact]
        public void GenerateCsvEmailAttachment_WhenPassedEmptyColumns_ExecutesWithoutError()
        {
            var columns = new List<KeyValuePair<string, string>>();

            var sut = new EmailAttachmentGenerator();

            sut.GenerateCsvEmailAttachment(columns, "filename.txt");
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GenerateCsvEmailAttachment_WhenPassedInvalidFileName_ThrowsArgumentException(string filename)
        {
            var columns = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Column", "Value")
            };
            var sut = new EmailAttachmentGenerator();

            var exception = Assert.Throws<ArgumentException>(() => sut.GenerateCsvEmailAttachment(columns, filename));
            Assert.Equal("filename", exception.ParamName);
        }

        [Fact]
        public void GenerateCsvEmailAttachment_WhenPassedValidFileName_ReturnsAttachmentWithSpecifiedName()
        {
            var columns = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Column", "Value")
            };
            const string fileName = "fileName.txt";

            var sut = new EmailAttachmentGenerator();

            var result = sut.GenerateCsvEmailAttachment(columns, fileName);
            Assert.Equal(fileName, result.Filename);
        }

        [Fact]
        public void GenerateCsvEmailAttachment_WhenPassedValidColumns_ReturnsAttachmentWithExpectedData()
        {
            var columns = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Column1", "Value1"),
                new KeyValuePair<string, string>("Column2", "Value2")
            };
            const string fileName = "fileName.txt";

            var sut = new EmailAttachmentGenerator();

            var result = sut.GenerateCsvEmailAttachment(columns, fileName);
            var reader =new StreamReader( result.Data);
            var actualFileContents = reader.ReadToEnd();

            const string expectedFileContents = "\"Column1\",\"Column2\"\n\"Value1\",\"Value2\"\n";
            Assert.Equal(expectedFileContents, actualFileContents);
        }

        [Fact]
        public void GenerateCsvEmailAttachment_WhenPassedColumnWithNullValue_ReturnsAttachmentWithExpectedData()
        {
            var columns = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Column1", null),
                new KeyValuePair<string, string>("Column2", "Value2")
            };
            const string fileName = "fileName.txt";

            var sut = new EmailAttachmentGenerator();

            var result = sut.GenerateCsvEmailAttachment(columns, fileName);
            var reader = new StreamReader(result.Data);
            var actualFileContents = reader.ReadToEnd();

            const string expectedFileContents = "\"Column1\",\"Column2\"\n\"\",\"Value2\"\n";
            Assert.Equal(expectedFileContents, actualFileContents);
        }

        [Fact]
        public void GenerateCsvEmailAttachment_WhenPassedColumnWithNullKey_ReturnsAttachmentWithExpectedData()
        {
            var columns = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(null, "Value1"),
                new KeyValuePair<string, string>("Column2", "Value2")
            };
            const string fileName = "fileName.txt";

            var sut = new EmailAttachmentGenerator();

            var result = sut.GenerateCsvEmailAttachment(columns, fileName);
            var reader = new StreamReader(result.Data);
            var actualFileContents = reader.ReadToEnd();

            const string expectedFileContents = "\"\",\"Column2\"\n\"Value1\",\"Value2\"\n";
            Assert.Equal(expectedFileContents, actualFileContents);
        }
    }
}
