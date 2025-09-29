using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PortailRH.API.SignalR
{
   // [Authorize]
    public class NotificationsHub : Hub
    {
        private readonly ILogger<NotificationsHub> _logger;

        public NotificationsHub(ILogger<NotificationsHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation("SignalR: Client connected - ConnectionId: {ConnectionId}, UserId: {UserId}",
                Context.ConnectionId, userId);

            if (!string.IsNullOrEmpty(userId))
            {
                // Associer l'userId à la connexion
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
                _logger.LogInformation("SignalR: User {UserId} added to group user_{UserId}", userId, userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation("SignalR: Client disconnected - ConnectionId: {ConnectionId}, UserId: {UserId}",
                Context.ConnectionId, userId);

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Méthode pour s'abonner aux notifications d'un utilisateur spécifique
        public async Task SubscribeToUser(string userId)
        {
            if (Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value == userId)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
                _logger.LogInformation("SignalR: User {UserId} subscribed to their notifications", userId);
            }
            else
            {
                _logger.LogWarning("SignalR: Unauthorized subscription attempt - User {UserId} tried to subscribe to {TargetUserId}",
                    Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, userId);
            }
        }

        // Méthode pour se désabonner
        public async Task UnsubscribeFromUser(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
            _logger.LogInformation("SignalR: User {UserId} unsubscribed from notifications", userId);
        }
    }
}