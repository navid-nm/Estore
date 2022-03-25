using Microsoft.EntityFrameworkCore;
using Estore.Models;

namespace Estore.Data
{
    /// <summary>
    /// Provides DbSets to abstract data access.
    /// </summary>
    public class EstoreDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Storefront> Storefronts { get; set; }
        public DbSet<ViewLogEntry> ViewLogEntries { get; set; }

        public EstoreDbContext(DbContextOptions<EstoreDbContext> options) : base(options) { }
        public EstoreDbContext() { }
    }
}
