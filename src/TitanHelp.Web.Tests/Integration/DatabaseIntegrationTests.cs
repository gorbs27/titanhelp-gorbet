using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TitanHelp.DataAccess.Data;
using TitanHelp.DataAccess.Entities;
using TitanHelp.DataAccess.Repositories;

namespace TitanHelp.Web.Tests.Integration
{
    [TestClass]
    public class DatabaseIntegrationTests
    {
        private ApplicationDbContext _context;
        private TicketRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "IntegrationTestDb_" + Guid.NewGuid())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            _repository = new TicketRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task FullCrudCycle_WorksCorrectly()
        {
            // Create
            var newTicket = new Ticket
            {
                Name = "Integration Test Ticket",
                ProblemDescription = "Full CRUD test",
                Status = "Open",
                Priority = TicketPriority.High
            };

            var created = await _repository.CreateTicketAsync(newTicket);
            Assert.IsNotNull(created);
            Assert.IsTrue(created.Id > 0);

            // Read
            var retrieved = await _repository.GetTicketByIdAsync(created.Id);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("Integration Test Ticket", retrieved.Name);

            // Update
            retrieved.Name = "Updated Name";
            retrieved.Status = "Closed";
            var updated = await _repository.UpdateTicketAsync(retrieved);
            Assert.AreEqual("Updated Name", updated.Name);
            Assert.AreEqual("Closed", updated.Status);

            // Delete
            var deleted = await _repository.DeleteTicketAsync(created.Id);
            Assert.IsTrue(deleted);

            // Verify deletion
            var shouldBeNull = await _repository.GetTicketByIdAsync(created.Id);
            Assert.IsNull(shouldBeNull);
        }

        [TestMethod]
        public async Task Repository_HandlesMultipleTickets()
        {
            // Arrange - Create multiple tickets
            for (int i = 1; i <= 5; i++)
            {
                await _repository.CreateTicketAsync(new Ticket
                {
                    Name = $"Ticket {i}",
                    ProblemDescription = $"Description {i}",
                    Priority = (TicketPriority)(i % 3)
                });
            }

            // Act
            var allTickets = await _repository.GetAllTicketsAsync();

            // Assert
            Assert.IsTrue(allTickets.Count() >= 5);
        }

        [TestMethod]
        public async Task Database_EnforcesValidation()
        {
            // Arrange - Create ticket with invalid data
            var invalidTicket = new Ticket
            {
                // Missing required Name
                ProblemDescription = "Test"
            };

            // Act & Assert
            _context.Tickets.Add(invalidTicket);
            await Assert.ThrowsExceptionAsync<DbUpdateException>(
                async () => await _context.SaveChangesAsync()
            );
        }

        [TestMethod]
        public async Task Repository_OrdersTicketsByDateDescending()
        {
            // Arrange
            var oldTicket = new Ticket
            {
                Name = "Old Ticket",
                ProblemDescription = "Old",
                Date = DateTime.Now.AddDays(-5)
            };

            var newTicket = new Ticket
            {
                Name = "New Ticket",
                ProblemDescription = "New",
                Date = DateTime.Now
            };

            await _context.Tickets.AddRangeAsync(oldTicket, newTicket);
            await _context.SaveChangesAsync();

            // Act
            var tickets = await _repository.GetAllTicketsAsync();
            var ticketList = tickets.ToList();

            // Assert
            Assert.IsTrue(ticketList[0].Date > ticketList[ticketList.Count - 1].Date);
        }
    }
}