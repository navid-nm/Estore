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
        public async Task NotifyOutbid(string itemid)
        {
            Debug.WriteLine(itemid);
            await Clients.Others.SendAsync("ReceiveOutbidNotification");
        }
    }
}