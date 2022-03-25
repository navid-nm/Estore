using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Estore.Models;

namespace Estore.Data
{
    /// <summary>
    /// Handles common data operations regarding users. 
    /// </summary>
    public class UserData : IDisposable
    {
        private readonly EstoreDbContext dbc;
        private readonly TextInfo ti;

        /// <summary>
        /// Construct a context-only instance.
        /// </summary>
        public UserData(EstoreDbContext context)
        {
            ti = new CultureInfo("en-GB", false).TextInfo;
            dbc = context;
        }

        /// <summary>
        /// Process and add a user to storage.
        /// </summary>
        /// <param name="user">User to add</param>
        public void AddUser(User user)
        {
            user.DateOfRegistration = DateTime.Now;
            user.FirstName = ti.ToTitleCase(user.FirstName);
            user.Surname = ti.ToTitleCase(user.Surname);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            dbc.Users.Add(user);
            dbc.SaveChanges();
        }

        /// <summary>
        /// Verify the sign in of an unauthenticated user.
        /// </summary>
        /// <param name="svm">Sign in attempt</param>
        /// <returns>True if verification passes</returns>
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

        /// <summary>
        /// Set the location of a user.
        /// </summary>
        /// <param name="name">Username of the user</param>
        /// <param name="location">Location to set for the user</param>
        public void SetLocation(string name, Location location)
        {
            User user = dbc.Users.First(u => u.Username == name);
            location.PostalCode = location.PostalCode.ToUpper();
            location.Address = ti.ToTitleCase(location.Address);
            user.ShippingLocation = location;
            dbc.SaveChanges();
        }

        /// <summary>
        /// Determine whether the user has provided their location.
        /// </summary>
        /// <param name="name">Username of the user</param>
        /// <returns>True if there exists a corresponding location</returns>
        public bool UserHasLocation(string name)
        {
            User user = dbc.Users.Include(u => u.ShippingLocation).First(u => u.Username == name);
            return user.ShippingLocation != null;
        }

        /// <summary>
        /// Retrieves a list of fields that are currently taken.
        /// </summary>
        /// <param name="email">Proposed email</param>
        /// <param name="name">Proposed username</param>
        /// <returns>List of taken fields</returns>
        public List<string> InUse(string email, string name)
        {
            var used = new List<string>();
            if (dbc.Users.FirstOrDefault(u => u.Email == email) != null) used.Add("Email");
            if (dbc.Users.FirstOrDefault(u => u.Username == name) != null) used.Add("Username");
            return used;
        }

        /// <summary>
        /// Add a record of the user having viewed an item
        /// </summary>
        /// <param name="name">Username of the user</param>
        /// <param name="item">Item that was viewed</param>
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

        /// <summary>
        /// Retrieve the items that a user has viewed.
        /// </summary>
        /// <param name="name">Username of the user</param>
        /// <returns>Items that the user has viewed</returns>
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
