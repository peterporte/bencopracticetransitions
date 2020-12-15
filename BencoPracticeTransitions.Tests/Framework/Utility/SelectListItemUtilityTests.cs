using System.Linq;
using BencoPracticeTransitions.Framework.Enums;
using BencoPracticeTransitions.Framework.Utility;
using Xunit;

namespace BencoPracticeTransitions.Tests.Framework.Utility
{
    public class SelectListItemUtilityTests
    {


        [Fact]
        public void ConvertEnumToSelectList_WhenPassedAnEnumWithDescription_ReturnsDescriptionInTextProperty()
        {
            var result = SelectListItemUtility.ConvertEnumToSelectList(typeof(RealEstateOptionForBuyEnum), null, false,
                false);

            var expectedResult = new[] {"Rent/Lease", "Buy", "Either"};
            Assert.Equal(expectedResult, result.Select(s => s.Text));
        }

        [Theory]
        [InlineData("RentingLeasing")]
        [InlineData("Buy")]
        [InlineData("Either")]
        public void ConvertEnumToSelectList_WhenSelectedIsSet_ReturnsListWithItemSelected(string selectedItem)
        {
            var result = SelectListItemUtility.ConvertEnumToSelectList(typeof(RealEstateOptionForBuyEnum), selectedItem, false,
                false);

            Assert.Equal(selectedItem, result.Single( s => s.Selected).Value);
        }



        [Fact]
        public void ConvertEnumToSelectList_WhenSelectedIsNull_ReturnsListWithNoItemSelected()
        {
            var result = SelectListItemUtility.ConvertEnumToSelectList(typeof(RealEstateOptionForBuyEnum), null, false,
                false);

            Assert.DoesNotContain(result, s => s.Selected);
        }



        [Fact]
        public void ConvertEnumToSelectList_WhenUseIntValisTrue_ReturnsListWithNumericValues()
        {
            var result = SelectListItemUtility.ConvertEnumToSelectList(typeof(RealEstateOptionForBuyEnum), null, true,
                false);

            Assert.Equal(new [] {"0", "1", "2"}, result.Select(s => s.Value));
        }


        [Fact]
        public void ConvertEnumToSelectList_WhenIncludeBlankIsTrue_ReturnsListWithFirstItemBlank()
        {
            var result = SelectListItemUtility.ConvertEnumToSelectList(typeof(RealEstateOptionForBuyEnum), null, false,
                true);

            var expectedResult = new[] { "", "Rent/Lease", "Buy", "Either" };
            Assert.Equal(expectedResult, result.Select(s => s.Text));
        }
    }
}
