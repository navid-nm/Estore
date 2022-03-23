using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Estore.Models;

namespace Estore.Data
{
    public class UserData : IDisposable
    {
        private readonly EstoreDbContext dbc;
        private readonly TextInfo ti;

        public UserData(EstoreDbContext context)
        {
            ti = new CultureInfo("en-GB", false).TextInfo;
            dbc = context;
        }

        public void AddUser(User user)
        {
            user.DateOfRegistration = DateTime.Now;
            user.FirstName = ti.ToTitleCase(user.FirstName);
            user.Surname = ti.ToTitleCase(user.Surname);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            dbc.Users.Add(user);
            dbc.SaveChanges();
        }

        public bool AuthenticateUser(SignIn svm)
        {
            bool conf = false;
            User user = dbc.Users.FirstOrDefault(u => u.Email == svm.Email);
            if (user != null)
            {
                conf = BCrypt.Net.BCrypt.Verify(svm.Password, user.Password);
            }
            return conf;
        }

        public void SetLocation(string name, Location location)
        {
            User user = dbc.Users.First(u => u.Username == name);
            location.PostalCode = location.PostalCode.ToUpper();
            location.Address = ti.ToTitleCase(location.Address);
            user.ShippingLocation = location;
            dbc.SaveChanges();
        }

        public bool UserHasLocation(string name)
        {
            User user = dbc.Users.Include(u => u.ShippingLocation).First(u => u.Username == name);
            return user.ShippingLocation != null;
        }

        public List<string> InUse(string email, string name)
        {
            var used = new List<string>();
            if (dbc.Users.FirstOrDefault(u => u.Email == email) != null) used.Add("Email");
            if (dbc.Users.FirstOrDefault(u => u.Username == name) != null) used.Add("Username");
            return used;
        }

        public void WriteViewed(string name, Item item)
        {
            if (item != null)
            {
                User user = dbc.Users.First(u => u.Username == name);
                if (dbc.ViewLogEntries.Where(v =>
                                    v.Item == item 
                                    && v.Viewer.Username == name).ToList().Count == 0 
                                    && user.Id != item.UserId)
                {
                    dbc.ViewLogEntries.Add(new ViewLogEntry { Item = item, Viewer = user });
                    dbc.SaveChanges();
                }
            }
        }

        public List<Item> GetViewed(string name)
        {
            User user = dbc.Users.First(u => u.Username == name);
            List<Item> outlist = new List<Item>();
            List<ViewLogEntry> vles = dbc.ViewLogEntries.Include(v => v.Item)
                                                        .Where(v => v.Viewer == user && !v.Item.Concluded)
                                                        .ToList();
            foreach (ViewLogEntry vle in vles)
            {
                outlist.Add(vle.Item);
            }
            return outlist;
        }

        public void Dispose()
        {
            dbc.Dispose();
        }
    }
}
