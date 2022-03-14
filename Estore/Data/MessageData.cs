using System;
using System.Linq;
using System.Collections.Generic;
using Estore.Models;
using Microsoft.AspNetCore.Hosting;

namespace Estore.Data
{
    public class MessageData : IDisposable
    {
        private readonly EstoreDbContext dbc;
        private readonly IWebHostEnvironment env;

        public MessageData(EstoreDbContext context)
        {
            dbc = context;
        }

        public MessageData(EstoreDbContext context, IWebHostEnvironment envr)
        {
            dbc = context;
            env = envr;
        }

        public void AddMessage(string body, User sender, User recipient, Item item)
        {
            Message message = new Message
            {
                Date = DateTime.Now,
                SubjectItemId = item.Id,
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                MessageBody = body
            };
            dbc.Messages.Add(message);
            dbc.SaveChanges();
        }

        public List<Message> GetMessages(string recipientName)
        {
            User recipient = dbc.Users.First(u => u.Username == recipientName);
            List<Message> messages = dbc.Messages.Where(m => m.RecipientId == recipient.Id).ToList();
            foreach (Message msg in messages)
            {
                msg.Recipient = recipient;
                msg.Sender = dbc.Users.First(u => u.Id == msg.SenderId);
                msg.SubjectItem = dbc.Items.First(i => i.Id == msg.SubjectItemId);
            }
            return messages;
        }

        public Message GetMessage(int id)
        {
            Message msg = dbc.Messages.FirstOrDefault(m => m.Id == id);
            msg.SubjectItem = new ItemData(dbc, env).GetItem(
                dbc.Items.First(i => i.Id == msg.SubjectItemId).FindCode
            );
            msg.Sender = dbc.Users.First(u => u.Id == msg.SenderId);
            msg.Recipient = dbc.Users.First(u => u.Id == msg.RecipientId);
            return msg;
        }

        public void Dispose()
        {
            dbc.Dispose();
        }
    }
}
