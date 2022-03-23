using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Estore.Models;

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
            var user = dbc.Users.First(u => u.Username == uname);
            item.UserId = user.Id;
            if (user.Items == null)
            {
                user.Items = new List<Item>();
            }
            user.Items.Add(item);
            dbc.SaveChanges();
        }

        public void ConcludeItem(Item item, string uname)
        {
            item.BuyerId = dbc.Users.First(u => u.Username == uname).Id;
            item.Concluded = true;
            dbc.SaveChanges();
        }

        public List<string> GetImages(Item item)
        {
            List<string> result = null;
            User owner = dbc.Users.FirstOrDefault(u => u.Id == item.UserId);
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
            Item item = dbc.Items.FirstOrDefault(u => u.FindCode == findcode);
            if (item != null)
            {
                item.ImageUrls = GetImages(item);
            }
            return item;
        }

        public List<Item> GetItems(Func<Item, bool> expr)
        {
            List<Item> items = dbc.Items.Where(expr).ToList();
            foreach (Item item in items)
            {
                item.ImageUrls = GetImages(item);
            }
            return items;
        }

        public void Dispose()
        {
            dbc.Dispose();
        }
    }
}
