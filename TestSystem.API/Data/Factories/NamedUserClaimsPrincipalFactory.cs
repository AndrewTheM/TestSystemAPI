using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using TestSystem.API.Core.Entities;

namespace TestSystem.API.Extensions
{
    public class NamedUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
    {
        public NamedUserClaimsPrincipalFactory(UserManager<User> userManager,
                                               RoleManager<IdentityRole> roleManager,
                                               IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);
            var identity = principal.Identity as ClaimsIdentity;

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
            }

            return principal;
        }
    }
}
