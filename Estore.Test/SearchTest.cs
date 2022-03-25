using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Estore.Test.Helpers;

namespace Estore.Test
{
    public class SearchTest
    {
        private TestServer server;

        [SetUp]
        public void Setup()
        {
            server = new TestServer(new WebHostHelper().GetWebHostBuilder());
        }

        //Test to ensure that the search feature functions correctly.
        [Test]
        public async Task TestPositiveSearchResult()
        {
            var response = await server.CreateRequest("/search/TestName").SendAsync("GET");
            string content = await response.Content.ReadAsStringAsync();
            Assert.That(content, Does.Contain("TestName"));
        }

        //Test to ensure that the "no such items" error is displayed in the view when applicable.
        [Test]
        public async Task TestNegativeSearchResult()
        {
            var response = await server.CreateRequest("/search/NonExistentItem").SendAsync("GET");
            string content = await response.Content.ReadAsStringAsync();
            Assert.That(content, Does.Contain("There were no results"));
        }
    }
}