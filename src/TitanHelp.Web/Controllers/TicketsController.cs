using Microsoft.AspNetCore.Mvc;
using TitanHelp.Application.DTOs;
using TitanHelp.Application.Interfaces;

namespace TitanHelp.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(
            ITicketService ticketService,
            ILogger<TicketsController> logger)

        {
            _ticketService = ticketService;
            _logger = logger;
        }

        // GET: Tickets
        // Displays all tickets in a table format
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Retrieving all tickets");
                var tickets = await _ticketService.GetAllTicketsAsync();
                return View(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving tickets");
                TempData["ErrorMessage"] = "An error occurred while loading tickets. Please try again.";
                return View(new List<TicketDto>());
            }
        }

        // GET: Tickets/Details/x
        // Shows detailed information for a specific ticket
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null id");
                return NotFound();
            }
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id.Value);

                if (ticket == null)
                {
                    _logger.LogWarning("Ticket with id {TicketId} not found", id);
                    return NotFound();
                }

                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving ticket {TicketId}", id);
                return NotFound();
            }
        }

        // GET: Tickets/Create
        // Displays the ticket creation form
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // Processes the ticket creation form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ProblemDescription,Priority")] TicketDto ticket)
        {
            // Remove Id and Date from validation since they're auto-generated
            ModelState.Remove("Id");
            ModelState.Remove("Date");
            ModelState.Remove("Status");

            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            try
            {
                _logger.LogInformation("Creating new ticket: {TicketName}", ticket.Name);

                // Set default values
                ticket.Status = "Open";
                ticket.Date = DateTime.Now;

                var createdTicket = await _ticketService.CreateTicketAsync(ticket);

                TempData["SuccessMessage"] = $"Ticket '{createdTicket.Name}' created successfully!";
                _logger.LogInformation("Ticket created successfully with ID: {TicketId}", createdTicket.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating ticket");
                ModelState.AddModelError("", ex.Message);
                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating ticket");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(ticket);
            }
        }

        // GET: Tickets/Edit/x
        // Displays the ticket editing form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id.Value);
                if (ticket == null)
                {
                    return NotFound();
                }

                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving ticket for editing");
                return NotFound();
            }
        }

        // POST: Tickets/Edit/x
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,ProblemDescription,Status,Priority")]) TicketDto ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            try
            {
                await _ticketService.UpdateTicketAsync(ticket);
                TempData["SuccessMessage"] = "Ticket updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating ticket");
                ModelState.AddModelError("", "An error occurred while updating the ticket.");
                return View(ticket);
            }
        }

        // GET: Tickets/Delete/x
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id.Value);
                if (ticket == null)
                {
                    return NotFound();
                }

                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving ticket for deletion");
                return NotFound();
            }
        }

        // POST: Tickets/Delete/x
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _ticketService.DeleteTicketAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Ticket deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ticket not found.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting ticket");
                TempData["ErrorMessage"] = "An error occurred while deleting the ticket.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
