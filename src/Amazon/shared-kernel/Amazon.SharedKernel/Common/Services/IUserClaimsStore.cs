using System.Security.Claims;

namespace Amazon.SharedKernel.Common.Services;

public interface IUserClaimsStore
{
    Task<List<Claim>> GetClaimsAsync(Guid userId, params string[] keys);
    Task SaveClaimsAsync(Guid userId, params Claim[] claims);
    Task RemoveClaimsAsync(Guid userId, params string[] keys);
}