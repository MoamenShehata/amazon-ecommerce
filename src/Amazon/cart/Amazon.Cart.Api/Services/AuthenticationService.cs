using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Services;
using System.Security.Claims;

namespace Amazon.Cart.Api.Services;

public class AuthenticationService(IHttpContextAccessor _httpContextAccessor) : IAuthenticationService
{
    private static CurrentUser _anonymouseInstance = new AnonymousUser();
    public CurrentUser CurrentUser
    {
        get
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            if (user is null || !user.Identity.IsAuthenticated) return _anonymouseInstance;

            return new AuthenticatedUser(Guid.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value), user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
        }
    }

}
