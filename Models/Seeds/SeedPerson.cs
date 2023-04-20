using System.Collections;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Text;
using TeczkaCore.Controllers;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Data;

namespace TeczkaCore.Models.Seeds
{
  public class SeedPerson
  {
    private TeczkaCoreContext context;
    private ILogger<RoleController>? _logger;

    public SeedPerson(TeczkaCoreContext dbContext, ILogger<RoleController> logger)
    {
      context = dbContext;
      _logger = logger;
    }

    public int SeedTable(WebApplication app)
    {
      try
      {
        context.Database.EnsureCreated();
        if (app.Environment.IsDevelopment() && !context.Persons.Any())
        {
          int sectionId = 0;
          int userId = 0;
          _logger?.LogInformation("Przygotowanie pierwszej informacji do umieszczenia w tablicy 'Person'");
          Section[] sections = context.Sections.Where(s => s.Pdf.ToLower().EndsWith("1152_36.pdf")).ToArray();
          User[] users = context.Users.ToArray();
          if (sections.Length > 0)
          {
            sectionId = sections[0].Id;
          }
          if (users.Length > 0)
          {
            userId = users[0].Id;
          }
          Article[] articles = context.Articles.ToArray();
          String[] files = Directory.GetFiles("E:\\ZZS\\VS\\Teczka\\teczka\\src\\assets\\ocr", "*.txt");
          int firstPage = 0;
          int scanId = 0;
          foreach (string file in files)
          {
            int page = 0;
            string fName = Path.GetFileNameWithoutExtension(file);
            TextReader reader = File.OpenText(file);
            int.TryParse(fName, out page);
            string line = string.Empty;
            string last = string.Empty;
            string first = string.Empty;
            while ((line = reader.ReadLine()) != null)
            {
              line = line.Trim().Replace("  ", " ");
              int f = line.LastIndexOf(' ');
              if (f > 0)
              {
                last = line.Substring(0, f);
                first = line.Substring(f+1);

              }
              else
              {
                last = line;
              }
              Person person = new Person(last, first, 1);
              context.Persons.Add(person);
              context.SaveChanges();
              int personId = person.Id;

              if (sectionId > 0 && personId > 0 && page > 0)
              {
                if (firstPage != page)
                {
                  firstPage = page;
                  Scan[] scans = context.Scans.Where(
                    s => s.SectionId == sectionId && s.Page == page).ToArray();
                  if (scans.Length > 0 )
                  {
                    Scan scan = scans[0];
                    scanId = scan.Id;
                    scan.UserId = userId;
                    scan.Done = true;
                    context.Scans.Update(scan);
                    context.SaveChanges();
                  }
                }
                Indeks index = new Indeks();
                index.ScanId = scanId;
                index.PersonId = personId;
                index.UserId = userId;
                context.Indexes.Add(index);
                context.SaveChanges();
              }
            }
          }
          return context.Sections.Count();
        }
        else
        {
          _logger?.LogInformation("Tablica 'Person' już była dostatecznie zasilona");
        }
      }
      catch (Exception ex)
      {
        _logger?.LogInformation($"Błąd inicjacji tablicy 'Person' {ex.Message}");
      }
      return 0;
    }

  }
}
