using System;
using System.Collections.Generic;
using System.Linq;
using Estore.Models;
using Microsoft.EntityFrameworkCore;

namespace Estore.Data
{
    public class UserData : IDisposable
    {
        private readonly EstoreDbContext dbc;

        public UserData(EstoreDbContext context)
        {
            dbc = context;
        }

        public void AddUser(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            dbc.Users.Add(user);
            dbc.SaveChanges();
        }

        public bool AuthenticateUser(SignIn svm)
        {
            bool conf = false;
            User user = dbc.Users.Where(u => u.Email == svm.Email).FirstOrDefault<User>();
            if (user != null)
            {
                conf = BCrypt.Net.BCrypt.Verify(svm.Password, user.Password);
            }
            return conf;
        }

        public List<string> InUse(string email, string username)
        {
            var used = new List<string>();
            if (dbc.Users.FirstOrDefault(u => u.Email == email) != null) used.Add("Email");
            if (dbc.Users.FirstOrDefault(u => u.Username == username) != null) used.Add("Username");
            return used;
        }

        public User GetUserByEmail(string email)
        {
            return dbc.Users.FirstOrDefault(u => u.Email == email);
        }

        public void WriteViewed(string username, Item item)
        {
            User user = dbc.Users.Where(u => u.Username == username).FirstOrDefault();
            ViewLogEntry entry = new ViewLogEntry { Item = item, Viewer = user };
            if (!dbc.ViewLogEntries.Contains(entry))
            {
                dbc.ViewLogEntries.Add(entry);
                dbc.SaveChanges();
            }
        }

        public List<Item> GetViewed(string username)
        {
            User user = dbc.Users.Where(u => u.Username == username).First();
            List<Item> outlist = new List<Item>();
            List<ViewLogEntry> vles = dbc.ViewLogEntries.Include(v => v.Item)
                                                        .Where(v => v.Viewer == user).ToList();
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
