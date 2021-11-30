using System;
using System.IO;
using System.Collections.Generic;
using MySqlConnector;
using AuctionSystemPOC.Models;

namespace AuctionSystemPOC.DataAccessLayers
{
    /// <summary>
    /// Abstracts database access for items.
    /// </summary>
    public class ItemDB
    {
        private readonly Database db;
        private readonly BidDB bdb;

        public ItemDB()
        {
            db = new Database();
            bdb = new BidDB();
            string sqlc;
            using (StreamReader srdr = File.OpenText("DataAccessLayers/ItemsAndBids.sql"))
            {
                sqlc = srdr.ReadToEnd();
            }
            db.RunUpdateFromText(sqlc);
        }

        /// <summary>
        /// Add an item to persistent storage.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <returns>ID of the item that was added</returns>
        public int AddItem(Item item)
        {
            using var conn = db.GetConnection();
            string ctext = "INSERT INTO auctionsystempoc.items "
                + "(name, description, startingprice, currentprice, itemcondition, username, views, datelisted, "
                + "conclusiondate, concluded) "
                + "VALUES (@nm, @desc, @price, @price, @cond, @uname, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP + "
                + "INTERVAL 2 HOUR, false); SELECT LAST_INSERT_ID() AS last;";
            MySqlCommand rcom = db.GetCommandWithArgs(conn, ctext, new Dictionary<string, string>()
            {
                { "nm", item.Name },
                { "desc", item.Description },
                { "price", Math.Round(item.Price, 2).ToString("0.00") },
                { "cond", item.Condition },
                { "uname", item.Username },
            });
            conn.Open();
            using var reader = rcom.ExecuteReader();
            reader.Read();
            return reader.GetInt32("last");
        }

        /// <summary>
        /// Get item object from ID in the items table.
        /// </summary>
        /// <param name="id">The ID of the item</param>
        /// <param name="incbids">Whether the bid query should be executed</param>
        /// <returns>Item corresponding to the given ID</returns>
        public Item GetItemFromID(long id)
        {
            string qtext = "SELECT name, description, startingprice, currentprice, itemcondition, username, concluded"
                + " FROM auctionsystempoc.items"
                + " WHERE id = @id";
            Item item = null;
            using (var msc = db.GetConnection())
            {
                MySqlCommand rcom = db.GetCommand(msc, qtext);
                rcom.Parameters.AddWithValue("@id", id);
                msc.Open();
                using var reader = rcom.ExecuteReader();
                if (!reader.HasRows) return null;
                reader.Read();
                item = new Item
                {
                    ID = id,
                    Name = reader.GetString("name"),
                    Description = reader.GetString("description"),
                    StartingPrice = reader.GetDecimal("startingprice"),
                    Price = reader.GetDecimal("currentprice"),
                    Condition = reader.GetString("itemcondition"),
                    Username = reader.GetString("username"),
                    Concluded = reader.GetBoolean("concluded"),
                    Bids = GetBids(id)
                };
            }
            return item;
        }

        /// <summary>
        /// Retrieve a list of all items stored in the items table.
        /// </summary>
        /// <returns>List of Item objects</returns>
        public List<Item> GetAllItems()
        {
            string qtext = "SELECT MAX(id) AS id FROM auctionsystempoc.items";
            long maxid = 1;
            var items = new List<Item>();
            using (var msc = db.GetConnection())
            {
                var rcom = db.GetCommand(msc, qtext);
                msc.Open();
                using var reader = rcom.ExecuteReader();
                if (!reader.HasRows) return null;
                reader.Read();
                maxid = reader.GetInt64("id");
            }
            for (long i = 1; i <= maxid; i++) items.Add(GetItemFromID(i));
            return items;
        }

        /// <summary>
        /// Retrieve bids for an item.
        /// </summary>
        /// <returns>List of bids corresponding to given item ID</returns>
        /// <param name="id">ID of the item</param>
        public List<Bid> GetBids(long id)
        {
            List<Bid> bids = new List<Bid>();
            string qtext = "SELECT bidid FROM auctionsystempoc.itemhasbids WHERE itemid = @id";
            using (var msc = db.GetConnection())
            {
                var qcom = db.GetCommand(msc, qtext);
                qcom.Parameters.AddWithValue("@id", id);
                msc.Open();
                using var reader = qcom.ExecuteReader();
                while (reader.Read())
                {
                    bids.Add(bdb.GetBidByID(reader.GetInt64("bidid")));
                }
            }
            return bids;
        }

        /// <summary>
        /// Retrieve usernames of bidders.
        /// </summary>
        /// <param name="id">ID of the item</param>
        /// <returns>List of usernames that belong to bidders on the item</returns>
        public List<string> GetBidders(long id)
        {
            var outlist = new List<string>();
            var bids = GetBids(id);
            foreach (Bid bid in bids)
            {
                outlist.Add(bid.Username);
            }
            return outlist;
        }

        /// <summary>
        /// Add a bid to an item.
        /// </summary>
        /// <param name="bid">The bid to add</param>
        public void AddBid(Bid bid)
        {
            string ctext = "UPDATE auctionsystempoc.items SET currentprice = @amount WHERE id = @id"
                + "; INSERT INTO auctionsystempoc.bids (amount, datemade, username) VALUES (@amount,"
                + " CURRENT_TIMESTAMP, @uname);";
            using (var msc = db.GetConnection())
            {
                MySqlCommand com = db.GetCommandWithArgs(msc, ctext, new Dictionary<string, string>
                {
                    { "id", bid.ID.ToString() },
                    { "amount", bid.Amount.ToString() },
                    { "uname", bid.Username }
                });
                msc.Open();
                com.ExecuteNonQuery();
                string hasinsert = "INSERT INTO auctionsystempoc.itemhasbids (itemid, bidid) VALUES (@id, @bidid)";
                var bidcom = db.GetCommandWithArgs(msc, hasinsert, new Dictionary<string, string>
                {
                    { "bidid", bdb.GetLastBidID().ToString() },
                    { "id", bid.ID.ToString() }
                });
                bidcom.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// End all auctions that have passed their time limit.
        /// </summary>
        public void ConcludeExpiredItems()
        {
            string ctext = "UPDATE auctionsystempoc.items SET concluded = 1 WHERE conclusiondate < CURRENT_TIMESTAMP";
            var conn = db.GetConnection();
            var rcom = db.GetCommand(conn, ctext);
            db.RunComWithConn(rcom, conn);
        }

        /// <summary>
        /// Increment the number of views for an item in persistent storage.
        /// </summary>
        /// <param name="id">ID of the item</param>
        public void IncrementViews(long id)
        {
            string ctext = "UPDATE auctionsystempoc.items SET views = views + 1 WHERE id = @id";
            using (var msc = db.GetConnection())
            {
                var rcom = db.GetCommand(msc, ctext);
                rcom.Parameters.AddWithValue("@id", id);
                msc.Open();
                rcom.ExecuteNonQuery();
            }
        }
    }
}