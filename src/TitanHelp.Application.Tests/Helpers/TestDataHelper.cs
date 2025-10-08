using TitanHelp.Application.DTOs;
using TitanHelp.DataAccess.Entities;

namespace TitanHelp.Application.Tests.Helpers
{
    /// <summary>
    /// Helper class for creating test data objects
    /// </summary>
    public static class TestDataHelper
    {
        #region Ticket Entity Helpers

        public static Ticket CreateTestTicket(
            int id = 1,
            string name = "Test Ticket",
            DateTime? date = null,
            string problemDescription = "Test Description",
            string status = "Open",
            TicketPriority priority = TicketPriority.Medium)
        {
            return new Ticket
            {
                Id = id,
                Name = name,
                Date = date ?? DateTime.Now,
                ProblemDescription = problemDescription,
                Status = status,
                Priority = priority
            };
        }

        public static List<Ticket> CreateTestTickets(int count)
        {
            var tickets = new List<Ticket>();
            for (int i = 1; i <= count; i++)
            {
                tickets.Add(CreateTestTicket(
                    id: i,
                    name: $"Ticket {i}",
                    problemDescription: $"Description {i}",
                    priority: (TicketPriority)(i % 3)
                ));
            }
            return tickets;
        }

        public static Ticket CreateLowPriorityTicket(int id = 1)
        {
            return CreateTestTicket(id: id, priority: TicketPriority.Low);
        }

        public static Ticket CreateMediumPriorityTicket(int id = 1)
        {
            return CreateTestTicket(id: id, priority: TicketPriority.Medium);
        }

        public static Ticket CreateHighPriorityTicket(int id = 1)
        {
            return CreateTestTicket(id: id, priority: TicketPriority.High);
        }

        public static Ticket CreateOpenTicket(int id = 1)
        {
            return CreateTestTicket(id: id, status: "Open");
        }

        public static Ticket CreateClosedTicket(int id = 1)
        {
            return CreateTestTicket(id: id, status: "Closed");
        }

        public static Ticket CreateInProgressTicket(int id = 1)
        {
            return CreateTestTicket(id: id, status: "In Progress");
        }

        #endregion

        #region TicketDto Helpers

        public static TicketDto CreateTestTicketDto(
            int id = 1,
            string name = "Test Ticket",
            DateTime? date = null,
            string problemDescription = "Test Description",
            string status = "Open",
            string priority = "Medium")
        {
            return new TicketDto
            {
                Id = id,
                Name = name,
                Date = date ?? DateTime.Now,
                ProblemDescription = problemDescription,
                Status = status,
                Priority = priority
            };
        }

        public static List<TicketDto> CreateTestTicketDtos(int count)
        {
            var tickets = new List<TicketDto>();
            for (int i = 1; i <= count; i++)
            {
                var priorityIndex = i % 3;
                var priority = priorityIndex == 0 ? "Low" : priorityIndex == 1 ? "Medium" : "High";

                tickets.Add(CreateTestTicketDto(
                    id: i,
                    name: $"Ticket {i}",
                    problemDescription: $"Description {i}",
                    priority: priority
                ));
            }
            return tickets;
        }

        public static TicketDto CreateLowPriorityTicketDto(int id = 1)
        {
            return CreateTestTicketDto(id: id, priority: "Low");
        }

        public static TicketDto CreateMediumPriorityTicketDto(int id = 1)
        {
            return CreateTestTicketDto(id: id, priority: "Medium");
        }

        public static TicketDto CreateHighPriorityTicketDto(int id = 1)
        {
            return CreateTestTicketDto(id: id, priority: "High");
        }

        public static TicketDto CreateOpenTicketDto(int id = 1)
        {
            return CreateTestTicketDto(id: id, status: "Open");
        }

        public static TicketDto CreateClosedTicketDto(int id = 1)
        {
            return CreateTestTicketDto(id: id, status: "Closed");
        }

        public static TicketDto CreateInProgressTicketDto(int id = 1)
        {
            return CreateTestTicketDto(id: id, status: "In Progress");
        }

        #endregion

        #region Invalid Data Helpers

        public static TicketDto CreateInvalidTicketDto_MissingName()
        {
            return new TicketDto
            {
                Name = "",
                ProblemDescription = "Test Description",
                Priority = "Medium"
            };
        }

        public static TicketDto CreateInvalidTicketDto_MissingDescription()
        {
            return new TicketDto
            {
                Name = "Test Ticket",
                ProblemDescription = "",
                Priority = "Medium"
            };
        }

        public static TicketDto CreateInvalidTicketDto_NameTooLong()
        {
            return new TicketDto
            {
                Name = new string('A', 101),
                ProblemDescription = "Test Description",
                Priority = "Medium"
            };
        }

        public static TicketDto CreateInvalidTicketDto_DescriptionTooLong()
        {
            return new TicketDto
            {
                Name = "Test Ticket",
                ProblemDescription = new string('B', 1001),
                Priority = "Medium"
            };
        }

        #endregion

        #region Boundary Data Helpers

        public static TicketDto CreateTicketDto_MaxNameLength()
        {
            return new TicketDto
            {
                Name = new string('A', 100),
                ProblemDescription = "Test Description",
                Priority = "Medium"
            };
        }

        public static TicketDto CreateTicketDto_MaxDescriptionLength()
        {
            return new TicketDto
            {
                Name = "Test Ticket",
                ProblemDescription = new string('B', 1000),
                Priority = "Medium"
            };
        }

        public static TicketDto CreateTicketDto_MinimalData()
        {
            return new TicketDto
            {
                Name = "A",
                ProblemDescription = "B",
                Priority = "Low"
            };
        }

        #endregion

        #region Date Helpers

        public static Ticket CreateTicketWithDate(DateTime date, int id = 1)
        {
            return CreateTestTicket(id: id, date: date);
        }

        public static TicketDto CreateTicketDtoWithDate(DateTime date, int id = 1)
        {
            return CreateTestTicketDto(id: id, date: date);
        }

        public static Ticket CreateOldTicket(int id = 1, int daysOld = 30)
        {
            return CreateTestTicket(id: id, date: DateTime.Now.AddDays(-daysOld));
        }

        public static Ticket CreateRecentTicket(int id = 1)
        {
            return CreateTestTicket(id: id, date: DateTime.Now.AddHours(-1));
        }

        #endregion

        #region Bulk Data Helpers

        public static List<Ticket> CreateTicketsWithDifferentPriorities()
        {
            return new List<Ticket>
            {
                CreateLowPriorityTicket(1),
                CreateMediumPriorityTicket(2),
                CreateHighPriorityTicket(3)
            };
        }

        public static List<Ticket> CreateTicketsWithDifferentStatuses()
        {
            return new List<Ticket>
            {
                CreateOpenTicket(1),
                CreateInProgressTicket(2),
                CreateClosedTicket(3)
            };
        }

        public static List<TicketDto> CreateTicketDtosWithDifferentPriorities()
        {
            return new List<TicketDto>
            {
                CreateLowPriorityTicketDto(1),
                CreateMediumPriorityTicketDto(2),
                CreateHighPriorityTicketDto(3)
            };
        }

        public static List<TicketDto> CreateTicketDtosWithDifferentStatuses()
        {
            return new List<TicketDto>
            {
                CreateOpenTicketDto(1),
                CreateInProgressTicketDto(2),
                CreateClosedTicketDto(3)
            };
        }

        #endregion
    }
}