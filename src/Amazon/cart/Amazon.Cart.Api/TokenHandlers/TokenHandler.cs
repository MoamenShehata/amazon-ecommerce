using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Amazon.Cart.Api.TokenHandlers;

public sealed class CartTokenHandler : TokenHandler
{
    private readonly JwtSecurityTokenHandler _innerHandler = new();

    public override async Task<TokenValidationResult> ValidateTokenAsync(
        string token,
        TokenValidationParameters validationParameters)
    {
        // Validate using built-in JWT handler
        var result = await _innerHandler.ValidateTokenAsync(token, validationParameters);

        //if (!result.IsValid)
        //{
        //    return result;
        //}

        // OPTIONAL: custom logic after validation
        var principal = result.ClaimsIdentity != null
            ? new System.Security.Claims.ClaimsPrincipal(result.ClaimsIdentity)
            : null;

        return new TokenValidationResult
        {
            IsValid = true,
            SecurityToken = result.SecurityToken,
            ClaimsIdentity = result.ClaimsIdentity,
        };
    }

}
