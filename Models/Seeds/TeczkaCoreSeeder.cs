using System.Collections;
using System.Security.Policy;
using TeczkaCore.Models.Interfaces;
using TeczkaCore.Models.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace TeczkaCore.Models.Seeds
{
  public class Vars
  {
    public int val_int = 0;
    public decimal val_dec = 0;
    public string val_str = "";
    public DateTime val_dt = new DateTime();
  }
  public class TeczkaCoreSeeder
  {
    private readonly TeczkaCoreContext _TeczkaCoreContext;
    private WebApplication _app;

    public TeczkaCoreSeeder(TeczkaCoreContext TeczkaCoreContext)
    {
      _TeczkaCoreContext = TeczkaCoreContext;
    }
    public void SetApplication(WebApplication app)
    {
      _app = app;
    }

    public static Vars GetVal(CreatedModel model, DictionaryEntry de)
    {
      Vars vars = new Vars();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
      object val = de.Value;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
      string key = (string)de.Key;
      if (val != null)
      {
        var type = val.GetType();
        if (type == typeof(int))
        {
          vars.val_int = (int)val;
        }
        else if (type == typeof(int))
        {
          vars.val_int = (int)val;
        }
        else if (type == typeof(uint))
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
          string str_int = val.ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
          int.TryParse(str_int, out vars.val_int);
        }
        else if (type == typeof(decimal))
        {
          vars.val_dec = (decimal)val;
        }
        else if (type == typeof(string))
          vars.val_str = (string)val;
        else if (type == typeof(DateTime))
          vars.val_dt = (DateTime)val;
        else
        {
        }
        switch (key)
        {
          case "created":
            model.Created = vars.val_dt;
            break;
          default:
            break;
        }
      }
      return vars;
    }


    public bool SeedRoles()
    {
      var scopedFactory = _app.Services.GetService<IServiceScopeFactory>();
      using (var scope = scopedFactory?.CreateScope())
      {
        var services = scope?.ServiceProvider;
        if (services != null)
        {
          var service = scope?.ServiceProvider.GetService<SeedRole>();
          if (service != null)
          {
            int result = service.SeedTable(_app);
            if (result > 0)
            {
              _TeczkaCoreContext.SaveChanges();
              return true;
            }
          }
        }
      }
      return false;
    }

    public bool SeedUsers()
    {
      var scopedFactory = _app.Services.GetService<IServiceScopeFactory>();
      using (var scope = scopedFactory?.CreateScope())
      {
        var services = scope?.ServiceProvider;
        if (services != null)
        {
          var service = scope?.ServiceProvider.GetService<SeedUser>();
          if (service != null)
          {
            int result = service.SeedTable(_app);
            if (result > 0)
            {
              _TeczkaCoreContext.SaveChanges();
              return true;
            }
          }
        }
      }
      return false;
    }

    public bool SeedArticles()
    {
      var scopedFactory = _app.Services.GetService<IServiceScopeFactory>();
      using (var scope = scopedFactory?.CreateScope())
      {
        var services = scope?.ServiceProvider;
        if (services != null)
        {
          var service = scope?.ServiceProvider.GetService<SeedArticle>();
          if (service != null)
          {
            int result = service.SeedTable(_app);
            if (result > 0)
            {
              _TeczkaCoreContext.SaveChanges();
              return true;
            }
          }
        }
      }
      return false;
    }

    public bool SeedSections()
    {
      var scopedFactory = _app.Services.GetService<IServiceScopeFactory>();
      using (var scope = scopedFactory?.CreateScope())
      {
        var services = scope?.ServiceProvider;
        if (services != null)
        {
          var service = scope?.ServiceProvider.GetService<SeedSection>();
          if (service != null)
          {
            int result = service.SeedTable(_app);
            if (result > 0)
            {
              _TeczkaCoreContext.SaveChanges();
              return true;
            }
          }
        }
      }
      return false;
    }

    public bool SeedPersons()
    {
      var scopedFactory = _app.Services.GetService<IServiceScopeFactory>();
      using (var scope = scopedFactory?.CreateScope())
      {
        var services = scope?.ServiceProvider;
        if (services != null)
        {
          var service = scope?.ServiceProvider.GetService<SeedPerson>();
          if (service != null)
          {
            int result = service.SeedTable(_app);
            if (result > 0)
            {
              _TeczkaCoreContext.SaveChanges();
              return true;
            }
          }
        }
      }
      return false;
    }

    public bool SeedScans()
    {
      var scopedFactory = _app.Services.GetService<IServiceScopeFactory>();
      using (var scope = scopedFactory?.CreateScope())
      {
        var services = scope?.ServiceProvider;
        if (services != null)
        {
          var service = scope?.ServiceProvider.GetService<SeedScan>();
          if (service != null)
          {
            int result = service.SeedTable(_app);
            if (result > 0)
            {
              _TeczkaCoreContext.SaveChanges();
              return true;
            }
          }
        }
      }
      return false;
    }

  }
}
