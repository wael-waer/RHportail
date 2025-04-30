namespace PortailRH.API.Features.Candidatures.DeleteCandidature
{

    public record DeleteCandidatureCommand(int Id) : ICommand<DeleteCandidatureResult>;

    public record DeleteCandidatureResult(bool IsSuccess);

    public class DeleteCandidatureCommandHandler(ICandidatureRepository repository)
        : ICommandHandler<DeleteCandidatureCommand, DeleteCandidatureResult>
    {
        public async Task<DeleteCandidatureResult> Handle(DeleteCandidatureCommand command, CancellationToken cancellationToken)
        {
            var candidature = await repository.GetByIdAsync(command.Id);
            if (candidature is null)
            {
                throw new NotFoundException("candidature", command.Id);
            }

            await repository.DeleteAsync(candidature);
            return new DeleteCandidatureResult(true);
        }
    }
}
