using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TAs.Domain.Entities;

namespace TAs.Infrastructure.Authorization
{
    public class TAsUserClaimPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole<Guid>>
    {
        public TAsUserClaimPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {

        }
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var id = await GenerateClaimsAsync(user);
            if (user.Region != null)
            {
                id.AddClaim(new Claim("Region", user.Region));
            }
            if (user.DateOfBirth is not null)
            {
                id.AddClaim(new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyy-MM-dd")));
            }
            return new ClaimsPrincipal(id);
        }

    }
}