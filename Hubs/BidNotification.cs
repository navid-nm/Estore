using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using AuctionSystemPOC.DataAccessLayers;

namespace AuctionSystemPOC.Hubs
{
    public class BidNotification : Hub
    {
        private ItemDB idb;

        public BidNotification()
        {
            idb = new ItemDB();
        }

        public async Task NotifyOutbid(string itemid)
        {
            List<string> bidders = idb.GetBidders(Int64.Parse(itemid));
            string bidders_str = "", sep = ",";
            for (int bidind = 0; bidind < bidders.Count; bidind++)
            {
                if (bidind == bidders.Count - 1)
                {
                    sep = "";
                }
                bidders_str += bidders[bidind] + sep;
            }
            await Clients.Others.SendAsync("ReceiveOutbidNotification", bidders_str);
        }
    }
}