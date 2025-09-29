using Microsoft.AspNetCore.Mvc;
using PortailRH.API.Models.DataBase;

namespace PortailRH.API.Features.Equipes.DeleteEquipe
{
    // ✅ Command
    public record DeleteEquipeCommand(Guid EquipeId) : IRequest<DeleteEquipeResult>;

    // ✅ Result
    public record DeleteEquipeResult(bool Success, string Message);

    // ✅ Handler
    public class DeleteEquipeCommandHandler : IRequestHandler<DeleteEquipeCommand, DeleteEquipeResult>
    {
        private readonly IEquipeRepository _equipeRepository;

        public DeleteEquipeCommandHandler(IEquipeRepository equipeRepository)
        {
            _equipeRepository = equipeRepository;
        }

        public async Task<DeleteEquipeResult> Handle(DeleteEquipeCommand command, CancellationToken cancellationToken)
        {
            var equipe = await _equipeRepository.GetByIdAsync(command.EquipeId);

            if (equipe == null)
                return new DeleteEquipeResult(false, "Équipe introuvable");

            await _equipeRepository.DeleteAsync(equipe);

            return new DeleteEquipeResult(true, "Équipe supprimée avec succès");
        }
    }

    // ✅ Endpoint
    public class DeleteEquipeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/equipes/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteEquipeCommand(id));

                if (!result.Success)
                    return Results.NotFound(result);

                return Results.Ok(result);
            })
            .WithName("DeleteEquipe")
            .Produces<DeleteEquipeResult>(StatusCodes.Status200OK)
            .Produces<DeleteEquipeResult>(StatusCodes.Status404NotFound);
        }
    }
}
