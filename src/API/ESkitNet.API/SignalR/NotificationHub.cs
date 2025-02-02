using ESkitNet.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ESkitNet.API.SignalR;

[Authorize]
public class NotificationHub(ILogger<NotificationHub> logger) : Hub
{
    private static readonly ConcurrentDictionary<string, string> UserConnections = new();

    public override Task OnConnectedAsync()
    {
        var email = Context.User?.GetEmail(false);

        logger.LogWarning("OnConnectedAsync: Email = '{Email}', ConnectionId = {ConnectionId}", email, Context.ConnectionId);

        if (!string.IsNullOrWhiteSpace(email))
            UserConnections[email] = Context.ConnectionId;

        logger.LogWarning("UserConnections['{Email}'] = {ConnectionId}", email, UserConnections[email!]);

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var email = Context.User?.GetEmail(false);

        logger.LogWarning("OnDisconnectedAsync: {Email}", email);

        if (!string.IsNullOrWhiteSpace(email))
            UserConnections.TryRemove(email, out var _);

        return base.OnDisconnectedAsync(exception);
    }

    public static string? GetConnectionStringByEmail(string email)
    {
        UserConnections.TryGetValue(email, out var connectionId);

        return connectionId;
    }
}
