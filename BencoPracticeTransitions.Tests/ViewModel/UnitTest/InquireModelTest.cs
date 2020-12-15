using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using AutoFixture;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.Tests.Helpers;
using BencoPracticeTransitions.ViewModels.JobListing;
using Xunit;

namespace BencoPracticeTransitions.Tests.ViewModel.UnitTest
{
    public class InquireModelTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenFirstNameIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.FirstName = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenLastNameIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.LastName = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenAddressIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.Address = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalid")] //not an email                                  
        [InlineData("Abc.example.com")] //no @ character                        
        [InlineData("A@b@c@example.com")] //only one @ is allowed                   
        public void InquireModel_WhenContactEmailIsInvalid_AddsErrorToValidationResults(string email)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ContactEmail = email;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData(" test@test.com ")] //space at beginning and end
        [InlineData(" test@test.com")] //space at beginning
        [InlineData("test@tes.com ")] //space at end
        [InlineData("test_tes@test.com")] //has underscore
        [InlineData("very.common@example.com")] //has dot but not in the first or last character in local part
        [InlineData("very.common.word@example.com")] //has two dots but not consecutive
        [InlineData("other.email-with-dash@example.com")] //has dot and dash special characters
        [InlineData("x@example.com")] //1 letter local part
        [InlineData("a!#$%&'*+-/=?^_`{|}~a@example.org")] //includes special characters except the first letter and last letter
        [InlineData("example@s.solutions")]
        [InlineData("example-indeed@strange-example.com")] //has dash in local and domain part
        [InlineData("example-indeed@example.com")] //has dash in local and domain part
        [InlineData("abc_def@ghijk.co")] //2 letter domain 
        [InlineData("test@test.com.us")] // 2 domain
        [InlineData("_test@test.com")] //starts with special character
        [InlineData("john..doe@example.com")] //double dot before @
        [InlineData("john.doe@example..com")] //double dot after @
        [InlineData("test.@example.com")] //dot as last character in local part
        [InlineData(".test@test.com")] //dot as first character in local part
        [InlineData("test_@example.com")] //special character in end of local part
        [InlineData("test@example.c")] //1 letter for domain
        [InlineData("test@me")] //no domain
        [InlineData("test@test.com.us.com")] //3 domain 
        [InlineData("test@test.c12.c1")] //digits in domain
        [InlineData("test@tes_t.com.us")] //special character _ in server part
        [InlineData("test@test.com_co")] //special character _ in domain
        [InlineData("test@test.testingmorethanten.ph")] //more than 10 characters for domain
        public void InquireModel_WhenContactEmailIsValid_DoesNotAddErrorToValidationResults(string email)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ContactEmail = email;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenWorkExperienceIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.WorkExperience = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenWorkExperienceOtherIsInvalid_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.WorkExperienceOther = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenJobTypeIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.JobType = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenJobLocationIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.JobLocation = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }

        [Fact]
        public void InquireModel_WhenAvailabilityIsNull_ReturnsAvailabilityAsEmptyListOfInquireAvailabilityModel()
        {
            var sut = CreateRandomInquireModel();
            sut.Availability = null;

            Assert.IsType<List<InquireAvailabilityModel>>(sut.Availability);
            Assert.Empty(sut.Availability);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenSpecialtyIsInvalid_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.Specialty = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(@"http://www.linkedin.com/in/QE-:%QWM-sA5FwhQu'd</")]
        [InlineData(@"linkedin.com/in/ZJ/")]
        [InlineData(@"linkedin.com/in/[+.NZ%Rd.cBltrfg0'7tE;")]
        [InlineData(@"www.linkedin.com/in/0N%x'R/")]
        [InlineData(@"www.linkedin.com/in/yB$5TAV/>H\9(_TxO-=")]
        [InlineData(@"http://linkedin.com/in/pp*@CC(y+6vo<wQ/")]
        [InlineData(@"https://linkedin.com/in/j)]4 9N[iyRz1!]G%za(es=?ad")]
        [InlineData(@"https://www.linkedin.com/in/Yl%GMR:*h\79U]zFgq``-xiLzc")]
        [InlineData(@"http://www.linkedin.com/in/EvLQ`#79d51")]
        [InlineData(@"www.linkedin.com/in/Wt)@enIiZGHC")]
        [InlineData(@"www.linkedin.com/in/sample")]
        public static void InquireModel_WhenLinkedInAccountIsValidUrl_DoesNotAddErorrToValidationResults(string linkedInUrl)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.LinkedInAccount = linkedInUrl;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(@"www.linkedin.com")]
        [InlineData(@"www.linkedin.com/in/")]
        [InlineData(@"http://www.linkedin.com")]
        [InlineData(@"http://www.linkedin.com/in/")]
        [InlineData(@"https://www.linkedin.com")]
        [InlineData(@"https://www.linkedin.com/in/")]
        [InlineData(@"http:/www.linkedin.com/in/test")]
        [InlineData(@".linkedin.com / in / ZJ /")]
        [InlineData(@"w.linkedin.com / in / ZJ /")]
        [InlineData(@"/invalidUrl")]
        [InlineData(@"    www.linkedin.com/in/sample   ")]
        [InlineData(@"    https://www.linkedin.com/in/sample    ")]
        [InlineData(@"    http://www.linkedin.com/in/sample    ")]
        [InlineData(@"     linkedin.com/in/ZJ/     ")]
        public static void InquireModel_WhenLinkedInAccountIsNotValidUrl_AddsErorrToValidationResults(string linkedInUrl)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.LinkedInAccount = linkedInUrl;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenAdditionalNotesIsInvalid_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.AdditionalNotes = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Fact]
        public void InquireModel_WhenAdditionalNoteLengthIsEqualMaxLengthAllowed_DoesNotAddErrorToValidationResults()
        {
            var notesMaxLengthAllowed = 1_000_000;
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            var fixture = new Fixture();
            sut.AdditionalNotes = new string('a', notesMaxLengthAllowed);
            Assert.Equal(notesMaxLengthAllowed, sut.AdditionalNotes.Length);

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Fact]
        public void InquireModel_WhenAdditionalNoteLengthIsGreaterThanMaxLengthAllowed_AddsErrorToValidationResults()
        {
            var notesMaxLengthAllowed = 1_000_000;
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            var fixture = new Fixture();
            sut.AdditionalNotes = new string('a', notesMaxLengthAllowed + 1);
            Assert.True(sut.AdditionalNotes.Length > notesMaxLengthAllowed);

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenHowDidYouHearAboutUsIsInvalid_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.HowDidYouHearAboutUs = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InquireModel_WhenHowDidYouHearAboutUsDetailIsInvalid_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomInquireModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.HowDidYouHearAboutUsDetail = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Fact]
        public void InquireModel_WhenHowDidYouHearAboutUsDetailIsNotNull_ReturnsReferredByAsConcatenateOfHowDidYouHearAboutUsAndDetails()
        {
            var sut = CreateRandomInquireModel();
            Assert.True(sut.HowDidYouHearAboutUsDetail != null);

            Assert.Equal($"{sut.HowDidYouHearAboutUs} / {sut.HowDidYouHearAboutUsDetail}", sut.ReferredBy);
        }

        private static InquireModel CreateRandomInquireModel()
        {
            var fixture = new Fixture();

            var model = fixture.Build<InquireModel>()
                .With(m => m.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .With(m => m.ContactEmail, fixture.Create<MailAddress>().Address)
                .With(m => m.Resume, fixture.Build<EmailAttachment>().Without(x => x.Data).Create())
                .Create();

            return model;
        }
        public void InquireModel_WhenContactPhoneIsNull_AddsErrorToValidationResults()
        {
            var fixture = new Fixture();
            var sut = fixture.Build<InquireModel>()
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .Without(x => x.Resume)
                .Create();

            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ContactNumber = null;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData("23456789012")] // if phone is 11 digits but does not start with 1
        [InlineData("234567890")] // too few digits
        [InlineData("234.567.8901")] // "." is not a supported separator
        public void InquireModel_WhenContactPhoneIsInvalid_AddsErrorToValidationResults(string phone)
        {
            var fixture = new Fixture();
            var sut = fixture.Build<InquireModel>()
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .Without(x => x.Resume)
                .Create();

            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ContactNumber = phone;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData("1-234-567-8901")] // 1-###-###-#### format
        [InlineData("12345678901")] // if phone is 11 digits and has 1 in the beginning
        [InlineData("1-234567-8901")] // 1-######-#### format
        [InlineData("1-234-5678901")] // 1-###-####### format
        [InlineData("234-567-8901")] // ###-###-#### format
        [InlineData("2345678901")] // if phone is 10 digits, without the country code (1) ##########
        [InlineData("(234)567-8901")] // (###)###-#### format
        [InlineData("(234) 567-8901")] // (###) ###-#### format
        public void InquireModel_WhenPhoneNumberIsValid_DoesNotAddErrorToValidationResults(string phone)
        {
            var fixture = new Fixture();
            var sut = fixture.Build<InquireModel>()
                .With(x => x.ContactNumber, RandomDataGenerator.RandomPhoneNumber)
                .With(x => x.ContactEmail, fixture.Create<MailAddress>().Address)
                .Without(x => x.Resume)
                .Create();

            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ContactNumber = phone;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }
    }
}
