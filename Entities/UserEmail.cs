using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeczkaCore.Entities
{
  public class UserEmail
  {
    public string Sender { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
  }

  public class MailSettings
  {
    public string Mail { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
  }
}
