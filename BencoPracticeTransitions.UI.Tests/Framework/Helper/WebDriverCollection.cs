using Benco.Framework.UI.Tests.Core.xUnit;
using Xunit;

namespace BencoPracticeTransitions.UI.Tests.Framework.Helper
{
    [CollectionDefinition("WebDriverCollection")]
    public class WebDriverCollection : ICollectionFixture<WebDriverFixture>
    {
        //this is just a marker class base on xunit documentation
        //https://xunit.github.io/docs/shared-context.html
    }
}