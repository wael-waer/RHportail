using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace PortailRH.API.Features.Equipes.UpdateEquipe
{
    public record UpdateEquipeCommand(
        Guid EquipeId,
        string Nom,
        string? Description,
        List<int> EmployeeIds,
        int ResponsableId
    ) : IRequest<UpdateEquipeResult>;

    public record UpdateEquipeResult(bool Success, string Message);

    // ✅ Validator
    public class UpdateEquipeCommandValidator : AbstractValidator<UpdateEquipeCommand>
    {
        public UpdateEquipeCommandValidator()
        {
            RuleFor(x => x.EquipeId)
                .NotEmpty().WithMessage("L'ID de l'équipe est requis");

            RuleFor(x => x.Nom)
                .NotEmpty().WithMessage("Le nom de l'équipe est requis")
                .MaximumLength(100).WithMessage("Le nom ne peut pas dépasser 100 caractères");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La description ne peut pas dépasser 500 caractères");

            RuleFor(x => x.EmployeeIds)
                .NotNull().WithMessage("La liste des employés est obligatoire")
                .Must(ids => ids != null && ids.Any()).WithMessage("Au moins un employé doit être sélectionné")
                .Must(HaveUniqueIds).WithMessage("Les IDs des employés doivent être uniques");

            RuleFor(x => x.ResponsableId)
                .GreaterThan(0).WithMessage("L'ID du responsable est requis");

            RuleForEach(x => x.EmployeeIds)
                .GreaterThan(0).WithMessage("L'ID de l'employé doit être positif");
        }

        private bool HaveUniqueIds(List<int> employeeIds)
            => employeeIds.Distinct().Count() == employeeIds.Count;
    }

    // ✅ Handler
    public class UpdateEquipeCommandHandler : IRequestHandler<UpdateEquipeCommand, UpdateEquipeResult>
    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public UpdateEquipeCommandHandler(IEquipeRepository equipeRepository, IEmployeeRepository employeeRepository)
        {
            _equipeRepository = equipeRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<UpdateEquipeResult> Handle(UpdateEquipeCommand command, CancellationToken cancellationToken)
        {
            var equipe = await _equipeRepository.GetByIdAsync(command.EquipeId);
            if (equipe == null)
                return new UpdateEquipeResult(false, "Équipe introuvable");

            // Vérification des employés
            foreach (var empId in command.EmployeeIds)
            {
                if (await _employeeRepository.GetByIdAsync(empId) == null)
                    return new UpdateEquipeResult(false, $"Employé {empId} inexistant");
            }

            // Mise à jour des données
            equipe.Nom = command.Nom;
            equipe.Description = command.Description;
            equipe.ResponsableId = command.ResponsableId;

            await _equipeRepository.UpdateEquipeWithEmployeesAsync(equipe, command.EmployeeIds);

            return new UpdateEquipeResult(true, "Équipe mise à jour avec succès");
        }
    }

    // ✅ Endpoint
    public class UpdateEquipeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/equipes/{equipeId:guid}", async (
                [FromRoute] Guid equipeId,
                [FromBody] UpdateEquipeCommand command,
                ISender sender) =>
            {
                if (equipeId != command.EquipeId)
                    return Results.BadRequest(new { Message = "Conflit entre ID de route et ID de commande" });

                var result = await sender.Send(command);
                return result.Success
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            })
            .WithName("UpdateEquipe")
            .Produces<UpdateEquipeResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
