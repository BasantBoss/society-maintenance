using System.ComponentModel.DataAnnotations;

namespace SocietyApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required] public string Username { get; set; } = null!;
        [Required] public byte[] PasswordHash { get; set; } = null!;
        [Required] public byte[] PasswordSalt { get; set; } = null!;
        [Required] public Role Role { get; set; }

        public string? FullName { get; set; }
        public string? FlatNumber { get; set; }
        public string? Phone { get; set; }
    }
}