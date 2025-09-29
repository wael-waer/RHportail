using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PortailRH.API.Models.DataBase;
using PortailRH.API.SignalR;
using System.Text.Json;

public interface INotificationService
{
    Task SendNotificationToUserAsync(string employeeId, string message, string type = "info");
    Task SendNotificationToAllAsync(string message, string type = "info");
}

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IHubContext<NotificationsHub> hubContext, ILogger<NotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    // Envoie à un utilisateur spécifique
    public async Task SendNotificationToUserAsync(string employeeId, string message, string type = "info")
    {
        try
        {
            _logger.LogInformation("SignalR: Envoi de notification à l'utilisateur {EmployeeId} : {Message}", employeeId, message);

            // Créer un objet de notification structuré
            var notificationObject = new
            {
                userId = employeeId,
                message = message,
                type = type,
                timestamp = DateTime.UtcNow
            };

            // Sérialiser en JSON
            var jsonNotification = JsonSerializer.Serialize(notificationObject);

            await _hubContext.Clients.User(employeeId)
                  .SendAsync("ReceiveNotification", notificationObject);

            _logger.LogInformation("SignalR: Notification envoyée avec succès à {EmployeeId}", employeeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'envoi de notification à l'utilisateur {EmployeeId}", employeeId);
        }
    }

    public async Task SendNotificationToAllAsync(string message, string type = "info")
    {
        try
        {
            _logger.LogInformation("SignalR: Envoi de notification à tous les utilisateurs : {Message}", message);

            var notificationObject = new
            {
                message = message,
                type = type,
                timestamp = DateTime.UtcNow
            };

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationObject);

            _logger.LogInformation("SignalR: Notification envoyée avec succès à tous les utilisateurs");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'envoi de notification à tous les utilisateurs");
        }
    }
}