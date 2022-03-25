using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Estore.Test.Helpers;

namespace Estore.Test
{
    public class UserTest
    {
        private TestServer server;

        [SetUp]
        public void Setup()
        {
            server = new TestServer(new WebHostHelper().GetWebHostBuilder());
        }

        //For ensuring the correct functioning of user data retrieval.
        [Test]
        public async Task TestUserProfileRetrieval()
        {
            var response = await server.CreateRequest("/user/TestUser").SendAsync("GET");
            string content = await response.Content.ReadAsStringAsync();
            List<int> dateData = new List<int> 
            {
                DateTime.Now.Day,
                DateTime.Now.Month,
                DateTime.Now.Year
            };
            Assert.That(content, Does.Contain("TestUser"));         //Check whether username is included
            Assert.That(content, Does.Contain("TestName"));         //Ensure item being sold by the user is included
            foreach (int num in dateData)
            {
                Assert.That(content, Does.Contain(num.ToString())); //Ensure sign-up date is included
            }
        }
    }
}