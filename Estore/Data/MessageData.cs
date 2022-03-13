using System;
using System.Linq;
using System.Collections.Generic;
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
            return new Message();
        }

        public void Dispose()
        {
            dbc.Dispose();
        }
    }
}
