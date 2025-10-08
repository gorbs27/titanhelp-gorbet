using TitanHelp.Application.DTOs;
using TitanHelp.DataAccess.Entities;

namespace TitanHelp.Web.Tests.Helpers
{
    public static class TestDataHelper
    {
        public static Ticket CreateTestTicket(int id = 1, string name = "Test Ticket")
        {
            return new Ticket
            {
                Id = id,
                Name = name,
                Date = DateTime.Now,
                ProblemDescription = "Test Description",
                Status = "Open",
                Priority = TicketPriority.Medium
            };
        }

        public static TicketDto CreateTestTicketDto(int id = 1, string name = "Test Ticket")
        {
            return new TicketDto
            {
                Id = id,
                Name = name,
                Date = DateTime.Now,
                ProblemDescription = "Test Description",
                Status = "Open",
                Priority = "Medium"
            };
        }

        public static List<Ticket> CreateTestTickets(int count)
        {
            var tickets = new List<Ticket>();
            for (int i = 1; i <= count; i++)
            {
                tickets.Add(CreateTestTicket(i, $"Ticket {i}"));
            }
            return tickets;
        }

        public static List<TicketDto> CreateTestTicketDtos(int count)
        {
            var tickets = new List<TicketDto>();
            for (int i = 1; i <= count; i++)
            {
                tickets.Add(CreateTestTicketDto(i, $"Ticket {i}"));
            }
            return tickets;
        }
    }
}