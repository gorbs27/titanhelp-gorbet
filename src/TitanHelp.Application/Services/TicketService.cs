using TitanHelp.Application.DTOs;
using TitanHelp.Application.Interfaces;
using TitanHelp.DataAccess.Entities;
using TitanHelp.DataAccess.Interfaces;

namespace TitanHelp.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
        {
            var tickets = await _ticketRepository.GetAllTicketsAsync();
            return tickets.Select(MapToDto);
        }

        public async Task<TicketDto?> GetTicketByIdAsync(int id)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(id);
            return ticket == null ? null : MapToDto(ticket);
        }

        public async Task<TicketDto> CreateTicketAsync(TicketDto ticketDto)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(ticketDto.Name))
                throw new ArgumentException("Name is required");

            if (ticketDto.Name.Length > 100)
                throw new ArgumentException("Name cannot exceed 100 characters");

            if (string.IsNullOrWhiteSpace(ticketDto.ProblemDescription))
                throw new ArgumentException("Problem description is required");

            if (ticketDto.ProblemDescription.Length > 1000)
                throw new ArgumentException("Description cannot exceed 1000 characters");

            var ticket = MapToEntity(ticketDto);
            var createdTicket = await _ticketRepository.CreateTicketAsync(ticket);
            return MapToDto(createdTicket);
        }

        public async Task<TicketDto> UpdateTicketAsync(TicketDto ticketDto)
        {
            var ticket = MapToEntity(ticketDto);
            var updatedTicket = await _ticketRepository.UpdateTicketAsync(ticket);
            return MapToDto(updatedTicket);
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            return await _ticketRepository.DeleteTicketAsync(id);
        }

        // Mapping methods
        private TicketDto MapToDto(Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                Name = ticket.Name,
                Date = ticket.Date,
                ProblemDescription = ticket.ProblemDescription,
                Status = ticket.Status,
                Priority = ticket.Priority.ToString()
            };
        }

        private Ticket MapToEntity(TicketDto dto)
        {
            return new Ticket
            {
                Id = dto.Id,
                Name = dto.Name,
                Date = dto.Date,
                ProblemDescription = dto.ProblemDescription,
                Status = dto.Status,
                Priority = Enum.Parse<TicketPriority>(dto.Priority)
            };
        }
    }
}