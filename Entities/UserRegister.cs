using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeczkaCore.Entities
{
    public class UserRegister
    {
        [MaxLength(64)]
        [Required]
        public string Name { get; set; }
        [MaxLength(64)]
        [Required]
        public string Email { get; set; }
        [MaxLength(16)]
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        [MaxLength(64)]
        public string? Password { get; set; }
        [MaxLength(64)]
        public string? ConfirmPassword { get; set; }
    }
}
