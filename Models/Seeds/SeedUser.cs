using System.Collections;
using System.Security.Cryptography;
using System.Text;
using TeczkaCore.Controllers;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace TeczkaCore.Models.Seeds
{
  public class SeedUser
  {
    private TeczkaCoreContext context;
    private ILogger<UserController>? _logger;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

    public SeedUser(TeczkaCoreContext dbContext, ILogger<UserController> logger, IPasswordHasher<User> passwordHasher)
    {
      context = dbContext;
      _logger = logger;
      _passwordHasher = passwordHasher;
    }

    private string GetUniqueKey(int size)
    {
      byte[] data = new byte[4 * size];
      using (var crypto = RandomNumberGenerator.Create())
      {
        crypto.GetBytes(data);
      }
      StringBuilder result = new StringBuilder(size);
      for (int i = 0; i < size; i++)
      {
        var rnd = BitConverter.ToUInt32(data, i * 4);
        var idx = rnd % chars.Length;

        result.Append(chars[idx]);
      }
      return result.ToString();
    }

    public int SeedTable(WebApplication app)
    {
      try
      {
        context.Database.EnsureCreated();
        if (!context.Users.Any())
        {
          _logger?.LogInformation("Przygotowanie pierwszej informacji do umieszczenia w tablicy 'User'");
          User user = new User();
          user.Name = "Administrator";
          user.Email = "zbigniew.sarata@gmail.com";
          user.Phone = "501247295";
          user.TempPassword = "Passw0rd";
          user.RoleId = 4;
          context.Users.Add(user);
          context.SaveChanges();
          _logger?.LogInformation("Tablica 'User' jest już całkowicie przygotowana");
          return context.Users.Count();
        }
        else
        {
          _logger?.LogInformation("Tablica 'User' już była dostatecznie zasilona");
        }
      }
      catch (Exception ex)
      {
        _logger?.LogInformation($"Błąd inicjacji tablicy 'User' {ex.Message}");
      }
      return 0;
    }
  }
}
