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
                    + "price DECIMAL NOT NULL,"
                    + "itemcondition TEXT NOT NULL,"
                    + "username TEXT NOT NULL,"
                    + "views INTEGER NOT NULL,"
                    + "datelisted DATETIME NOT NULL,"
                    + "conclusiondate DATETIME NOT NULL,"
                    + "concluded BOOLEAN NOT NULL,"
                    + "FOREIGN KEY (userid) REFERENCES Users(id)"
                + ");"
            );
        }

        public void AddItem(Item item, string username)
        {
            var msc = db.GetConnection();
            string ctext = "INSERT INTO ecompoc.items"
                + " (name, description, price, itemcondition, username, userid, views, datelisted, concluded)"
                + " VALUES (@nm, @desc, @price, @cond, @uname, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP "
                + "+ INTERVAL 1 HOUR, false)";
            MySqlCommand rcom = db.GetCommandWithArgs(msc, ctext, new Dictionary<string, string>()
            {
                { "nm", item.Name },
                { "desc", item.Description },
                { "price", Math.Round(item.Price, 2).ToString() },
                { "cond", item.Condition },
                { "uname", username },
            });
            db.RunComWithConn(rcom, msc);
        }

        public Tuple<string, string, float, string, string, bool> GetItemInfoFromID(long id)
        {
            string qtext = "SELECT name, description, price, itemcondition, username, concluded"
                + " FROM ecompoc.items"
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
                    reader.GetString("name"), reader.GetString("description"), (float)reader.GetFloat("price"),
                    reader.GetString("itemcondition"), reader.GetString("username"), reader.GetBoolean("concluded")
                );
                reader.Close();
                return info;
            }
        }

        public void IncrementViews(long id)
        {
            string ctext = "UPDATE ecompoc.items SET views = views + 1 WHERE id = @id";
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
            string ctext = "UPDATE ecompoc.items SET concluded = true WHERE id = @id";
            var msc = db.GetConnection();
            var rcom = db.GetCommand(msc, ctext);
            rcom.Parameters.AddWithValue("@id", id);
            db.RunComWithConn(rcom, msc);
        }
    }
}