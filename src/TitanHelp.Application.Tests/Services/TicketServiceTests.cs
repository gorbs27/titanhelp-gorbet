using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TitanHelp.Application.DTOs;
using TitanHelp.Application.Services;
using TitanHelp.DataAccess.Entities;
using TitanHelp.DataAccess.Interfaces;

namespace TitanHelp.Application.Tests.Services
{
    [TestClass]
    public class TicketServiceTests
    {
        private Mock<ITicketRepository> _mockRepository;
        private TicketService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<ITicketRepository>();
            _service = new TicketService(_mockRepository.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockRepository = null;
            _service = null;
        }

        #region GetAllTicketsAsync Tests

        [TestMethod]
        public async Task GetAllTicketsAsync_ReturnsAllTickets_MappedToDtos()
        {
            // Arrange
            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    Id = 1,
                    Name = "Ticket 1",
                    Date = new DateTime(2024, 1, 1),
                    ProblemDescription = "Description 1",
                    Status = "Open",
                    Priority = TicketPriority.Low
                },
                new Ticket
                {
                    Id = 2,
                    Name = "Ticket 2",
                    Date = new DateTime(2024, 1, 2),
                    ProblemDescription = "Description 2",
                    Status = "Closed",
                    Priority = TicketPriority.High
                }
            };

            _mockRepository.Setup(r => r.GetAllTicketsAsync())
                .ReturnsAsync(tickets);

            // Act
            var result = await _service.GetAllTicketsAsync();
            var resultList = result.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual("Ticket 1", resultList[0].Name);
            Assert.AreEqual("Low", resultList[0].Priority);
            Assert.AreEqual("Ticket 2", resultList[1].Name);
            Assert.AreEqual("High", resultList[1].Priority);

            _mockRepository.Verify(r => r.GetAllTicketsAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllTicketsAsync_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllTicketsAsync())
                .ReturnsAsync(new List<Ticket>());

            // Act
            var result = await _service.GetAllTicketsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            _mockRepository.Verify(r => r.GetAllTicketsAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllTicketsAsync_MapsAllPropertiesCorrectly()
        {
            // Arrange
            var testDate = new DateTime(2024, 6, 15, 10, 30, 0);
            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    Id = 5,
                    Name = "Mapping Test",
                    Date = testDate,
                    ProblemDescription = "Testing property mapping",
                    Status = "In Progress",
                    Priority = TicketPriority.Medium
                }
            };

            _mockRepository.Setup(r => r.GetAllTicketsAsync())
                .ReturnsAsync(tickets);

            // Act
            var result = await _service.GetAllTicketsAsync();
            var dto = result.First();

