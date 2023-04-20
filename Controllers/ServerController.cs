using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using TeczkaCore.Entities;

namespace TeczkaCore.Controllers
{
  [ApiController]
  [AllowAnonymous]
  [Route("api/server")]

  public class ServerController : ControllerBase
  {
    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<ServerController> _logger;

    public ServerController(TeczkaCoreContext teczkacoreContext, ILogger<ServerController> logger)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
    }

    [HttpGet]
    public ActionResult<List<ServerData>> Get()
    {
      List<ServerData> serverData = new List<ServerData>();
      try
      {
        ServerData sd = new ServerData("Machine Name", Environment.MachineName);
        serverData.Add(sd);
      }
      catch (Exception ex)
      {
        serverData.Add(new ServerData("MachineName", ex.Message));
      };
      try
      {
        ServerData sd = new ServerData("OS Version", Environment.OSVersion.ToString());
        serverData.Add(sd);
      }
      catch (Exception ex)
      {
        serverData.Add(new ServerData("OSVersion", ex.Message));
      };
      try
      {
        ServerData sd = new ServerData("Version", Environment.Version.ToString());
        serverData.Add(sd);
      }
      catch (Exception ex)
      {
        serverData.Add(new ServerData("Version", ex.Message));
      };
      try
      {
        ServerData sd = new ServerData("Processor Count", Environment.ProcessorCount.ToString());
        serverData.Add(sd);
      }
      catch (Exception ex)
      {
        serverData.Add(new ServerData("ProcessorCount", ex.Message));
      };
      try
      {
        ServerData sd = new ServerData("User Domain Name", Environment.UserDomainName);
        serverData.Add(sd);
      }
      catch (Exception ex)
      {
        serverData.Add(new ServerData("UserDomainName", ex.Message));
      };
      try
      {
        ServerData sd = new ServerData("User Name", Environment.UserName);
        serverData.Add(sd);
      }
      catch (Exception ex)
      {
        serverData.Add(new ServerData("UserName", ex.Message));
      };
      try
      {
        ServerData sd = new ServerData("Current Directory", Environment.CurrentDirectory);
        serverData.Add(sd);
      }
      catch (Exception ex)
      {
        serverData.Add(new ServerData("CurrentDirectory", ex.Message));
      };
      //try
      //{
      //  ServerData sd = new ServerData("Command Line", Environment.CommandLine);
      //  serverData.Add(sd);
      //}
      //catch (Exception ex)
      //{
      //  serverData.Add(new ServerData("CommandLine", ex.Message));
      //};
      //try
      //{
      //  ServerData sd = new ServerData("System Directory", Environment.SystemDirectory);
      //  serverData.Add(sd);
      //}
      //catch (Exception ex)
      //{
      //  serverData.Add(new ServerData("SystemDirectory", ex.Message));
      //};
      try
      {
        ServerData sd = new ServerData("Process Path", Environment.ProcessPath.ToString());
        serverData.Add(sd);
      }
      catch (Exception ex)
      {
        serverData.Add(new ServerData("ProcessPath", ex.Message));
      };
      try
      {
        ServerData sd = new ServerData("Process Id", Environment.ProcessId.ToString());
        serverData.Add(sd);
      }
      catch (Exception ex)
      {
        serverData.Add(new ServerData("ProcessId", ex.Message));
      };
      return Ok(serverData);
    }
  }
}
