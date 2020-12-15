using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BencoPracticeTransitions.Tests
{
    public class StartUpTests
    {

        [Fact]
        public void CanCreateStartUp()
        {
            var configuration = new Mock<IConfiguration>().Object;
            // ReSharper disable once UnusedVariable
            var sut = new Startup(configuration);
        }



        [Fact]
        public void ConfigureServices_WhenPassedNull_ThrowsNullReferenceException()
        {
            var configuration = new Mock<IConfiguration>().Object;
            var sut = new Startup(configuration);

            Assert.Throws<ArgumentNullException>( () => sut.ConfigureServices(null));
        }


        [Fact]
        public void ConfigureServices_WhenPassedValidServiceCollection_ExecutesWithoutException()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(m => m.GetSection(It.IsAny<string>()))
                .Returns((string sectionName) => new Mock<IConfigurationSection>().Object);

            var sut = new Startup(mockConfiguration.Object);

            var services = new ServiceCollection();
            sut.ConfigureServices(services);
        }


        [Fact]
        public void Configure_WhenPassedNullApp_ThrowsNullReferenceException()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(m => m.GetSection(It.IsAny<string>()))
                .Returns((string sectionName) => new Mock<IConfigurationSection>().Object);

            var sut = new Startup(mockConfiguration.Object);

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Configure(null, new HostingEnvironment()));
            Assert.Equal("app", exception.ParamName);
        }


        [Fact]
        public void Configure_WhenPassedNullEnv_ThrowsNullReferenceException()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(m => m.GetSection(It.IsAny<string>()))
                .Returns((string sectionName) => new Mock<IConfigurationSection>().Object);

            var applicationBuilder = new Mock<IApplicationBuilder>().Object;

            var sut = new Startup(mockConfiguration.Object);


            var exception = Assert.Throws<ArgumentNullException>(() => sut.Configure(applicationBuilder, null));
            Assert.Equal("hostingEnvironment", exception.ParamName);
        }

    }
}
