using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace AuctionSystemPOC.Hubs
{
    public class BidNotification : Hub
    {
        private readonly IList<string> biddergroups;

        public BidNotification()
        {
            biddergroups = new List<string>();
        }

        public async Task NotifyOutbid(string itemid)
        {
            Debug.WriteLine(itemid);
            await Clients.OthersInGroup(itemid).SendAsync("ReceiveOutbidNotification");
        }

        private void CreateBiddersGroup(string groupname)
        {
            biddergroups.Add(groupname);
        }

        private bool GroupExists(string groupname)
        {
            return biddergroups.Contains(groupname);
        }

        /// <summary>
        /// Adds a bidder to the relevant group.
        /// </summary>
        /// <param name="itemid">The ID of the item as a string</param>
        public async Task JoinBiddersGroup(string itemid)
        {
            if (!GroupExists(itemid))
            {
                    CreateBiddersGroup(itemid);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, itemid);
        }
    }
}
