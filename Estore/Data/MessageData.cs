using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Estore.Models;

namespace Estore.Data
{
    /// <summary>
    /// Handles common data operations regarding messages. 
    /// </summary>
    public class MessageData : IDisposable
    {
        private readonly EstoreDbContext dbc;
        private readonly IWebHostEnvironment env;

        /// <summary>
        /// Construct a context-only instance.
        /// </summary>
        public MessageData(EstoreDbContext context)
        {
            dbc = context;
        }

        /// <summary>
        /// Construct an instance including the environment.
        /// Required for previews.
        /// </summary>
        public MessageData(EstoreDbContext context, IWebHostEnvironment envr)
        {
            dbc = context;
            env = envr;
        }

        /// <summary>
        /// Add a message to storage.
        /// </summary>
        /// <param name="body">HTML content of the message</param>
        /// <param name="sender">Sender of the message</param>
        /// <param name="recipient">Recipient of the message</param>
        /// <param name="item">Subject item of the message</param>
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

        /// <summary>
        /// Retrieve all messages for a recipient.
        /// </summary>
        /// <param name="recipientName">Username of the recipient</param>
        /// <returns>List of messages addressed to the recipient</returns>
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

        /// <summary>
        /// Retrieve a message with pre-populated recipient, subject item and sender attributes.
        /// </summary>
        /// <param name="id">ID of the message</param>
        /// <returns>The requested message</returns>
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
