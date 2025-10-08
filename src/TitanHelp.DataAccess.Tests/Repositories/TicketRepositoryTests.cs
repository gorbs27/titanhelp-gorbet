using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TitanHelp.DataAccess.Data;
using TitanHelp.DataAccess.Entities;
using TitanHelp.DataAccess.Repositories;

namespace TitanHelp.DataAccess.Tests.Repositories
{
    [TestClass]
    public class TicketRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private TicketRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new TicketRepository(_context);

            // Seed test data
            SeedTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedTestData()
        {
            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    Id = 1,
                    Name = "Test Ticket 1",
                    Date = new DateTime(2024, 1, 1),
                    ProblemDescription = "Description 1",
                    Status = "Open",
                    Priority = TicketPriority.Low
                },
                new Ticket
                {
                    Id = 2,
                    Name = "Test Ticket 2",
                    Date = new DateTime(2024, 1, 2),
                    ProblemDescription = "Description 2",
                    Status = "Closed",
                    Priority = TicketPriority.High
                }
            };

            _context.Tickets.AddRange(tickets);
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task GetAllTicketsAsync_ReturnsAllTickets_OrderedByDateDescending()
        {
            // Act
            var result = await _repository.GetAllTicketsAsync();
            var ticketList = result.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, ticketList.Count);
            Assert.AreEqual("Test Ticket 2", ticketList[0].Name); // Most recent first
            Assert.AreEqual("Test Ticket 1", ticketList[1].Name);
        }

        [TestMethod]
        public async Task GetAllTicketsAsync_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            _context.Tickets.RemoveRange(_context.Tickets);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllTicketsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task GetTicketByIdAsync_ValidId_ReturnsTicket()
        {
            // Act
            var result = await _repository.GetTicketByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test Ticket 1", result.Name);
            Assert.AreEqual("Description 1", result.ProblemDescription);
            Assert.AreEqual("Open", result.Status);
            Assert.AreEqual(TicketPriority.Low, result.Priority);
        }

        [TestMethod]
        public async Task GetTicketByIdAsync_InvalidId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetTicketByIdAsync(999);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreateTicketAsync_ValidTicket_AddsTicketToDatabase()
        {
            // Arrange
            var newTicket = new Ticket
            {
                Name = "New Ticket",
                ProblemDescription = "New Description",
                Status = "Open",
                Priority = TicketPriority.Medium
            };

            // Act
            var result = await _repository.CreateTicketAsync(newTicket);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual("New Ticket", result.Name);
            Assert.IsTrue((DateTime.Now - result.Date).TotalSeconds < 5); // Date was set to now

            // Verify in database
            var ticketInDb = await _context.Tickets.FindAsync(result.Id);
            Assert.IsNotNull(ticketInDb);
            Assert.AreEqual("New Ticket", ticketInDb.Name);
        }

        [TestMethod]
        public async Task CreateTicketAsync_SetsDateToCurrentTime()
        {
            // Arrange
            var newTicket = new Ticket
            {
                Name = "Date Test",
                ProblemDescription = "Testing date",
                Date = new DateTime(2020, 1, 1) // Old date
            };

            var beforeCreate = DateTime.Now;

            // Act
            var result = await _repository.CreateTicketAsync(newTicket);
            var afterCreate = DateTime.Now;

            // Assert
            Assert.IsTrue(result.Date >= beforeCreate);
            Assert.IsTrue(result.Date <= afterCreate);
            Assert.AreNotEqual(new DateTime(2020, 1, 1), result.Date);
        }

        [TestMethod]
        public async Task UpdateTicketAsync_ValidTicket_UpdatesTicketInDatabase()
        {
            // Arrange
            var ticketToUpdate = await _context.Tickets.FindAsync(1);
            Assert.IsNotNull(ticketToUpdate);

            _context.Entry(ticketToUpdate).State = EntityState.Detached;

            ticketToUpdate.Name = "Updated Name";
            ticketToUpdate.Status = "In Progress";
            ticketToUpdate.Priority = TicketPriority.High;

            // Act
            var result = await _repository.UpdateTicketAsync(ticketToUpdate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Name", result.Name);
            Assert.AreEqual("In Progress", result.Status);
            Assert.AreEqual(TicketPriority.High, result.Priority);

            // Verify in database
            var updatedTicket = await _context.Tickets.FindAsync(1);
            Assert.IsNotNull(updatedTicket);
            Assert.AreEqual("Updated Name", updatedTicket.Name);
            Assert.AreEqual("In Progress", updatedTicket.Status);
        }

        [TestMethod]
        public async Task DeleteTicketAsync_ValidId_RemovesTicketAndReturnsTrue()
        {
            // Arrange
            var initialCount = await _context.Tickets.CountAsync();

            // Act
            var result = await _repository.DeleteTicketAsync(1);

            // Assert
            Assert.IsTrue(result);
            var finalCount = await _context.Tickets.CountAsync();
            Assert.AreEqual(initialCount - 1, finalCount);

            var deletedTicket = await _context.Tickets.FindAsync(1);
            Assert.IsNull(deletedTicket);
        }

        [TestMethod]
        public async Task DeleteTicketAsync_InvalidId_ReturnsFalse()
        {
            // Arrange
            var initialCount = await _context.Tickets.CountAsync();

            // Act
            var result = await _repository.DeleteTicketAsync(999);

            // Assert
            Assert.IsFalse(result);
            var finalCount = await _context.Tickets.CountAsync();
            Assert.AreEqual(initialCount, finalCount); // Count unchanged
        }

        [TestMethod]
        public async Task SaveChangesAsync_WithChanges_ReturnsTrue()
        {
            // Arrange
            var ticket = await _context.Tickets.FindAsync(1);
            Assert.IsNotNull(ticket);
            ticket.Name = "Modified";

            // Act
            var result = await _repository.SaveChangesAsync();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task SaveChangesAsync_NoChanges_ReturnsFalse()
        {
            // Act
            var result = await _repository.SaveChangesAsync();

            // Assert
            Assert.IsFalse(result);
        }
    }
}