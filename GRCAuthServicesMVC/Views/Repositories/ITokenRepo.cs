using Microsoft.AspNetCore.Identity;

namespace GRCAuthServicesMVC.Views.Repositories
{
    public interface ITokenRepo
    {

       string CreateJWTToken(IdentityUser user, List<string> roles);

    }
}
