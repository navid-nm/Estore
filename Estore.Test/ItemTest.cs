using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Estore.Test.Helpers;

namespace Estore.Test
{
    public class ItemTest
    {
        private TestServer server;

        [SetUp]
        public void Setup()
        {
            server = new TestServer(new WebHostHelper().GetWebHostBuilder());
        }

        [Test]
        public async Task TestItemRetrieval()
        {
            var response = await server.CreateRequest("/item/TestFindCode").SendAsync("GET");
            string content = await response.Content.ReadAsStringAsync();
            Assert.That(content, Does.Contain("TestName"));     //Item listing name/title
            Assert.That(content, Does.Contain("TestUser"));     //Seller
            Assert.That(content, Does.Contain("10.00"));        //Price
        }

        [Test]
        public async Task TestNonExistentItem()
        {
            var response = await server.CreateRequest("/item/NonExistentItem").SendAsync("GET");
            string content = await response.Content.ReadAsStringAsync();
            Assert.That(content, Does.Contain("404"));
        }
    }
}