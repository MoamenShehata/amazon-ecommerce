using Amazon.Cart.Infrastructure.Data;
using Amazon.Cart.Infrastructure.Data.Models;
using Amazon.SharedKernel.Common.Services;
using Microsoft.EntityFrameworkCore;
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
        _context.UserClaims.AddRange(claims.Select(c => new CustomerClaim
        {
            CustomerId = userId,
            Key = c.Type,
            Value = c.Value
        }));

        await _context.SaveChangesAsync();
    }
}