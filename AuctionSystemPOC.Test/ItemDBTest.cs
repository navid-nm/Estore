using NUnit.Framework;
using AuctionSystemPOC.DataAccessLayers;
using AuctionSystemPOC.Models;
using System.Collections.Generic;
using System;

namespace AuctionSystemPOC.Test
{
    [TestFixture]
    public class ItemDBTest
    {
        public ItemDB idb;
        public static List<TestCaseData> testcases = new List<TestCaseData>();
        public static List<Item> testingitems = new List<Item>
        {
            new Item {
                Name = "Test", Description = "TestDesc", Price = 0.01M, Condition = "Used",
                Username = "Testuser1"
            },
            new Item {
                Name = "Test2", Description = "TestDesc2", Price = 499.99M, Condition = "New",
                Username = "Testuser2"
            }
        };

        [SetUp]
        public void Setup()
        {
            idb = new ItemDB();
        }

        /*
         * Test 1
         * AddItem should add an item to persistent storage and return the item's ID.
         * The multiple test cases ensure that varying IDs can be retrieved from the database successfully.
         */
        [TestCaseSource("AddItemTestCases"), Order(1)]
        public int AddItemTest(Item item)
        {
            return idb.AddItem(item);
        }

        /*
         * Test 2
         * Needs to return a tuple containing the stored information for a given item.
         * (i.e. Checking if GetItemInfoFromID returns the correct data for the items added in test 1)
         */
        [TestCaseSource("GetInfoTestCases"), Order(2)]
        public Tuple<string, string, List<decimal>, string, string, bool, List<Bid>> GetInfoTest(long id)
        {
            return idb.GetItemInfoFromID(id);
        }

        /*
         * Test 3
         * In order to pass, GetAllItems should retrieve all items in the item table and return them 
         * as a list of Item objects.
         */
        [Test, Order(3)]
        public void GetAllItemsTest()
        {
            idb.AddItem(testingitems[0]);
            var items = idb.GetAllItems();
            Assert.AreEqual(items.Count, 3);
            Assert.True(items[0].Name == "Test" && items[1].Price == 499.99M);
        }

        /*
         * Test 4
         * To pass, Conclude() should set the value of "concluded" in the table to true.
         * (Indicating the listing has ended)
         */
        [Test, Order(4)]
        public void ConcludeItemTest()
        {
            idb.Conclude(1);
            Assert.True(idb.GetItemInfoFromID(1).Item6);
        }

        /*
         * Test 5
         * AddBid should add a bid to an item.
         * This will be stored in the item as an object of type List<Bid>.
         */
        [Test, Order(5)]
        public void AddBidTest()
        {
            idb.AddBid(new Bid { ID = 2, Amount = 49.99M, Username = "Testuser" });
            Assert.AreEqual(idb.GetItemInfoFromID(2).Item7.Count, 1);
            //Note: Item7 in the GetItemInfo tuple is the list of bids.
        }

        /*
         * Test 6
         * To pass, GetBids should return a list of bids for an item.
         */
        [Test, Order(6)]
        public void GetBidsTest()
        {
            Assert.AreEqual(idb.GetBids(2).Count, 1);
        }

        /*
         * Test 7
         * To pass, the method ConcludeExpiredItems should set the "concluded" field to true for all auctions that
         * have passed their expiry date.
         */
        [Test, Order(10)]
        public void ConcludeExpiredItemsTest()
        {
            var db = new Database();
            var res = new List<string>();

            using (var conn = db.GetConnection())
            {
                /*
                 * Adding an item to the items table that expired an hour ago (hence: timestamp - interval 1 hour)
                 * As ItemDB cannot add auctions that have already expired, this must be done with raw SQL in the test.
                 */
                var command = db.GetCommand(conn,
                    "INSERT INTO auctionsystempoc.items (name, description, startingprice, currentprice,"
                    + "itemcondition, views, username, datelisted, conclusiondate, concluded) "
                    + "VALUES('ExpiryTestItem', 'Testing', 1.00, 1.00, 'New', 0, 'Testuser', "
                    + "CURRENT_TIMESTAMP, CURRENT_TIMESTAMP - INTERVAL 1 HOUR, 0)");
                conn.Open();
                command.ExecuteNonQuery();
                idb.ConcludeExpiredItems();

                //The name of the item "ExpiryTestItem" should be among the results of the query below.
                command = db.GetCommand(conn, "SELECT name FROM auctionsystempoc.items WHERE concluded = 1");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(reader.GetString("name"));
                }
            }
            Assert.True(res.Contains("ExpiryTestItem"));
        }

        public static List<TestCaseData> AddItemTestCases
        {
            get
            {
                for (int i = 0; i < testingitems.Count; i++)
                {
                    //i + 1: the list index begins from 0, whereas the table IDs begin from 1.
                    testcases.Add(new TestCaseData(testingitems[i]).Returns(i + 1));
                }
                return testcases;
            }
        }

        public static List<TestCaseData> GetInfoTestCases
        {
            get
            {
                testcases.Clear();
                for (int i = 0; i < testingitems.Count; i++)
                {
                    testcases.Add(new TestCaseData((long) i + 1).Returns(Tuple.Create(
                        testingitems[i].Name, testingitems[i].Description,
                        new List<decimal> { testingitems[i].Price, testingitems[i].Price }, testingitems[i].Condition,
                        testingitems[i].Username, false, new List<Bid>()
                    )));
                }
                return testcases;
            }
        }
    }
}