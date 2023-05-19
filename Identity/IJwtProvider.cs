using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeczkaCore.Models;

namespace TeczkaCore.Identity
{
  public interface IJwtProvider
  {
    public string GenerateJwtToken(User user);
    public string GenerateRefreshJwtToken(int userId);
  }
}
