using Microsoft.EntityFrameworkCore;
using PRN221ProjectGroup.Models;

namespace PRN221ProjectGroup.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> user { get; set; }
        public DbSet<Conversation> conversation { get; set; }
        public DbSet<Friend> friend { get; set; }
        public DbSet<Message> messages { get; set; }
        public DbSet<Participants> participants { get; set; }
        public DbSet<Photo> photos { get; set; }
    }
}
