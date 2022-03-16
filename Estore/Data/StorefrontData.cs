using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Estore.Models;

namespace Estore.Data
{
    public class StorefrontData : IDisposable
    {
        private readonly EstoreDbContext dbc;

        public StorefrontData(EstoreDbContext context)
        {
            dbc = context;
        }

        public void AddStorefront(Storefront sf, string uname)
        {
            sf.UserId = dbc.Users.First(u => u.Username == uname).Id;
            dbc.Storefronts.Add(sf);
            dbc.SaveChanges();
        }

        public void AddItem(Item item, int storefrontId)
        {
            Storefront sf = GetStorefront(storefrontId);
            item.StorefrontItem = true;
            sf.Items.Add(item);
            dbc.SaveChanges();
        }

        public Storefront GetStorefront(int id)
        {
            return dbc.Storefronts.Include(s => s.Items).FirstOrDefault(s => s.Id == id);
        }

        public void Dispose()
        {
            dbc.Dispose();
        }
    }
}
