

using PortailRH.API.Features.Contrats.CreateContrat;

namespace PortailRH.API.Features.Contrats
{

    public static class ContratsEndpoints
    {
        public static IEndpointRouteBuilder MapContratsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/contrats").WithTags("Contrats");

            group.MapPost("/", async (CreateContratCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Created($"/api/contrats/{result.Id}", result);
            });


            return app;
        }
    }
}
