using AutoMapper.Internal;
using TeczkaCore.Models;

namespace TeczkaCore.Services
{
  public interface IMailService
  {
    Task SendEmailAsync(MailRequest mailRequest);
  }
}
