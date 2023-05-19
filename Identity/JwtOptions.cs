using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeczkaCore.Identity
{
    public class JwtOptions
    {
        public string JwtKey { get; set; }
        public string JwtRefreshKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireMinutes { get; set; }
        public int JwtRefreshExpireMinutes { get; set; }
    }
}
