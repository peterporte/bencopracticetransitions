using Microsoft.AspNetCore.Http;
using Moq;
using reCAPTCHA.AspNetCore;

namespace BencoPracticeTransitions.Tests.Helpers
{
    internal class MockReCaptchaService
    {
        public static IRecaptchaService Create(bool success)
        {
            var reCaptchaResponse = new RecaptchaResponse { success = success };

            var mockReCaptcha = new Mock<IRecaptchaService>();
            mockReCaptcha.Setup(m => m.Validate(It.IsAny<HttpRequest>(), It.IsAny<bool>())).ReturnsAsync(reCaptchaResponse);
            var reCaptcha = mockReCaptcha.Object;
            return reCaptcha;
        }
    }
}
