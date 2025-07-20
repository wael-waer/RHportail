

namespace PortailRH.API.Features.Employees.LoginEmployee
{

    public class LoginEmployeeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/employes/login", async (
                LoginEmployeeCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);
                if (result == null)
                {
                    return Results.Json(new { message = "Matricule ou mot de passe incorrect." }, statusCode: 401);

                }
                return TypedResults.Ok(result);
            })
            .WithName("LoginEmployee")
            .Produces<LoginEmployeeResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }

}
