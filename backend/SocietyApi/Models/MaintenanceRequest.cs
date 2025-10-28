using System.ComponentModel.DataAnnotations;

namespace SocietyApi.Models
{
    public class MaintenanceRequest
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? RequestedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? AssignedTo { get; set; }
        public bool Completed { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
    }
}