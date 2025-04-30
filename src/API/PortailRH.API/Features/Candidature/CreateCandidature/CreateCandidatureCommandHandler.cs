namespace PortailRH.API.Features.Candidatures.CreateCandidature
{
    public record CreateCandidatureCommand(string Nom, string Prenom, string Email, string PosteSouhaite)
        : ICommand<CreateCandidatureResult>;

    public record CreateCandidatureResult(int Id);

    public class CreateCandidatureCommandValidator : AbstractValidator<CreateCandidatureCommand>
    {
        public CreateCandidatureCommandValidator()
        {
            RuleFor(x => x.Nom).NotEmpty();
            RuleFor(x => x.Prenom).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PosteSouhaite).NotEmpty();
            
        }
    }

    public class CreateCandidatureCommandHandler(ICandidatureRepository candidatureRepository)
        : ICommandHandler<CreateCandidatureCommand, CreateCandidatureResult>
    {
        public async Task<CreateCandidatureResult> Handle(CreateCandidatureCommand command, CancellationToken cancellationToken)
        {
            var candidature = command.Adapt<Candidature>();
            var result = await candidatureRepository.AddAsync(candidature);
            return new CreateCandidatureResult(result.Id);
        }
    }
}
