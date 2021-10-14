using System;
using System.Collections.Generic;
using EcomProofOfConcept.Models;
using MySqlConnector;

namespace EcomProofOfConcept.DataAccessLayers
{
    public class UserDB
    {
        private readonly Database db;

        public UserDB()
        {
            db = new Database();
            db.RunUpdateFromText(
                "CREATE TABLE IF NOT EXISTS Users ("
                    + "id INTEGER AUTO_INCREMENT PRIMARY KEY,"
                    + "username TEXT NOT NULL,"
                    + "email TEXT NOT NULL,"
                    + "pass TEXT NOT NULL,"
                    + "sellrating INTEGER NOT NULL,"
                    + "buyrating INTEGER NOT NULL,"
                    + "dateofreg DATETIME NOT NULL"
                + ");"
            );
        }

        public void AddUser(User user)
        {
            var msc = db.GetConnection();
            string ctext = "INSERT INTO ecompoc.users" 
                + " (username, email, pass, sellrating, buyrating, dateofreg)" 
                + " VALUES (@nm, @mail, @ps, 0, 0, CURRENT_TIMESTAMP)";
            MySqlCommand rcom = db.GetCommandWithArgs(msc, ctext, new Dictionary<string, string>()
            {
                { "nm", user.Username },
                { "mail", user.Email },
                { "ps", user.Password }
            });
            db.RunComWithConn(rcom, msc);
        }

        public string GetUsernameByEmail(string email)
        {
            string qtext = "SELECT * FROM ecompoc.users WHERE email = @mail", res = "";
            using (var msc = db.GetConnection())
            {
                MySqlCommand rcom = db.GetCommand(msc, qtext);
                rcom.Parameters.AddWithValue("@mail", email);
                msc.Open();
                MySqlDataReader reader = rcom.ExecuteReader();
                if (reader.Read())
                    res = reader.GetString(reader.GetOrdinal("username"));
                reader.Close();
            }
            return res;
        }

        public bool VerifyEmailAndPass(string email, string pass)
        {
            string qtext = "SELECT * FROM ecompoc.users WHERE email = @mail AND pass = @ps";
            bool verif = false;
            using (var msc = db.GetConnection())
            {
                MySqlCommand rcom = db.GetCommandWithArgs(msc, qtext, new Dictionary<string, string>()
                {
                    { "mail", email },
                    { "ps", pass }
                });
                msc.Open();
                MySqlDataReader reader = rcom.ExecuteReader();
                verif = reader.HasRows;
                reader.Close();
            }
            return verif;
        }

        /// <summary>
        /// Returns user info for a given username
        /// </summary>
        /// <param name="name">Given username</param>
        /// <returns>ID, Email, Buyer Rating, Seller Rating, Date of Registration</returns>
        public Tuple<int, string, int, int, string> GetUserInfoFromName(string name)
        {
            string qtext = "SELECT id, email, sellrating, buyrating, dateofreg FROM ecompoc.users WHERE username = @name";
            using (var msc = db.GetConnection())
            {
                MySqlCommand rcom = db.GetCommand(msc, qtext);
                rcom.Parameters.AddWithValue("@name", name);
                msc.Open();
                MySqlDataReader reader = rcom.ExecuteReader();
                if (!reader.HasRows) return null;
                reader.Read();
                var info = Tuple.Create(
                    (int) reader.GetInt16("id"), 
                    reader.GetString("email"),
                    (int) reader.GetInt16("buyrating"),
                    (int) reader.GetInt16("sellrating"),
                    reader.GetDateTime("dateofreg").ToString()
                );
                reader.Close();
                return info;
            }
        }

        public int GetIDFromName(string name)
        {
            string qtext = "SELECT id FROM ecompoc.users WHERE username = @name";
            using (var msc = db.GetConnection())
            {
                MySqlCommand rcom = db.GetCommand(msc, qtext);
                rcom.Parameters.AddWithValue("@name", name);
                msc.Open();
                var reader = rcom.ExecuteReader();
                reader.Read();
                return reader.GetInt16("id");
            }
        }

        /// <summary>
        /// Get listings from the user in the format Name : ID
        /// </summary>
        /// <param name="name">The username of the user</param>
        /// <returns>A dictionary mapping the title to ID</returns>
        public Dictionary<string, long> GetListings(string name)
        {
            var nameidmap = new Dictionary<string, long>();
            string qtext = "SELECT id, name FROM ecompoc.items WHERE username = @nm AND concluded = false";
            using (var msc = db.GetConnection())
            {
                msc.Open();
                var rcom = db.GetCommand(msc, qtext);
                rcom.Parameters.AddWithValue("@nm", name);
                var reader = rcom.ExecuteReader();
                while (reader.Read())
                {
                    nameidmap[reader.GetString("name")] = reader.GetInt64("id");
                }
                reader.Close();
            }
            return nameidmap;
        }
    }
}
