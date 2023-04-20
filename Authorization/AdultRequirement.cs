using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TeczkaCore.Authorization
{
    public class AdultRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get;}

        public AdultRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
