using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeczkaCore.Entities
{
    public class UserPassword
    {
        [MaxLength(64)]
        [Required]
        public string? Old { get; set; }
        [MaxLength(64)]
        [Required]
        public string? Password { get; set; }
        [MaxLength(64)]
        [Required]
        public string? Confirm { get; set; }
    }
}
