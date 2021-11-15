using System;
using System.Collections.Generic;
using MySqlConnector;
using AuctionSystemPOC.Models;

namespace AuctionSystemPOC.DataAccessLayers
{
    public class ItemDB
    {
        private readonly Database db;

        public ItemDB()
        {
            db = new Database();
            db.RunUpdateFromText(
                "CREATE TABLE IF NOT EXISTS Items ("
                    + "id INTEGER AUTO_INCREMENT PRIMARY KEY,"
                    + "name TEXT NOT NULL,"
                    + "description TEXT NOT NULL,"
                    + "price DECIMAL(9, 2) NOT NULL,"
                    + "itemcondition TEXT NOT NULL,"
                    + "username TEXT NOT NULL,"
                    + "views INTEGER NOT NULL,"
                    + "datelisted DATETIME NOT NULL,"
                    + "conclusiondate DATETIME NOT NULL,"
                    + "concluded BOOLEAN NOT NULL"
                + ");"
            );
        }

        public int AddItem(Item item)
        {
            var msc = db.GetConnection();
            string ctext = "INSERT INTO auctionsystempoc.items"
                + " (name, description, price, itemcondition, username, views, datelisted, conclusiondate, concluded)"
                + " VALUES (@nm, @desc, @price, @cond, @uname, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP "
                + "+ INTERVAL 1 HOUR, false); SELECT LAST_INSERT_ID();";
            MySqlCommand rcom = db.GetCommandWithArgs(msc, ctext, new Dictionary<string, string>()
            {
                { "nm", item.Name },
                { "desc", item.Description },
                { "price", Math.Round(item.Price, 2).ToString() },
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
            string qtext = "SELECT name, description, price, itemcondition, username, concluded"
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
                    reader.GetString("name"), reader.GetString("description"), reader.GetDecimal("price"),
                    reader.GetString("itemcondition"), reader.GetString("username"), reader.GetBoolean("concluded")
                );
                reader.Close();
                return info;
            }
        }

        public List<Item> GetAllItems()
        {
            string qtext = "SELECT id, name, description, price, itemcondition, username, concluded"
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
                            Price = reader.GetDecimal("price"),
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