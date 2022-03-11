using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.TestHost;
using Estore.Test.Helpers;

namespace Estore.Test
{
    public class AuthorizationTest
    {
        private TestServer server;

        [SetUp]
        public void Setup()
        {
            server = new TestServer(new WebHostHelper().GetWebHostBuilder());
        }

        [Test]
        public async Task TestAuth()
        {
            var protectedPaths = new List<string> {
                "/location/TestFindCode",
                "/dashboard",
                "/purchased",
                "/sell",
                "/buy"
            };
            foreach (var path in protectedPaths)
            {
                var response = await server.CreateRequest(path).SendAsync("GET");
                Assert.True(response.StatusCode.Equals(HttpStatusCode.Redirect));
            }
        }
    }
}