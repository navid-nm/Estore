﻿using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using AuctionSystemPOC.DataAccessLayers;

namespace AuctionSystemPOC.BackgroundServices
{
    /// <summary>
    /// Checks for and concludes auctions that have passed their end date.
    /// </summary>
    public class AuctionExpiryMonitor : IHostedService
    {
        private Timer timer;

        public Task StartAsync(CancellationToken ct)
        {
            timer = new Timer(op => { new ItemDB().ConcludeExpiredItems(); },
                null, 
                TimeSpan.Zero, 
                TimeSpan.FromSeconds(4));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
