using NUnit.Framework;
using AuctionSystemPOC.DataAccessLayers;
using System.IO;
using System;

namespace AuctionSystemPOC.Test
{
    [SetUpFixture]
    public class DBTestHelper
    {
        readonly Database db;

        public DBTestHelper()
        {
            db = new Database();
        }
        
        [OneTimeSetUp]
        public void RunBeforeAll()
        {
            string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            if (!Directory.Exists("DataAccessLayers")) Directory.CreateDirectory("DataAccessLayers");
            File.Copy(
                root + @"../AuctionSystemPOC/DataAccessLayers/ItemsAndBids.sql",
                root + @"bin/Debug/netcoreapp3.1/DataAccessLayers/ItemsAndBids.sql",
                true
            );
            DropAll();
        }
        
        /// <summary>
        /// For dropping all tables prior to testing.
        /// </summary>
        public void DropAll()
        {
            //Variable "comtext" should be updated on the addition of new tables
            string comtext = "DROP TABLE IF EXISTS items, itemhasbids, bids";
            using (var conn = db.GetConnection())
            {
                var command = db.GetCommand(conn, comtext);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}