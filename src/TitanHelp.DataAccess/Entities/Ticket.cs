using System.ComponentModel.DataAnnotations;

namespace TitanHelp.DataAccess.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        [MaxLength(1000)]
        public string ProblemDescription { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Open";
        [Required]
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    }

    public enum TicketPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}