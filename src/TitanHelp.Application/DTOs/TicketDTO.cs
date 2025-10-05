using System.ComponentModel.DataAnnotations;

namespace TitanHelp.Application.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Problem description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string ProblemDescription { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Open";

        [Required]
        public string Priority { get; set; } = "Medium";
    }
}