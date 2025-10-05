using Microsoft.EntityFrameworkCore;
using TitanHelp.DataAccess.Entities;

namespace TitanHelp.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure Ticket entity
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ProblemDescription).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Priority).IsRequired();
            });

            // Seed initial data
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket
                {
                    Id = 1,
                    Name = "Sample Ticket",
                    Date = new DateTime(2025, 9, 29, 10, 0, 0),
                    ProblemDescription = "This is a sample ticket for testing purposes.",
                    Status = "Open",
                    Priority = TicketPriority.Medium
                }
            );
        }
    }
}