            // Assert
            Assert.AreEqual(5, dto.Id);
            Assert.AreEqual("Mapping Test", dto.Name);
            Assert.AreEqual(testDate, dto.Date);
            Assert.AreEqual("Testing property mapping", dto.ProblemDescription);
            Assert.AreEqual("In Progress", dto.Status);
            Assert.AreEqual("Medium", dto.Priority);
        }

        #endregion

        #region GetTicketByIdAsync Tests

        [TestMethod]
        public async Task GetTicketByIdAsync_ValidId_ReturnsTicketDto()
        {
            // Arrange
            var ticket = new Ticket
            {
                Id = 1,
                Name = "Test Ticket",
                Date = new DateTime(2024, 1, 1),
                ProblemDescription = "Test Description",
                Status = "Open",
                Priority = TicketPriority.Medium
            };

            _mockRepository.Setup(r => r.GetTicketByIdAsync(1))
                .ReturnsAsync(ticket);

            // Act
            var result = await _service.GetTicketByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test Ticket", result.Name);
            Assert.AreEqual("Medium", result.Priority);
            Assert.AreEqual("Open", result.Status);
            Assert.AreEqual(new DateTime(2024, 1, 1), result.Date);

            _mockRepository.Verify(r => r.GetTicketByIdAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task GetTicketByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetTicketByIdAsync(999))
                .ReturnsAsync((Ticket?)null);

            // Act
            var result = await _service.GetTicketByIdAsync(999);

            // Assert
            Assert.IsNull(result);
            _mockRepository.Verify(r => r.GetTicketByIdAsync(999), Times.Once);
        }

        [TestMethod]
        public async Task GetTicketByIdAsync_NegativeId_CallsRepositoryAndReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetTicketByIdAsync(-1))
                .ReturnsAsync((Ticket?)null);

            // Act
            var result = await _service.GetTicketByIdAsync(-1);

            // Assert
            Assert.IsNull(result);
            _mockRepository.Verify(r => r.GetTicketByIdAsync(-1), Times.Once);
        }

        [TestMethod]
        public async Task GetTicketByIdAsync_ZeroId_CallsRepositoryAndReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetTicketByIdAsync(0))
                .ReturnsAsync((Ticket?)null);

            // Act
            var result = await _service.GetTicketByIdAsync(0);

            // Assert
            Assert.IsNull(result);
        }

        #endregion

        #region CreateTicketAsync Tests

        [TestMethod]
        public async Task CreateTicketAsync_ValidTicketDto_ReturnsCreatedTicketDto()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Name = "New Ticket",
                ProblemDescription = "New Description",
                Priority = "High",
                Status = "Open"
            };

            var createdTicket = new Ticket
            {
                Id = 1,
                Name = "New Ticket",
                Date = DateTime.Now,
                ProblemDescription = "New Description",
                Status = "Open",
                Priority = TicketPriority.High
            };

            _mockRepository.Setup(r => r.CreateTicketAsync(It.IsAny<Ticket>()))
                .ReturnsAsync(createdTicket);

            // Act
            var result = await _service.CreateTicketAsync(ticketDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("New Ticket", result.Name);
            Assert.AreEqual("High", result.Priority);

            _mockRepository.Verify(r => r.CreateTicketAsync(It.Is<Ticket>(t =>
                t.Name == "New Ticket" &&
                t.ProblemDescription == "New Description" &&
                t.Priority == TicketPriority.High
            )), Times.Once);
        }

        [TestMethod]
        public async Task CreateTicketAsync_MapsAllPropertiesCorrectly()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Name = "Mapping Test",
                ProblemDescription = "Test mapping",
                Priority = "Low",
                Status = "Open"
            };

            Ticket capturedTicket = null;
            _mockRepository.Setup(r => r.CreateTicketAsync(It.IsAny<Ticket>()))
                .Callback<Ticket>(t => capturedTicket = t)
                .ReturnsAsync((Ticket t) => { t.Id = 1; return t; });

            // Act
            await _service.CreateTicketAsync(ticketDto);

            // Assert
            Assert.IsNotNull(capturedTicket);
            Assert.AreEqual("Mapping Test", capturedTicket.Name);
            Assert.AreEqual("Test mapping", capturedTicket.ProblemDescription);
            Assert.AreEqual(TicketPriority.Low, capturedTicket.Priority);
            Assert.AreEqual("Open", capturedTicket.Status);
        }

        [TestMethod]
        public async Task CreateTicketAsync_LowPriority_ConvertsCorrectly()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Name = "Low Priority Ticket",
                ProblemDescription = "Test",
                Priority = "Low"
            };

            _mockRepository.Setup(r => r.CreateTicketAsync(It.IsAny<Ticket>()))
                .ReturnsAsync((Ticket t) => { t.Id = 1; return t; });

            // Act
            await _service.CreateTicketAsync(ticketDto);

            // Assert
            _mockRepository.Verify(r => r.CreateTicketAsync(It.Is<Ticket>(t =>
                t.Priority == TicketPriority.Low
            )), Times.Once);
        }

        [TestMethod]
        public async Task CreateTicketAsync_MediumPriority_ConvertsCorrectly()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Name = "Medium Priority Ticket",
                ProblemDescription = "Test",
                Priority = "Medium"
            };

            _mockRepository.Setup(r => r.CreateTicketAsync(It.IsAny<Ticket>()))
                .ReturnsAsync((Ticket t) => { t.Id = 1; return t; });

            // Act
            await _service.CreateTicketAsync(ticketDto);

            // Assert
            _mockRepository.Verify(r => r.CreateTicketAsync(It.Is<Ticket>(t =>
                t.Priority == TicketPriority.Medium
            )), Times.Once);
        }

        [TestMethod]
        public async Task CreateTicketAsync_HighPriority_ConvertsCorrectly()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Name = "High Priority Ticket",
                ProblemDescription = "Test",
                Priority = "High"
            };

            _mockRepository.Setup(r => r.CreateTicketAsync(It.IsAny<Ticket>()))
                .ReturnsAsync((Ticket t) => { t.Id = 1; return t; });

            // Act
            await _service.CreateTicketAsync(ticketDto);

            // Assert
            _mockRepository.Verify(r => r.CreateTicketAsync(It.Is<Ticket>(t =>
                t.Priority == TicketPriority.High
            )), Times.Once);
        }

        #endregion

        #region UpdateTicketAsync Tests

        [TestMethod]
        public async Task UpdateTicketAsync_ValidTicketDto_ReturnsUpdatedTicketDto()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Id = 1,
                Name = "Updated Ticket",
                Date = new DateTime(2024, 1, 1),
                ProblemDescription = "Updated Description",
                Status = "In Progress",
                Priority = "High"
            };

            var updatedTicket = new Ticket
            {
                Id = 1,
                Name = "Updated Ticket",
                Date = new DateTime(2024, 1, 1),
                ProblemDescription = "Updated Description",
                Status = "In Progress",
                Priority = TicketPriority.High
            };

            _mockRepository.Setup(r => r.UpdateTicketAsync(It.IsAny<Ticket>()))
                .ReturnsAsync(updatedTicket);

            // Act
            var result = await _service.UpdateTicketAsync(ticketDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Updated Ticket", result.Name);
            Assert.AreEqual("In Progress", result.Status);
            Assert.AreEqual("High", result.Priority);

            _mockRepository.Verify(r => r.UpdateTicketAsync(It.Is<Ticket>(t =>
                t.Id == 1 &&
                t.Name == "Updated Ticket" &&
                t.Status == "In Progress"
            )), Times.Once);
        }

        [TestMethod]
        public async Task UpdateTicketAsync_ChangesStatus_UpdatesCorrectly()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Id = 1,
                Name = "Ticket",
                Date = DateTime.Now,
                ProblemDescription = "Description",
                Status = "Closed",
                Priority = "Medium"
            };

            _mockRepository.Setup(r => r.UpdateTicketAsync(It.IsAny<Ticket>()))
                .ReturnsAsync((Ticket t) => t);

            // Act
            await _service.UpdateTicketAsync(ticketDto);

            // Assert
            _mockRepository.Verify(r => r.UpdateTicketAsync(It.Is<Ticket>(t =>
                t.Status == "Closed"
            )), Times.Once);
        }

        [TestMethod]
        public async Task UpdateTicketAsync_PreservesDate_DoesNotChangeDate()
        {
            // Arrange
            var originalDate = new DateTime(2024, 1, 1, 10, 30, 0);
            var ticketDto = new TicketDto
            {
                Id = 1,
                Name = "Ticket",
                Date = originalDate,
                ProblemDescription = "Description",
                Status = "Open",
                Priority = "Medium"
            };

            _mockRepository.Setup(r => r.UpdateTicketAsync(It.IsAny<Ticket>()))
                .ReturnsAsync((Ticket t) => t);

            // Act
            await _service.UpdateTicketAsync(ticketDto);

            // Assert
            _mockRepository.Verify(r => r.UpdateTicketAsync(It.Is<Ticket>(t =>
                t.Date == originalDate
            )), Times.Once);
        }

        [TestMethod]
        public async Task UpdateTicketAsync_ChangePriority_UpdatesCorrectly()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Id = 1,
                Name = "Ticket",
                Date = DateTime.Now,
                ProblemDescription = "Description",
                Status = "Open",
                Priority = "High"
            };

            _mockRepository.Setup(r => r.UpdateTicketAsync(It.IsAny<Ticket>()))
                .ReturnsAsync((Ticket t) => t);

            // Act
            await _service.UpdateTicketAsync(ticketDto);

            // Assert
            _mockRepository.Verify(r => r.UpdateTicketAsync(It.Is<Ticket>(t =>
                t.Priority == TicketPriority.High
            )), Times.Once);
        }

        #endregion

        #region DeleteTicketAsync Tests

        [TestMethod]
        public async Task DeleteTicketAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteTicketAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteTicketAsync(1);

            // Assert
            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.DeleteTicketAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task DeleteTicketAsync_InvalidId_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteTicketAsync(999))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteTicketAsync(999);

            // Assert
            Assert.IsFalse(result);
            _mockRepository.Verify(r => r.DeleteTicketAsync(999), Times.Once);
        }

        [TestMethod]
        public async Task DeleteTicketAsync_NegativeId_CallsRepository()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteTicketAsync(-1))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteTicketAsync(-1);

            // Assert
            Assert.IsFalse(result);
            _mockRepository.Verify(r => r.DeleteTicketAsync(-1), Times.Once);
        }

        #endregion

        #region Priority Enum Mapping Tests

        [TestMethod]
        public async Task Service_HandlesAllPriorityLevels()
        {
            // Test all three priority levels
            var priorities = new[] { "Low", "Medium", "High" };

            foreach (var priority in priorities)
            {
                // Arrange
                var dto = new TicketDto
                {
                    Name = "Test",
                    ProblemDescription = "Test",
                    Priority = priority
                };

                var expectedPriority = Enum.Parse<TicketPriority>(priority);

                _mockRepository.Setup(r => r.CreateTicketAsync(It.IsAny<Ticket>()))
                    .ReturnsAsync((Ticket t) => { t.Id = 1; return t; });

                // Act
                await _service.CreateTicketAsync(dto);

                // Assert
                _mockRepository.Verify(r => r.CreateTicketAsync(It.Is<Ticket>(t =>
                    t.Priority == expectedPriority
                )), Times.Once);

                _mockRepository.Reset();
            }
        }

        [TestMethod]
        public async Task Service_MapsEntityPriorityToDtoString()
        {
            // Arrange
            var ticket = new Ticket
            {
                Id = 1,
                Name = "Test",
                Date = DateTime.Now,
                ProblemDescription = "Test",
                Status = "Open",
                Priority = TicketPriority.High
            };

            _mockRepository.Setup(r => r.GetTicketByIdAsync(1))
                .ReturnsAsync(ticket);

            // Act
            var result = await _service.GetTicketByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("High", result.Priority);
            Assert.IsInstanceOfType(result.Priority, typeof(string));
        }

        #endregion

        #region Repository Interaction Tests

        [TestMethod]
        public async Task Service_CallsRepositoryMethods_WithCorrectParameters()
        {
            // Arrange
            var tickets = new List<Ticket>();
            var ticket = new Ticket { Id = 1, Name = "Test", ProblemDescription = "Test" };

            _mockRepository.Setup(r => r.GetAllTicketsAsync()).ReturnsAsync(tickets);
            _mockRepository.Setup(r => r.GetTicketByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            _mockRepository.Setup(r => r.CreateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(ticket);
            _mockRepository.Setup(r => r.UpdateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(ticket);
            _mockRepository.Setup(r => r.DeleteTicketAsync(It.IsAny<int>())).ReturnsAsync(true);

            var dto = new TicketDto
            {
                Id = 1,
                Name = "Test",
                Date = DateTime.Now,
                ProblemDescription = "Test",
                Status = "Open",
                Priority = "Medium"
            };

            // Act
            await _service.GetAllTicketsAsync();
            await _service.GetTicketByIdAsync(1);
            await _service.CreateTicketAsync(dto);
            await _service.UpdateTicketAsync(dto);
            await _service.DeleteTicketAsync(1);

            // Assert
            _mockRepository.Verify(r => r.GetAllTicketsAsync(), Times.Once);
            _mockRepository.Verify(r => r.GetTicketByIdAsync(1), Times.Once);
            _mockRepository.Verify(r => r.CreateTicketAsync(It.IsAny<Ticket>()), Times.Once);
            _mockRepository.Verify(r => r.UpdateTicketAsync(It.IsAny<Ticket>()), Times.Once);
            _mockRepository.Verify(r => r.DeleteTicketAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task Service_DoesNotCallRepository_MultipleTimesForSingleOperation()
        {
            // Arrange
            var ticket = new Ticket { Id = 1, Name = "Test", ProblemDescription = "Test" };
            _mockRepository.Setup(r => r.GetTicketByIdAsync(1)).ReturnsAsync(ticket);

            // Act
            await _service.GetTicketByIdAsync(1);

            // Assert
            _mockRepository.Verify(r => r.GetTicketByIdAsync(1), Times.Once);
            _mockRepository.Verify(r => r.GetTicketByIdAsync(It.IsAny<int>()), Times.Once);
        }

        #endregion
    }
}