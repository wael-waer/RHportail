using Microsoft.AspNetCore.SignalR;
using PortailRH.API.SignalR;

public interface INotificationService
{
    Task SendNotificationToUserAsync(string userId, string message);
    Task SendNotificationToAllAsync(string message);
}

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationsHub> _hubContext;

    public NotificationService(IHubContext<NotificationsHub> hubContext)
    {
        _hubContext = hubContext;
    }

    // Envoie à un utilisateur spécifique (identifié par son userId SignalR)
    public async Task SendNotificationToUserAsync(string userId, string message)
    {
        await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
    }

    // Envoie à tous les clients connectés
    public async Task SendNotificationToAllAsync(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
    }
}
