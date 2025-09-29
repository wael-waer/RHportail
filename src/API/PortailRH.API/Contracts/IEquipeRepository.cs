

namespace PortailRH.API.Contracts
{
    public interface IEquipeRepository : IAsyncRepository<Equipe>
    {
        // Méthodes spécifiques pour Equipe
        Task<Equipe?> GetByIdAsync(Guid id);
        Task<Equipe?> GetByIdWithEmployeesAsync(Guid id);
        Task<IEnumerable<Equipe>> GetAllWithEmployeesAsync();
        Task<bool> ExistsByNameAsync(string nom);
        Task<bool> ExistsAsync(Guid id);

        // Vérifier si un employé (int Id) est dans l'équipe (Guid Id)
        Task<bool> EmployeeIsInEquipeAsync(int employeeId, Guid equipeId);

        // Méthodes pour la création/mise à jour d'équipe avec affectation
        Task AssignEmployeesToEquipeAsync(Guid equipeId, IEnumerable<int> employeeIds);
        Task<Equipe> CreateEquipeWithEmployeesAsync(Equipe equipe, IEnumerable<int> employeeIds);
        Task UpdateEquipeWithEmployeesAsync(Equipe equipe, IEnumerable<int> employeeIds);
    }
}
