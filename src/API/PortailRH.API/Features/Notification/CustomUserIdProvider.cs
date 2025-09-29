using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace PortailRH.API.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // ✅ Option 1: Cherchez d'abord EmployeeId
            var employeeId = connection.User?.FindFirst("EmployeeId")?.Value;

            // ✅ Option 2: Si non trouvé, cherchez NameIdentifier
            if (string.IsNullOrEmpty(employeeId))
            {
                employeeId = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            // ✅ Option 3: Si toujours non trouvé, cherchez le claim complet
            if (string.IsNullOrEmpty(employeeId))
            {
                employeeId = connection.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            }

            // 🔍 LOGS DE DÉBOGAGE CRUCIAUX
            Console.WriteLine($"🔍 CustomUserIdProvider returning: {employeeId}");

            if (connection.User != null)
            {
                Console.WriteLine("🔍 Available claims:");
                foreach (var claim in connection.User.Claims)
                {
                    Console.WriteLine($"   {claim.Type} = {claim.Value}");
                }
            }

            return employeeId;
        }
    }
}