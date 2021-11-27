using MySqlConnector;
using AuctionSystemPOC.Models;

namespace AuctionSystemPOC.DataAccessLayers
{
    /// <summary>
    /// Abstracts database access for bids.
    /// </summary>
    public class BidDB
    {
        private readonly Database db;

        public BidDB()
        {
            db = new Database();
        }

        /// <summary>
        /// Retrieve a bid object by its ID in the bids table.
        /// </summary>
        /// <param name="id">ID of the bid</param>
        /// <returns>Bid object corresponding to given ID</returns>
        public Bid GetBidByID(long id)
        {
            Bid bid = null;
            using (var msc = db.GetConnection())
            {
                string qtext = "SELECT * FROM auctionsystempoc.bids WHERE id = @id";
                var qcom = db.GetCommand(msc, qtext);
                qcom.Parameters.AddWithValue("@id", id);
                msc.Open();
                var reader = qcom.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    bid = new Bid
                    {
                        ID = id,
                        Amount = reader.GetDecimal("amount"),
                        DateMade = reader.GetDateTime("datemade"),
                        Username = reader.GetString("username")
                    };
                }
            }
            return bid;
        }

        /// <summary>
        /// Get the ID of the last added bid.
        /// </summary>
        /// <returns>ID of last bid</returns>
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