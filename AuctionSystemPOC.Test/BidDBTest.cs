using NUnit.Framework;
using AuctionSystemPOC.DataAccessLayers;
using AuctionSystemPOC.Models;
using System.Collections.Generic;

namespace AuctionSystemPOC.Test
{
    [TestFixture]
    public class BidDBTest
    {
        public BidDB bdb;
        public ItemDB idb = new ItemDB();
        public static List<Bid> testingbids = new List<Bid>();
        public static List<TestCaseData> testcases = new List<TestCaseData>();

        [SetUp]
        public void Setup()
        {
            bdb = new BidDB();
        }

        /* 
         * Test 1
         * To pass, the GetBidByID should return a bid object given its ID in the table.
         */
        [TestCaseSource("GetBidByIDTestCases"), Order(8)]
        public Bid GetBidByIDTest(long id)
        {
            return bdb.GetBidByID(id);
        }

        /* 
         * Test 2
         * Should return the ID of the last bid that was placed.
         * This method will be run after an bid is added via the item model.
         */
        [Test, Order(9)]
        public void GetLastBidID()
        {
            Assert.AreEqual(bdb.GetLastBidID(), 0);
        }

        public static List<TestCaseData> GetBidByIDTestCases
        {
            get
            {
                for (int i = 0; i < testingbids.Count; i++)
                {
                    testcases.Add(new TestCaseData((long)i + 1).Returns(testingbids[i]));
                }
                return testcases;
            }
        }
    }
}