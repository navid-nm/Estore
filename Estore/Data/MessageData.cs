using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Estore.Models;

namespace Estore.Data
{
    public class MessageData : IDisposable
    {
        private readonly EstoreDbContext dbc;

        public MessageData(EstoreDbContext context)
        {
            dbc = context;
        }

        public void AddMessage(string body, User sender, User recipient, Item item)
        {
            Message message = new Message
            {
                Date = DateTime.Now,
                SubjectItem = item,
                Recipient = recipient,
                Sender = sender,
                MessageBody = body
            };
            dbc.Messages.Add(message);
        }

        public void Dispose()
        {
            dbc.Dispose();
        }
    }
}
