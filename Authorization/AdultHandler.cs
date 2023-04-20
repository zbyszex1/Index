using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace TeczkaCore.Authorization
{
    public class AdultHandler : AuthorizationHandler<AdultRequirement>, IAuthorizationRequirement
    {
        private readonly ILogger<AdultHandler> _logger;

        public AdultHandler(ILogger<AdultHandler> logger)
        {
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdultRequirement requirement)
        {
            try
            {
                var userEmail = context?.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
                var dateOfBirth = DateTime.Parse(context?.User.FindFirst(c => c.Type == "DateOfBirth").Value);

                _logger.LogInformation($"Handling adult requirement of: {userEmail}. [dateOfBirth: {dateOfBirth}]");

                if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
                {
                    _logger.LogInformation("Access granted");
                    context.Succeed(requirement);
                }
                else
                {
                    _logger.LogInformation("Access denied");
                    context.Fail(new AuthorizationFailureReason(null, "Access denied."));
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation("Access denied");
                context.Fail(new AuthorizationFailureReason(null, ex.Message));
            }

            return Task.CompletedTask;
        }
    }
}
