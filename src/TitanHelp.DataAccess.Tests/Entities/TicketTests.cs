using Microsoft.VisualStudio.TestTools.UnitTesting;
using TitanHelp.DataAccess.Entities;

namespace TitanHelp.DataAccess.Tests.Entities
{
    [TestClass]
    public class TicketTests
    {
        [TestMethod]
        public void Ticket_DefaultConstructor_SetsDefaultValues()
        {
            // Act
            var ticket = new Ticket();

            // Assert
            Assert.AreEqual(0, ticket.Id);
            Assert.AreEqual(string.Empty, ticket.Name);
            Assert.AreEqual(string.Empty, ticket.ProblemDescription);
            Assert.AreEqual("Open", ticket.Status);
            Assert.AreEqual(TicketPriority.Medium, ticket.Priority);
        }

        [TestMethod]
        public void Ticket_CanSetAllProperties()
        {
            // Arrange
            var date = new DateTime(2024, 1, 1);

            // Act
            var ticket = new Ticket
            {
                Id = 1,
                Name = "Test Ticket",
                Date = date,
                ProblemDescription = "Test Description",
                Status = "Closed",
                Priority = TicketPriority.High
            };

            // Assert
            Assert.AreEqual(1, ticket.Id);
            Assert.AreEqual("Test Ticket", ticket.Name);
            Assert.AreEqual(date, ticket.Date);
            Assert.AreEqual("Test Description", ticket.ProblemDescription);
            Assert.AreEqual("Closed", ticket.Status);
            Assert.AreEqual(TicketPriority.High, ticket.Priority);
        }

        [TestMethod]
        public void TicketPriority_HasCorrectEnumValues()
        {
            // Assert
            Assert.AreEqual(0, (int)TicketPriority.Low);
            Assert.AreEqual(1, (int)TicketPriority.Medium);
            Assert.AreEqual(2, (int)TicketPriority.High);
        }

        [TestMethod]
        public void Ticket_DateProperty_DefaultsToNow()
        {
            // Arrange
            var beforeCreation = DateTime.Now;

            // Act
            var ticket = new Ticket();
            var afterCreation = DateTime.Now;

            // Assert
            Assert.IsTrue(ticket.Date >= beforeCreation);
            Assert.IsTrue(ticket.Date <= afterCreation);
        }
    }
}