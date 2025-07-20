namespace PortailRH.API.Features.Admins.Login
{
    public record LoginAdminRequest(string Email, string Password);
    public record LoginAdminResponse(bool IsSuccess, string? Message , string? Role);

    public class LoginAdminEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/admin/login", async (LoginAdminRequest request, ISender sender) =>
            {
                var command = request.Adapt<LoginAdminCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<LoginAdminResponse>();

                if (!result.IsSuccess)
                    return Results.Unauthorized();

                return Results.Ok(response);
            })
            .WithName("LoginAdmin")
            .Produces<LoginAdminResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary("Login Admin")
            .WithDescription("Authenticates an admin user");
        }
    }
}
