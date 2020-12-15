using BencoPracticeTransitions.Tests.Helpers;
using BencoPracticeTransitions.ViewModels.Practice;
using System.Linq;
using System.Net.Mail;
using AutoFixture;
using Xunit;

namespace BencoPracticeTransitions.Tests.ViewModel.UnitTest
{
    public class SellPracticeModelTest
    {

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SellPracticeModel_WhenPracticeNameIsInValid_FailsValidation(string practiceName)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.PracticeName = practiceName;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SellPracticeModel_WhenCityIsInValid_FailsValidation(string city)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.City = city;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SellPracticeModel_WhenStateIsInValid_FailsValidation(string state)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.State = state;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("1234")]
        [InlineData("-12345")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public void SellPracticeModel_WhenZipCodeIsInValid_FailsValidation(string zipCode)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.ZipCode = zipCode;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("12345-1234")]
        [InlineData("25484")]
        [InlineData(" 25484 ")]
        public void SellPracticeModel_WhenZipCodeIsValid_ReturnsTrue(string zipCode)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.ZipCode = zipCode;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 0);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SellPracticeModel_WhenPracticeTypeIsInValid_FailsValidation(string practiceType)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.PracticeType = practiceType;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SellPracticeModel_WhenContactFirstNameIsInValid_FailsValidation(string contactFirstName)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.ContactFirstName = contactFirstName;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SellPracticeModel_WhenContactLastNameIsInValid_FailsValidation(string contactLastName)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.ContactLastName = contactLastName;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("754-3010")] //incomplete
        [InlineData("+541 754 3010")] //missing +1
        [InlineData("(01)541-754-3010")] //(01) format
        [InlineData("23456789012")] // if phone is 11 digits but does not start with 1
        [InlineData("234567890")] // too few digits
        [InlineData("234.567.8901")] // "." is not a supported separator
        public void SellPracticeModel_WhenContactPhoneIsInvalid_FailsValidation(string contactPhoneNumber)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.ContactPhone = contactPhoneNumber;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData("1-(541)-754-3010")]// 1-(###)-###-#### format
        [InlineData("(541) 754 3010")]// (###)) ### #### format
        [InlineData("1541-7543010")] // ####-####### format
        [InlineData(" 541-7865210 ")] // ###-####### format
        [InlineData("+1-(541)-754-3010")] // +1-(###)-###-####
        [InlineData("1-234-567-8901")] // 1-###-###-#### format
        [InlineData("12345678901")] // if phone is 11 digits and has 1 in the beginning
        [InlineData("1-234567-8901")] // 1-######-#### format
        [InlineData("1-234-5678901")] // 1-###-####### format
        [InlineData("234-567-8901")] // ###-###-#### format
        [InlineData("2345678901")] // if phone is 10 digits, without the country code (1) ##########
        [InlineData("(234)567-8901")] // (###)###-#### format
        [InlineData("(234) 567-8901")] // (###) ###-#### format
        public void SellPracticeModel_WhenContactPhoneIsValid_ReturnsTrue(string contactPhoneNumber)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.ContactPhone = contactPhoneNumber;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Theory]
        [InlineData("")]
        [InlineData("mc@1234@gmail.com")]
        [InlineData("email.gmail@mail.mail@com")]
        [InlineData(null)]
        [InlineData("invalid")] //not an email                                  
        [InlineData("Abc.example.com")] //no @ character                        
        [InlineData("A@b@c@example.com")] //only one @ is allowed    
        public void SellPracticeModel_WhenContactEmailIsInvalid_FailsValidation(string contactEmail)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.ContactEmail = contactEmail;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
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
        public void SellPracticeModel_WhenContactEmailIsValid_ReturnsTrue(string contactEmail)
        {
            var sut = CreateRandomSellPracticeModel();
            sut.ContactEmail = contactEmail;

            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }


        [Fact]
        public void SellPracticeModel_WhenAskingPriceIsNull_AddsErrorToValidationResults()
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.AskingPrice = null;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData("-0.01")] // Too small
        [InlineData("10000000.01")] // To large
        public void SellPracticeModel_WhenAskingPriceIsInvalid_AddsErrorToValidationResults(string testValue)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.AskingPrice = decimal.Parse(testValue);

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }

        [Fact]
        public void SellPracticeModel_WhenIsAppraisalNeededIsNull_FailsValidation()
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.IsAppraisalNeeded = null;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Fact]
        public void SellPracticeModel_WhenCollectionsAmountIsInNull_FailsValidation()
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.CollectionsAmount = null;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SellPracticeModel_WhenRealEstateOptionIsInValid_FailsValidation(string realEstateOption)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.RealEstateOption = realEstateOption;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData(1001)]
        [InlineData(-1)]
        [InlineData(null)]
        public void SellPracticeModel_WhenWorkingOperatoryCountIsInValid_FailsValidation(int? operatoryCount)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.WorkingOperatoryCount = operatoryCount;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(999)]
        [InlineData(15)]
        [InlineData(1)]
        public void SellPracticeModel_WhenWorkingOperatoryCountIsValid_ReturnsTrue(int operatoryCount)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.WorkingOperatoryCount = operatoryCount;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(1001)]
        public void SellPracticeModel_WhenExpandableOperatoryCountIsInValid_FailsValidation(int? expandableOperatoryCount)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ExpandableOperatoryCount = expandableOperatoryCount;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }


        [Theory]
        [InlineData(999)]
        [InlineData(15)]
        [InlineData(0)]
        [InlineData(null)]
        public void SellPracticeModel_WhenExpandableOperatoryCountIsValid_ReturnsTrue(int? expandableOperatoryCount)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ExpandableOperatoryCount = expandableOperatoryCount;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Fact]
        public void SellPracticeModel_WhenIsDoctorWillingToStayOnAfterTransitionIsNull_FailsValidation()
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.IsDoctorWillingToStayOnAfterTransition = null;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Any());
        }

        [Fact]
        public void SellPracticeModel_WhenIsDoctorWillingToStayOnAfterTransitionIsValid_ReturnsTrue()
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);

            Assert.False(validationResults.Any());
            sut.IsDoctorWillingToStayOnAfterTransition = true;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void SellPracticeModel_WhenAdditionalNotesIsInvalid_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.AdditionalNotes = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Fact]
        public void SellPracticeModel_WhenAdditionalNoteLengthIsEqualMaxLengthAllowed_DoesNotAddErrorToValidationResults()
        {
            var notesMaxLengthAllowed = 1_000_000;
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            var fixture = new Fixture();
            sut.AdditionalNotes = new string('a', notesMaxLengthAllowed);
            Assert.Equal(notesMaxLengthAllowed, sut.AdditionalNotes.Length);

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Fact]
        public void SellPracticeModel_WhenAdditionalNoteLengthIsGreaterThanMaxLengthAllowed_AddsErrorToValidationResults()
        {
            var notesMaxLengthAllowed = 1_000_000;
            var sut = CreateRandomSellPracticeModel();
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
        public void SellPracticeModel_WhenHowDidYouHearAboutUsIsInvalid_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomSellPracticeModel();
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
        public void SellPracticeModel_WhenHowDidYouHearAboutUsDetailIsNull_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomSellPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.HowDidYouHearAboutUsDetail = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Fact]
        public void SellPracticeModel_WhenHowDidYouHearAboutUsDetailIsNotNull_ReturnsReferredByAsConcatenateOfHowDidYouHearAboutUsAndDetails()
        {
            var sut = CreateRandomSellPracticeModel();
            Assert.True(sut.HowDidYouHearAboutUsDetail != null);

            Assert.Equal($"{sut.HowDidYouHearAboutUs} / {sut.HowDidYouHearAboutUsDetail}", sut.ReferredBy);
        }


        private static SellPracticeModel CreateRandomSellPracticeModel()
        {
            var collectionsAmountValues = new[]
                {"$0 - $500,000", "$500,000 - $1,000,000", "$1,500,000 - $2,000,000", "$2,000,000 +"};

            var fixture = new Fixture();

            var model = fixture.Build<SellPracticeModel>()
                .With(m => m.CollectionsAmount,
                    collectionsAmountValues[RandomDataGenerator.RandomInt(0, collectionsAmountValues.Length - 1)])
                .With(m => m.ContactPhone, RandomDataGenerator.RandomPhoneNumber)
                .With(m => m.ContactEmail, fixture.Create<MailAddress>().Address)
                .Create();

            return model;
        }
    }
}
