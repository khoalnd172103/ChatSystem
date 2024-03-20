using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PRN221ProjectGroup.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Friend> Friend { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
            var strConn = config["ConnectionStrings:DefaultConnection"];

            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure primary key for Friend entity
            modelBuilder.Entity<Friend>()
                .HasKey(f => f.RequestId);

            modelBuilder.Entity<Friend>()
            .Property(a => a.RequestId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

            //one user can send friend request to many users
            //when delete this user, also delete the friend request
            modelBuilder.Entity<Friend>()
                .HasOne(s => s.SenderUser)
                .WithMany(l => l.SentUsers)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            //one user can be sent by many users
            modelBuilder.Entity<Friend>()
                .HasOne(s => s.RecipientUser)
                .WithMany(l => l.SentByUsers)
                .HasForeignKey(s => s.RecipientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Conversation)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Participants>()
                .HasKey(p => new {p.ConversationId, p.UserId});

            modelBuilder.Entity<Participants>()
                .HasOne(p => p.Conversation)
                .WithMany(c => c.Participants)
                .HasForeignKey(p => p.ConversationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Participants>()
                .HasOne(p => p.User)
                .WithMany(u => u.ParticipatedConversations)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}