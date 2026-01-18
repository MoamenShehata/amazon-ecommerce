using System.Security.Claims;
using Amazon.Identity.Presentation.Models;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;

namespace Amazon.Identity.Presentation.Profiles;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var claims = await GetUserCustomCLaimsAsync(context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value);

        context.IssuedClaims.AddRange(claims);
    }

    private async Task<List<Claim>> GetUserCustomCLaimsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var claims = new List<Claim>();

        claims.Add(new Claim("name", user.UserName));
        claims.Add(new Claim("email", user.Email));

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        return claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        context.IsActive = user != null;
    }
}
