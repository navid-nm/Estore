using NUnit.Framework;
using Estore.Test.Helpers;

namespace Estore.Test
{
    [SetUpFixture]
    public class Setup
    {
        private TestDataProvider data;

        [OneTimeSetUp]
        public void SetupAll()
        {
            data = new TestDataProvider();
            data.CreateZeroUser();
            data.CreateZeroItem();
        }

        [OneTimeTearDown]
        public void TearDownAll()
        {
            data.Down();
        }
    }
}
