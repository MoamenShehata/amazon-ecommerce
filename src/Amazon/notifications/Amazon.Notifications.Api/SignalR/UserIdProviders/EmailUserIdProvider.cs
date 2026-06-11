using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Amazon.Notifications.Api.SignalR.UserIdProviders;

public class EmailUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        // Example: Use a claim like "sub" or "id"
        return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
    }
}