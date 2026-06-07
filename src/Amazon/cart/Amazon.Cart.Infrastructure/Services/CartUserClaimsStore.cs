using Amazon.Cart.Infrastructure.Data;
using Amazon.Cart.Infrastructure.Data.Models;
using Amazon.SharedKernel.Common.Services;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using MongoDB.Bson;
using System.Security.Claims;

namespace Amazon.Cart.Infrastructure.Services;

public class CartUserClaimsStore(
    //ShoppingCartContext _context,
    IRepository<CustomerClaim, ObjectId> _repository,
    IUnitOfWork _unitOfWork) : IUserClaimsStore
{
    public async Task<List<Claim>> GetClaimsAsync(Guid userId, params string[] keys)
    {
        return (await _repository
            .GetAllAsync(c => c.CustomerId == userId && keys.Contains(c.Key), x => new Claim(x.Key, x.Value))).ToList();

        //return await _context.UserClaims
        //    .AsNoTracking()
        //    .Where(uc => uc.CustomerId == userId && keys.Contains(uc.Key))
        //     .Select(uc => new Claim(uc.Key, uc.Value))
        //     .ToListAsync();
    }

    public async Task RemoveClaimsAsync(Guid userId, params string[] keys)
    {
        var claims = await _repository
            .GetAllAsync(c => c.CustomerId == userId && keys.Contains(c.Key));

        foreach (var claim in claims)
            _repository.Remove(claim);

        //await _context.UserClaims
        //    .Where(uc => uc.CustomerId == userId && keys.Contains(uc.Key))
        //    .ExecuteDeleteAsync();
    }

    public async Task SaveClaimsAsync(Guid userId, params Claim[] claims)
    {
        var existingClaims = (await
            _repository
            .GetAllAsync(uc => uc.CustomerId == userId && claims.Select(c => c.Type).Contains(uc.Key)))
            .ToList();

        foreach (var claimToSave in claims)
        {
            var existingClaim = existingClaims.FirstOrDefault(ec => ec.Key == claimToSave.Type);
            if (existingClaim is null)
                _repository.Add(new CustomerClaim
                {
                    CustomerId = userId,
                    Key = claimToSave.Type,
                    Value = claimToSave.Value
                });
            else
                existingClaim.Value = claimToSave.Value;
        }


        await _unitOfWork.CommitAsync();
        //var existingClaims = await
        //    _context.UserClaims
        //    .Where(uc => uc.CustomerId == userId && claims.Select(c => c.Type).Contains(uc.Key))
        //    .ToListAsync();

        //foreach (var claimToSave in claims)
        //{
        //    var existingClaim = existingClaims.FirstOrDefault(ec => ec.Key == claimToSave.Type);
        //    if (existingClaim is null)
        //        _context.UserClaims.Add(new CustomerClaim
        //        {
        //            CustomerId = userId,
        //            Key = claimToSave.Type,
        //            Value = claimToSave.Value
        //        });
        //    else
        //        existingClaim.Value = claimToSave.Value;
        //}


        //await _context.SaveChangesAsync();
    }
}