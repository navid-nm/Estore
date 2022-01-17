using Microsoft.EntityFrameworkCore;
using Estore.Models;

namespace Estore.Data
{
    public class EstoreDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }

        public EstoreDbContext(DbContextOptions<EstoreDbContext> options) : base(options) { }
    }
}
