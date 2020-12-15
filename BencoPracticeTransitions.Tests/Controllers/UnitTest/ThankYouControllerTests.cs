using BencoPracticeTransitions.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BencoPracticeTransitions.Tests.Controllers.UnitTest
{
    public class ThankYouControllerTests
    {


        [Fact]
        public void CanCreateThankYouController()
        {
            // ReSharper disable once UnusedVariable
            var sut = new ThankYouController();
        }



        [Fact]
        public void Index_WhenDoingHttpGet_ReturnsView()
        {
            var sut = new ThankYouController();

            var result = sut.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);

        }
    }
}
