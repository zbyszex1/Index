using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeczkaCore.Entities
{
    public class PersonUpdate
    {
        public string? Last { get; set; }
        public string? First { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; } = 0;
  }
}
