using Microsoft.EntityFrameworkCore;

namespace Chat.Server.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.Property(u => u.Username).HasMaxLength(32);
                e.HasIndex(u => u.Username).IsUnique();
            });

            modelBuilder.Entity<Message>(e =>
            {
                e.Property(m => m.Content).HasMaxLength(1000);
                e.HasIndex(m => m.SentAt);
            });
        }
    }
}
