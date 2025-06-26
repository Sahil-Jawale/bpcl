using Microsoft.AspNetCore.Identity;

namespace GRCAuthServicesMVC.Repositories
{
    public interface ITokenRepo
    {

       string CreateJWTToken(IdentityUser user, List<string> roles);

    }
}
