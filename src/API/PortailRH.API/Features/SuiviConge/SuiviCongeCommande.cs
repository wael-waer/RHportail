namespace PortailRH.API.Features.SuiviConges
{
    public record CreateOrUpdateSuiviCongeCommand(int EmployeeId, int Annee, int SoldeInitial)
        : ICommand<CreateOrUpdateSuiviCongeResult>;

    public record CreateOrUpdateSuiviCongeResult(bool IsSuccess);

    public class CreateOrUpdateSuiviCongeCommandHandler(
        IEmployeeRepository employeeRepository
    ) : ICommandHandler<CreateOrUpdateSuiviCongeCommand, CreateOrUpdateSuiviCongeResult>
    {
        public async Task<CreateOrUpdateSuiviCongeResult> Handle(CreateOrUpdateSuiviCongeCommand command, CancellationToken cancellationToken)
        {
            // Vérifie que l'employé existe
            var employee = await employeeRepository.GetByIdAsync(command.EmployeeId);
            if (employee is null)
                throw new Exception("Employé introuvable.");

            // Récupère tous les suivis de congé de cet employé
            var allSuivis = await employeeRepository.GetAllSuivisCongeByEmployeeIdAsync(command.EmployeeId);

            // Vérifie s’il existe déjà un suivi pour l’année demandée
            var existingForYear = allSuivis.FirstOrDefault(s => s.Annee == command.Annee);

            // Met tous les autres suivis en non-actifs (archivés)
            foreach (var suivi in allSuivis)
            {
                if (suivi.Annee != command.Annee && suivi.Actif)
                {
                    suivi.Actif = false;
                    await employeeRepository.UpdateSuiviCongeAsync(suivi);
                }
            }

            // Crée un nouveau suivi ou met à jour celui existant
            if (existingForYear == null)
            {
                var newSuivi = new SuiviConge
                {
                    EmployeeId = command.EmployeeId,
                    Annee = command.Annee,
                    Actif = true,
                    SoldeInitial = command.SoldeInitial,
                    SoldeRestant = command.SoldeInitial
                };

                await employeeRepository.AddSuiviCongeAsync(newSuivi);
            }
            else
            {
                existingForYear.SoldeInitial = command.SoldeInitial;
                existingForYear.SoldeRestant = command.SoldeInitial;
                existingForYear.Actif = true;

                await employeeRepository.UpdateSuiviCongeAsync(existingForYear);
            }

            return new CreateOrUpdateSuiviCongeResult(true);
        }
    }
}
