using Estore.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Estore.Data
{
    public class ItemData : IDisposable
    {
        private readonly EstoreDbContext dbc;
        private readonly IWebHostEnvironment env;

        public ItemData(EstoreDbContext context)
        {
            dbc = context;
        }

        public ItemData(EstoreDbContext context, IWebHostEnvironment envr)
        {
            dbc = context;
            env = envr;
        }

        public void AddItem(Item item, string uname)
        {
            /*
             * Username is passed as a string because User object should be created in AddItem
             * (for EF).
             */
            var user = dbc.Users.Where(u => u.Username == uname).First();
            item.UserId = user.Id;
            if (user.Items == null)
            {
                user.Items = new List<Item>();
            }
            user.Items.Add(item);
            dbc.SaveChanges();
        }

        public List<string> GetImages(Item item)
        {
            List<string> result = null;
            User owner = dbc.Users.Where(u => u.Id == item.UserId).FirstOrDefault();
            string username = owner.Username;
            string path = env.ContentRootPath + "\\wwwroot\\img\\items\\"
                + username + "\\" + item.FindCode;
            if (Directory.Exists(path))
            {
                result = Directory.GetFiles(path).ToList();
            }
            return result;
        }

        public Item GetItem(string findcode)
        {
            Item item = dbc.Items.Where(u => u.FindCode == findcode).FirstOrDefault();
            if (item != null)
            {
                item.ImageUrls = GetImages(item);
            }
            return item;
        }

        public void Dispose()
        {
            dbc.Dispose();
        }
    }
}
