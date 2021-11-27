using System.Collections.Generic;
using MySqlConnector;

namespace AuctionSystemPOC.DataAccessLayers
{
    /// <summary>
    /// Provides ease-of-use methods for database operations and stores the connection string.
    /// </summary>
    public class Database
    {
        private readonly string conn;

        public Database()
        {
            this.conn = "Server=localhost;"
                + "User ID=admin;"
                + "Password=password;"
                + "Database=auctionsystempoc";
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(conn);
        }

        public MySqlCommand GetCommand(MySqlConnection aconn, string comtext)
        {
            return new MySqlCommand { Connection = aconn, CommandText = comtext };
        }

        public MySqlCommand GetCommandWithArgs(MySqlConnection aconn, string comtext, Dictionary<string, string> args)
        {
            MySqlCommand cmd = GetCommand(aconn, comtext);
            foreach (string arg in args.Keys)
            {
                cmd.Parameters.AddWithValue(arg, args[arg]);
            }
            return cmd;
        }

        public void RunUpdateFromText(string comtext)
        {
            var aconn = GetConnection();
            var com = GetCommand(aconn, comtext);
            RunComWithConn(com, aconn);
        }

        public void RunComWithConn(MySqlCommand com, MySqlConnection aconn)
        {
            aconn.Open();
            com.ExecuteNonQuery();
            aconn.Close();
        }
    }
}