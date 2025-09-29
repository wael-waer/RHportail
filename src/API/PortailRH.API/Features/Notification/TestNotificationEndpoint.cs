using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PortailRH.API.SignalR;

namespace PortailRH.API.Features.Notifications
{
    public class TestNotificationEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/test/test-notification", async ([FromServices] IHubContext<NotificationsHub> hubContext) =>
            {
                var employeeId = "4021"; // 🔴 Replace with a real EmployeeId from your DB

                await hubContext.Clients.User(employeeId)
                    .SendAsync("ReceiveNotification", "Notification test SignalR !");

                return Results.Ok("Notification envoyée avec succès 🚀");
            });
        }
    }
}
