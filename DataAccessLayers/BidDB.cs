using MySqlConnector;
using AuctionSystemPOC.Models;

namespace AuctionSystemPOC.DataAccessLayers
{
    public class BidDB
    {
        private readonly Database db;

        public BidDB()
        {
            db = new Database();
        }

        public Bid GetBidByID(long id)
        {
            Bid bid = null;
            using (var msc = db.GetConnection())
            {
                string qtext = "SELECT (amount, datemade, username) FROM actionsystempoc.bids WHERE id = @id";
                var qcom = db.GetCommand(msc, qtext);
                qcom.Parameters.AddWithValue("@id", id);
                msc.Open();
                var reader = qcom.ExecuteReader();
                if (reader.HasRows)
                {
                    bid = new Bid
                    {
                        ID = id,
                        Amount = reader.GetInt16("amount"),
                        DateMade = reader.GetDateTime("datemade"),
                        Username = reader.GetString("username")
                    };
                }
            }
            return bid;
        }

        public long GetLastBidID()
        {
            string ctext = "SELECT id FROM auctionsystempoc.bids WHERE id = (SELECT MAX(id) FROM auctionsystempoc.bids)";
            long last = 0;
            using (var msc = db.GetConnection())
            {
                MySqlCommand com = db.GetCommand(msc, ctext);
                msc.Open();
                MySqlDataReader reader = com.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    last = reader.GetInt64("id");
                }
            }
            return last;
        }
    }
}