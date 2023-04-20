using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeczkaCore.Entities
{
    public class UserPersonal
    {
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Email { get; set; }
        [MaxLength(16)]
        public string? Phone { get; set; }
    }
}
