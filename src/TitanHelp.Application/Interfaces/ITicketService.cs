using TitanHelp.Application.DTOs;

namespace TitanHelp.Application.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
        Task<TicketDto?> GetTicketByIdAsync(int id);
        Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);
        Task<TicketDto> UpdateTicketAsync(TicketDto ticketDto);
        Task<bool> DeleteTicketAsync(int id);
    }
}