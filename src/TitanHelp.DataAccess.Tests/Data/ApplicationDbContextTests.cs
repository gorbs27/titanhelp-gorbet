using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TitanHelp.DataAccess.Data;
using TitanHelp.DataAccess.Entities;

namespace TitanHelp.DataAccess.Tests.Data
{
    [TestClass]
    public class ApplicationDbContextTests
    {
        private ApplicationDbContext _context = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public void DbContext_HasTicketsDbSet()
        {
            // Assert
            Assert.IsNotNull(_context.Tickets);
        }

        [TestMethod]
        public async Task DbContext_SeedData_ContainsSampleTicket()
        {
            // Act
            var tickets = await _context.Tickets.ToListAsync();

            // Assert
            Assert.IsTrue(tickets.Count > 0);
            Assert.IsTrue(tickets.Any(t => t.Name == "Sample Ticket"));
        }

        [TestMethod]
        public async Task DbContext_CanAddTicket()
        {
            // Arrange
            var ticket = new Ticket
            {
                Name = "Context Test Ticket",
                ProblemDescription = "Testing context",
                Date = DateTime.Now,
                Status = "Open",
                Priority = TicketPriority.Medium
            };

            // Act
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // Assert
            var savedTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Name == "Context Test Ticket");
            Assert.IsNotNull(savedTicket);
        }

        [TestMethod]
        public async Task DbContext_TicketEntity_HasRequiredConstraints()
        {
            // Arrange
            var invalidTicket = new Ticket
            {
                // Missing required fields - Name will be empty string by default
                ProblemDescription = "", // Empty required field
                Date = DateTime.Now
            };

            // Act & Assert
            _context.Tickets.Add(invalidTicket);
            await Assert.ThrowsExceptionAsync<DbUpdateException>(
                async () => await _context.SaveChangesAsync()
            );
        }

        [TestMethod]
        public async Task DbContext_TicketName_EnforcesMaxLength()
        {
            // Arrange
            var ticket = new Ticket
            {
                Name = new string('A', 101), // Exceeds 100 character limit
                ProblemDescription = "Test",
                Date = DateTime.Now,
                Status = "Open",
                Priority = TicketPriority.Low
            };

            // Act & Assert
            _context.Tickets.Add(ticket);
            await Assert.ThrowsExceptionAsync<DbUpdateException>(
                async () => await _context.SaveChangesAsync()
            );
        }

        [TestMethod]
        public async Task DbContext_ProblemDescription_EnforcesMaxLength()
        {
            // Arrange
            var ticket = new Ticket
            {
                Name = "Test",
                ProblemDescription = new string('B', 1001), // Exceeds 1000 character limit
                Date = DateTime.Now,
                Status = "Open",
                Priority = TicketPriority.Low
            };

            // Act & Assert
            _context.Tickets.Add(ticket);
            await Assert.ThrowsExceptionAsync<DbUpdateException>(
                async () => await _context.SaveChangesAsync()
            );
        }
    }
}