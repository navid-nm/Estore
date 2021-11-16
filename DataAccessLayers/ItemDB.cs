using System;
using System.IO;
using System.Collections.Generic;
using MySqlConnector;
using AuctionSystemPOC.Models;

namespace AuctionSystemPOC.DataAccessLayers
{
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

        public int AddItem(Item item)
        {
            var msc = db.GetConnection();
            string ctext = "INSERT INTO auctionsystempoc.items"
                + " (name, description, startingprice, currentprice, itemcondition, username, views, datelisted, "
                + "conclusiondate, concluded)"
                + " VALUES (@nm, @desc, @price, @price, @cond, @uname, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP "
                + "+ INTERVAL 1 HOUR, false); SELECT LAST_INSERT_ID();";
            MySqlCommand rcom = db.GetCommandWithArgs(msc, ctext, new Dictionary<string, string>()
            {
                { "nm", item.Name },
                { "desc", item.Description },
                { "price", Math.Round(item.Price, 2).ToString("0.00") },
                { "cond", item.Condition },
                { "uname", item.Username },
            });
            msc.Open();
            MySqlDataReader reader = rcom.ExecuteReader();
            reader.Read();
            int id = reader.GetInt32("LAST_INSERT_ID()");
            msc.Close();
            return id;
        }

        public Tuple<string, string, decimal, string, string, bool> GetItemInfoFromID(long id)
        {
            string qtext = "SELECT name, description, currentprice, itemcondition, username, concluded"
                + " FROM auctionsystempoc.items"
                + " WHERE id = @id";
            using (var msc = db.GetConnection())
            {
                MySqlCommand rcom = db.GetCommand(msc, qtext);
                rcom.Parameters.AddWithValue("@id", id);
                msc.Open();
                MySqlDataReader reader = rcom.ExecuteReader();
                if (!reader.HasRows) return null;
                reader.Read();
                var info = Tuple.Create(
                    reader.GetString("name"), reader.GetString("description"), reader.GetDecimal("currentprice"),
                    reader.GetString("itemcondition"), reader.GetString("username"), reader.GetBoolean("concluded")
                );
                reader.Close();
                return info;
            }
        }

        public List<Item> GetAllItems()
        {
            string qtext = "SELECT id, name, description, currentprice, itemcondition, username, concluded"
                + " FROM auctionsystempoc.items";
            var items = new List<Item>();
            using (var msc = db.GetConnection())
            {
                var rcom = db.GetCommand(msc, qtext);
                msc.Open();
                var reader = rcom.ExecuteReader();
                if (!reader.HasRows) return null;
                else
                {
                    while (reader.Read())
                    {
                        Item itm = new Item
                        {
                            ID = reader.GetInt64("id"),
                            Name = reader.GetString("name"),
                            Description = reader.GetString("description"),
                            Price = reader.GetDecimal("currentprice"),
                            Condition = reader.GetString("itemcondition"),
                            Username = reader.GetString("username"),
                            Concluded = reader.GetBoolean("concluded")
                        };
                        items.Add(itm);
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Retrieve bids for an item
        /// </summary>
        /// <returns>A list of bids</returns>
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
                var reader = qcom.ExecuteReader();
                while (reader.HasRows)
                {
                    bids.Add(bdb.GetBidByID(reader.GetInt64("bidid")));
                }
            }
            return bids;
        }

        /// <summary>
        /// Add a bid to an item
        /// </summary>
        /// <param name="bid">The bid to add</param>
        public void AddBid(Bid bid)
        {
            string ctext = "UPDATE auctionsystempoc.items SET currentprice = @price WHERE id = @id"
                + "; INSERT INTO auctionsystempoc.bids (amount, datemade, username) VALUES (@amount,"
                + " CURRENT_TIMESTAMP, @uname);";
            using (var msc = db.GetConnection())
            {
                MySqlCommand com = db.GetCommandWithArgs(msc, ctext, new Dictionary<string, string>
                {
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

        public void Conclude(long id)
        {
            string ctext = "UPDATE auctionsystempoc.items SET concluded = true WHERE id = @id";
            var msc = db.GetConnection();
            var rcom = db.GetCommand(msc, ctext);
            rcom.Parameters.AddWithValue("@id", id);
            db.RunComWithConn(rcom, msc);
        }
    }
}