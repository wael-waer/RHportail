
namespace PortailRH.API.Features.Candidatures.CreateCandidature
{
    public record CreateCandidatureCommand(
     string Nom,
     string Prenom,
     string Email,
     string PosteSouhaite,
     string CVUrl
     
 ) : ICommand<CreateCandidatureResult>;


    public record CreateCandidatureResult(int Id);

    public class CreateCandidatureCommandValidator : AbstractValidator<CreateCandidatureCommand>
    {
        public CreateCandidatureCommandValidator()
        {
            RuleFor(x => x.Nom).NotEmpty();
            RuleFor(x => x.Prenom).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PosteSouhaite).NotEmpty();
            RuleFor(x => x.CVUrl).NotEmpty().WithMessage("Le CV est requis");
           
        }
    }


    public class CreateCandidatureCommandHandler(ICandidatureRepository candidatureRepository)
        : ICommandHandler<CreateCandidatureCommand, CreateCandidatureResult>
    {
        public async Task<CreateCandidatureResult> Handle(CreateCandidatureCommand command, CancellationToken cancellationToken)
        {
            var candidature = new Candidature
            {
                Nom = command.Nom,
                Prenom = command.Prenom,
                Email = command.Email,
                PosteSouhaite = command.PosteSouhaite,
                CVUrl = command.CVUrl
                
            };

            var result = await candidatureRepository.AddAsync(candidature);
            return new CreateCandidatureResult(result.Id);
        }
    }
}
