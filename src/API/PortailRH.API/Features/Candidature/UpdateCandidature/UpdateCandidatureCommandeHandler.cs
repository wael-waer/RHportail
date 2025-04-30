namespace PortailRH.API.Features.Candidatures.UpdateCandidature
{

    public record UpdateCandidatureCommand(int Id, string Nom, string Prenom, string Email, string PosteSouhaite)
        : ICommand<UpdateCandidatureResult>;

    public record UpdateCandidatureResult(bool IsSuccess);

    public class UpdateCandidatureCommandValidator : AbstractValidator<UpdateCandidatureCommand>
    {
        public UpdateCandidatureCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.Nom).NotEmpty().WithMessage("Nom is required");
            RuleFor(x => x.Prenom).NotEmpty().WithMessage("Prenom is required");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is invalid");
            RuleFor(x => x.PosteSouhaite).NotEmpty().WithMessage("PosteSouhaite is required");
        }
    }

    public class UpdateCandidatureCommandHandler(ICandidatureRepository repository)
        : ICommandHandler<UpdateCandidatureCommand, UpdateCandidatureResult>
    {
        public async Task<UpdateCandidatureResult> Handle(UpdateCandidatureCommand command, CancellationToken cancellationToken)
        {
            var candidature = await repository.GetByIdAsync(command.Id);
            if (candidature is null)
            {
                throw new NotFoundException("candidature", command.Id);
            }

            candidature.Nom = command.Nom;
            candidature.Prenom = command.Prenom;
            candidature.Email = command.Email;
            candidature.PosteSouhaite = command.PosteSouhaite;

            await repository.UpdateAsync(candidature);
            return new UpdateCandidatureResult(true);
        }
    }
}
