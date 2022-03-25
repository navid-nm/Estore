using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Estore.Models;

namespace Estore.Data
{
    /// <summary>
    /// Handles common data operations regarding items. 
    /// </summary>
    public class ItemData : IDisposable
    {
        private readonly EstoreDbContext dbc;
        private readonly IWebHostEnvironment env;

        /// <summary>
        /// Constructs a context-only instance.
        /// </summary>
        public ItemData(EstoreDbContext context)
        {
            dbc = context;
        }

        /// <summary>
        /// Constructs an instance including the environment.
        /// Required for image processing.
        /// </summary>
        public ItemData(EstoreDbContext context, IWebHostEnvironment envr)
        {
            dbc = context;
            env = envr;
        }

        /// <summary>
        /// Add an item to storage.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="uname">Username of the seller</param>
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

        /// <summary>
        /// Conclude an item that has been listed.
        /// </summary>
        /// <param name="item">Item to conclude</param>
        /// <param name="uname">Username of the buyer</param>
        public void ConcludeItem(Item item, string uname)
        {
            item.BuyerId = dbc.Users.First(u => u.Username == uname).Id;
            item.Concluded = true;
            dbc.SaveChanges();
        }

        /// <summary>
        /// Retrieve the image URLs for a given item.
        /// </summary>
        /// <param name="item">Item to retrieve image URLs for</param>
        /// <returns>A list of image paths</returns>
        public List<string> GetImages(Item item)
        {
            List<string> result = null;
            User owner = dbc.Users.First(u => u.Id == item.UserId);
            string username = owner.Username;
            string path = env.ContentRootPath + "\\wwwroot\\img\\items\\"
                        + username + "\\" + item.FindCode;
            if (Directory.Exists(path))
            {
                result = Directory.GetFiles(path).ToList();
            }
            return result;
        }

        /// <summary>
        /// Retrieve an item including image URLs.
        /// </summary>
        /// <param name="findcode">Findcode corresponding to the item</param>
        /// <returns>The requested item</returns>
        public Item GetItem(string findcode)
        {
            Item item = dbc.Items.FirstOrDefault(u => u.FindCode == findcode);
            if (item != null)
            {
                item.ImageUrls = GetImages(item);
            }
            return item;
        }

        /// <summary>
        /// Retrieve a list of items according to a given expression.
        /// </summary>
        /// <param name="expr">Criteria for items</param>
        /// <returns>The requested items</returns>
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
