using Microsoft.EntityFrameworkCore;

using TinyCard.Core.Model;

namespace TinyCard.Core.Data
{
    public class TinyCardDbContext : DbContext
    {
        public TinyCardDbContext(DbContextOptions<TinyCardDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Card
            modelBuilder.Entity<Card>()
                .ToTable("Cards");

            modelBuilder.Entity<Card>()
               .HasIndex(c => c.CardNumber)
               .IsUnique();

            // Limit
            modelBuilder.Entity<Limit>()
                .ToTable("Limits");

            modelBuilder.Entity<Limit>()
               .HasIndex(l => l.ID)
               .IsUnique();
        }
    }
}
