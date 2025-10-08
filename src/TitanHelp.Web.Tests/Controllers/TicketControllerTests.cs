using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TitanHelp.Application.DTOs;
using TitanHelp.Application.Interfaces;
using TitanHelp.Web.Controllers;

namespace TitanHelp.Web.Tests.Controllers
{
    [TestClass]
    public class TicketsControllerTests
    {
        private Mock<ITicketService> _mockService = null!;
        private TicketsController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ITicketService>();
            _controller = new TicketsController(_mockService.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockService = null!;
            _controller = null!;
        }

        #region Index Action Tests

        [TestMethod]
        public async Task Index_ReturnsViewResult_WithListOfTickets()
        {
            // Arrange
            var tickets = new List<TicketDto>
            {
                new TicketDto { Id = 1, Name = "Ticket 1", ProblemDescription = "Desc 1" },
                new TicketDto { Id = 2, Name = "Ticket 2", ProblemDescription = "Desc 2" }
            };

            _mockService.Setup(s => s.GetAllTicketsAsync())
                .ReturnsAsync(tickets);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(IEnumerable<TicketDto>));
            var model = viewResult.Model as IEnumerable<TicketDto>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count());

            _mockService.Verify(s => s.GetAllTicketsAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Index_ReturnsViewResult_WithEmptyList()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllTicketsAsync())
                .ReturnsAsync(new List<TicketDto>());

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as IEnumerable<TicketDto>;
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Count());
        }

        #endregion

        #region Details Action Tests

        [TestMethod]
        public async Task Details_WithNullId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var ticket = new TicketDto
            {
                Id = 1,
                Name = "Test Ticket",
                ProblemDescription = "Test Description"
            };

            _mockService.Setup(s => s.GetTicketByIdAsync(1))
                .ReturnsAsync(ticket);

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(TicketDto));
            var model = viewResult.Model as TicketDto;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Test Ticket", model.Name);

            _mockService.Verify(s => s.GetTicketByIdAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetTicketByIdAsync(999))
                .ReturnsAsync((TicketDto?)null);

            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        #endregion

        #region Create Action Tests

        [TestMethod]
        public void Create_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Name = "New Ticket",
                ProblemDescription = "New Description",
                Priority = "Medium"
            };

            _mockService.Setup(s => s.CreateTicketAsync(It.IsAny<TicketDto>()))
                .ReturnsAsync(ticketDto);

            // Act
            var result = await _controller.Create(ticketDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);

            _mockService.Verify(s => s.CreateTicketAsync(It.IsAny<TicketDto>()), Times.Once);
        }

        [TestMethod]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Name = "New Ticket",
                ProblemDescription = "New Description"
            };

            _controller.ModelState.AddModelError("Priority", "Priority is required");

            // Act
            var result = await _controller.Create(ticketDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(TicketDto));

            _mockService.Verify(s => s.CreateTicketAsync(It.IsAny<TicketDto>()), Times.Never);
        }

        #endregion

        #region Edit Action Tests

        [TestMethod]
        public async Task Edit_Get_WithNullId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_Get_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var ticket = new TicketDto
            {
                Id = 1,
                Name = "Test Ticket",
                ProblemDescription = "Test Description"
            };

            _mockService.Setup(s => s.GetTicketByIdAsync(1))
                .ReturnsAsync(ticket);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(TicketDto));
            var model = viewResult.Model as TicketDto;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [TestMethod]
        public async Task Edit_Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetTicketByIdAsync(999))
                .ReturnsAsync((TicketDto?)null);

            // Act
            var result = await _controller.Edit(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_Post_MismatchedIds_ReturnsNotFound()
        {
            // Arrange
            var ticketDto = new TicketDto { Id = 1 };

            // Act
            var result = await _controller.Edit(2, ticketDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var ticketDto = new TicketDto
            {
                Id = 1,
                Name = "Updated Ticket",
                Date = DateTime.Now,
                ProblemDescription = "Updated Description",
                Status = "In Progress",
                Priority = "High"
            };

            _mockService.Setup(s => s.UpdateTicketAsync(It.IsAny<TicketDto>()))
                .ReturnsAsync(ticketDto);

            // Act
            var result = await _controller.Edit(1, ticketDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);

            _mockService.Verify(s => s.UpdateTicketAsync(It.IsAny<TicketDto>()), Times.Once);
        }

        [TestMethod]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var ticketDto = new TicketDto { Id = 1 };
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Edit(1, ticketDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(TicketDto));

            _mockService.Verify(s => s.UpdateTicketAsync(It.IsAny<TicketDto>()), Times.Never);
        }

        #endregion

        #region Delete Action Tests

        [TestMethod]
        public async Task Delete_Get_WithNullId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Delete(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_Get_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var ticket = new TicketDto
            {
                Id = 1,
                Name = "Test Ticket",
                ProblemDescription = "Test Description"
            };

            _mockService.Setup(s => s.GetTicketByIdAsync(1))
                .ReturnsAsync(ticket);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult.Model, typeof(TicketDto));
        }

        [TestMethod]
        public async Task Delete_Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetTicketByIdAsync(999))
                .ReturnsAsync((TicketDto?)null);

            // Act
            var result = await _controller.Delete(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteConfirmed_Post_ValidId_RedirectsToIndex()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteTicketAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);

            _mockService.Verify(s => s.DeleteTicketAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task DeleteConfirmed_Post_CallsServiceDelete()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteTicketAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _controller.DeleteConfirmed(1);

            // Assert
            _mockService.Verify(s => s.DeleteTicketAsync(1), Times.Once);
        }

        #endregion

        #region Service Integration Tests

        [TestMethod]
        public async Task Controller_UsesServiceCorrectly_ForAllOperations()
        {
            // Arrange
            var ticket = new TicketDto { Id = 1, Name = "Test", ProblemDescription = "Test" };
            _mockService.Setup(s => s.GetAllTicketsAsync()).ReturnsAsync(new List<TicketDto> { ticket });
            _mockService.Setup(s => s.GetTicketByIdAsync(1)).ReturnsAsync(ticket);
            _mockService.Setup(s => s.CreateTicketAsync(It.IsAny<TicketDto>())).ReturnsAsync(ticket);
            _mockService.Setup(s => s.UpdateTicketAsync(It.IsAny<TicketDto>())).ReturnsAsync(ticket);
            _mockService.Setup(s => s.DeleteTicketAsync(1)).ReturnsAsync(true);

            // Act
            await _controller.Index();
            await _controller.Details(1);
            await _controller.Create(ticket);
            await _controller.Edit(1, ticket);
            await _controller.DeleteConfirmed(1);

            // Assert
            _mockService.Verify(s => s.GetAllTicketsAsync(), Times.Once);
            _mockService.Verify(s => s.GetTicketByIdAsync(1), Times.Once);
            _mockService.Verify(s => s.CreateTicketAsync(It.IsAny<TicketDto>()), Times.Once);
            _mockService.Verify(s => s.UpdateTicketAsync(It.IsAny<TicketDto>()), Times.Once);
            _mockService.Verify(s => s.DeleteTicketAsync(1), Times.Once);
        }

        #endregion
    }
}