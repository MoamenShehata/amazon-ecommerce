using Amazon.Cart.Infrastructure.Data;
using Amazon.Cart.Infrastructure.Data.Models;
using Amazon.SharedKernel.Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace Amazon.Cart.Infrastructure.Services;

public class CartUserClaimsStore(ShoppingCartContext _context) : IUserClaimsStore
{
    public async Task<List<Claim>> GetClaimsAsync(Guid userId, params string[] keys)
    {
        return await _context.UserClaims
            .AsNoTracking()
            .Where(uc => uc.CustomerId == userId && keys.Contains(uc.Key))
             .Select(uc => new Claim(uc.Key, uc.Value))
             .ToListAsync();
    }

    public async Task RemoveClaimsAsync(Guid userId, params string[] keys)
    {
        await _context.UserClaims
            .Where(uc => uc.CustomerId == userId && keys.Contains(uc.Key))
            .ExecuteDeleteAsync();
    }

    public async Task SaveClaimsAsync(Guid userId, params Claim[] claims)
    {
        var existingClaims = await
            _context.UserClaims
            .Where(uc => uc.CustomerId == userId && claims.Select(c => c.Type).Contains(uc.Key))
            .ToListAsync();

        foreach (var claimToSave in claims)
        {
            var existingClaim = existingClaims.FirstOrDefault(ec => ec.Key == claimToSave.Type);
            if (existingClaim is null)
                _context.UserClaims.Add(new CustomerClaim
                {
                    CustomerId = userId,
                    Key = claimToSave.Type,
                    Value = claimToSave.Value
                });
            else
                existingClaim.Value = claimToSave.Value;
        }


        await _context.SaveChangesAsync();
    }
}