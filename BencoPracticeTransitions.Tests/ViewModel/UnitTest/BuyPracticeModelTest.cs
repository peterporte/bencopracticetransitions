using System.Linq;
using System.Net.Mail;
using AutoFixture;
using BencoPracticeTransitions.Tests.Helpers;
using BencoPracticeTransitions.ViewModels.Practice;
using Xunit;

namespace BencoPracticeTransitions.Tests.ViewModel.UnitTest
{
    public class BuyPracticeModelTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BuyPracticeModel_WhenCollectionsAmountIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.CollectionsAmount = testData;
            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BuyPracticeModel_WhenContactFirstNameIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ContactFirstName = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BuyPracticeModel_WhenContactLastNameIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ContactLastName = testData;

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
        public void BuyPracticeModel_WhenContactEmailIsInvalid_AddsErrorToValidationResults(string email)
        {
            var sut = CreateRandomBuyPracticeModel();
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
        public void BuyPracticeModel_WhenContactEmailIsValid_DoesNotAddErrorToValidationResults(string email)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ContactEmail = email;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
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
        public void BuyPracticeModel_WhenContactPhoneIsInvalid_AddsErrorToValidationResults(string phone)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ContactPhoneNumber = phone;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
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
        public void BuyPracticeModel_WhenPhoneNumberIsValid_DoesNotAddErrorToValidationResults(string phone)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.ContactPhoneNumber = phone;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());         
        }
       

        [Fact]
        public void BuyPracticeModel_WhenIsDoctorWillingToStayOnAfterTransitionIsNull_AddsErrorToValidationResults()
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.IsDoctorWillingToStayOnAfterTransition = null;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData(null)]
        [InlineData(0)] // Too small
        [InlineData(1_001)] // To large
        public void BuyPracticeModel_WhenMinOperatoryCountIsInvalid_AddsErrorToValidationResults(int? testValue)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.MinOperatoryCount = testValue;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BuyPracticeModel_WhenRealEstateOptionIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.RealEstateOption = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BuyPracticeModel_WhenPracticeTypeIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.PracticeType = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BuyPracticeModel_WhenPurchaseLocationIsInvalid_AddsErrorToValidationResults(string testData)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.PurchaseLocation = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Fact]
        public void BuyPracticeModel_WhenMinPurchaseAmountIsNull_AddsErrorToValidationResults()
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.MinPurchaseAmount = null;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData("-0.01")] // Too small
        [InlineData("10000000.01")] // To large
        public void BuyPracticeModel_WhenMinPurchaseAmountIsInvalid_AddsErrorToValidationResults(string testValue)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.MinPurchaseAmount = decimal.Parse(testValue);

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }

        [Fact]
        public void BuyPracticeModel_WhenMaxPurchaseAmountIsNull_AddsErrorToValidationResults()
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.MaxPurchaseAmount = null;

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }



        [Theory]
        [InlineData("-0.01")] // Too small
        [InlineData("10000000.01")] // To large
        public void BuyPracticeModel_WhenMaxPurchaseAmountIsInvalid_AddsErrorToValidationResults(string testValue)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.MaxPurchaseAmount = decimal.Parse(testValue);

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BuyPracticeModel_WhenAdditionalNotesIsInvalid_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.AdditionalNotes = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Fact]
        public void BuyPracticeModel_WhenAdditionalNoteLengthIsEqualMaxLengthAllowed_DoesNotAddErrorToValidationResults()
        {
            var notesMaxLengthAllowed = 1_000_000;
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            var fixture = new Fixture();
            sut.AdditionalNotes = new string('1', notesMaxLengthAllowed);
            Assert.Equal(notesMaxLengthAllowed, sut.AdditionalNotes.Length);

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Fact]
        public void BuyPracticeModel_WhenAdditionalNoteLengthIsGreaterThanMaxLengthAllowed_AddsErrorToValidationResults()
        {
            var notesMaxLengthAllowed = 1_000_000;
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            var fixture = new Fixture();
            sut.AdditionalNotes = new string('1', notesMaxLengthAllowed + 1);
            Assert.True(sut.AdditionalNotes.Length > notesMaxLengthAllowed);

            validationResults = ModelValidator.Validate(sut);
            Assert.True(validationResults.Count == 1);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BuyPracticeModel_WhenHowDidYouHearAboutUsIsInvalid_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomBuyPracticeModel();
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
        public void BuyPracticeModel_WhenHowDidYouHearAboutUsDetailIsInvalid_DoesNotAddErrorToValidationResults(string testData)
        {
            var sut = CreateRandomBuyPracticeModel();
            var validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());

            sut.HowDidYouHearAboutUsDetail = testData;

            validationResults = ModelValidator.Validate(sut);
            Assert.False(validationResults.Any());
        }

        [Fact]
        public void BuyPracticeModel_WhenHowDidYouHearAboutUsDetailIsNotNull_ReturnsReferredByAsConcatenateOfHowDidYouHearAboutUsAndDetails()
        {
            var sut = CreateRandomBuyPracticeModel();
            Assert.True(sut.HowDidYouHearAboutUsDetail != null);

            Assert.Equal($"{sut.HowDidYouHearAboutUs} / {sut.HowDidYouHearAboutUsDetail}", sut.ReferredBy);
        }

        private static BuyPracticeModel CreateRandomBuyPracticeModel()
        {
            var collectionsAmountValues = new[] { "$0 - $500,000", "$500,000 - $1,000,000", "$1,500,000 - $2,000,000", "$2,000,000 +" };

            var fixture = new Fixture();

            var model = fixture.Build<BuyPracticeModel>()
                .With(m=>m.CollectionsAmount, collectionsAmountValues[RandomDataGenerator.RandomInt(0, collectionsAmountValues.Length - 1)])
                .With(m=>m.ContactPhoneNumber, RandomDataGenerator.RandomPhoneNumber)
                .With(m => m.ContactEmail, fixture.Create<MailAddress>().Address)
                .Create();

            return model;
        }
    }
}
