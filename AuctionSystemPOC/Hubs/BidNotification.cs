﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using AuctionSystemPOC.DataAccessLayers;

namespace AuctionSystemPOC.Hubs
{
    /// <summary>
    /// For sending and receiving notifications regarding bids.
    /// </summary>
    public class BidNotification : Hub
    {
        private readonly ItemDB idb;

        public BidNotification()
        {
            idb = new ItemDB();
        }

        public async Task NotifyOutbid(string itemid)
        {
            long idaslong = Int64.Parse(itemid);
            List<string> bidders = idb.GetBidders(idaslong);
            string bidders_str = "", sep = ",", itemname = idb.GetItemFromID(idaslong).Name;
            for (int i = 0; i < bidders.Count; i++)
            {
                if (i == bidders.Count - 1)
                    sep = "";
                bidders_str += bidders[i] + sep;
            }
            await Clients.Others.SendAsync("ReceiveOutbidNotification", bidders_str, itemid, itemname);
        }
    }
}