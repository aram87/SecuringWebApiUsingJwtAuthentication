using Microsoft.AspNetCore.Authorization;
using SecuringWebApiUsingJwtAuthentication.Helpers;
using SecuringWebApiUsingJwtAuthentication.Requirements;
using System;
using System.Threading.Tasks;

namespace SecuringWebApiUsingJwtAuthentication.Handlers
{
    public class CustomerBlockedStatusHandler : AuthorizationHandler<CustomerBlockedStatusRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomerBlockedStatusRequirement requirement)
        {
            var claim = context.User.FindFirst(c => c.Type == "IsBlocked" && c.Issuer == TokenHelper.Issuer);
            if (!context.User.HasClaim(c => c.Type == "IsBlocked" && c.Issuer == TokenHelper.Issuer))
            {
                return Task.CompletedTask;
            }

            string value = context.User.FindFirst(c => c.Type == "IsBlocked" && c.Issuer == TokenHelper.Issuer).Value;
            var customerBlockedStatus = Convert.ToBoolean(value);

            if (customerBlockedStatus == requirement.IsBlocked)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
