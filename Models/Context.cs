using Microsoft.EntityFrameworkCore;

namespace weddingPlanner.Models
{
    public class Context : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Wedding> weddings { get; set; }
        public DbSet<Rsvp> rsvp { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }
    }
} 