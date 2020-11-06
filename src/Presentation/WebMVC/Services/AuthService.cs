using System.Security.Principal;
using System.Linq;
using System.Security.Claims;
using WebMVC.Models;
using System;

namespace WebMVC.Services
{
    public class AuthService : IAuthService<ApplicationUser>
    {
        public ApplicationUser Get(IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claims)
            {
                var user = new ApplicationUser()
                {
                    Email = claims.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value ?? "",
                    Id = claims.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ?? ""
                };

                return user;
            }

            throw new ArgumentException(message: "the principal must be a claimsprincipal", paramName: nameof(principal));
        }
    }
}