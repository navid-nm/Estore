using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Estore.Models;

namespace Estore.Data
{
    /// <summary>
    /// Handles common data operations regarding storefronts. 
    /// </summary>
    public class StorefrontData : IDisposable
    {
        private readonly EstoreDbContext dbc;

        /// <summary>
        /// Construct an instance given a context.
        /// </summary>
        public StorefrontData(EstoreDbContext context)
        {
            dbc = context;
        }

        /// <summary>
        /// Add a given storefront to storage and attribute it to a user.
        /// </summary>
        /// <param name="sf">Storefront to add</param>
        /// <param name="uname">Username of the user</param>
        public void AddStorefront(Storefront sf, string uname)
        {
            sf.UserId = dbc.Users.First(u => u.Username == uname).Id;
            dbc.Storefronts.Add(sf);
            dbc.SaveChanges();
        }

        /// <summary>
        /// Add an item to a storefront.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="storefrontId">ID of the storefront</param>
        public void AddItem(Item item, int storefrontId)
        {
            Storefront sf = GetStorefront(storefrontId);
            item.StorefrontItem = true;
            sf.Items.Add(item);
            dbc.SaveChanges();
        }

        /// <summary>
        /// Retrieve a storefront including the mapped items.
        /// </summary>
        /// <param name="id">ID of the storefront</param>
        /// <returns>The storefront</returns>
        public Storefront GetStorefront(int id)
        {
            return dbc.Storefronts.Include(s => s.Items)
                                  .FirstOrDefault(s => s.Id == id);
        }

        public void Dispose()
        {
            dbc.Dispose();
        }
    }
}
