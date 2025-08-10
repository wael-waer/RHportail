namespace PortailRH.API.Features.Employees.UpdateSalary
{
    public record UpdateSalaryCommand(int EmployeeId, decimal Salary) : ICommand;

    public class UpdateSalaryCommandHandler : ICommandHandler<UpdateSalaryCommand>
    {
        private readonly IEmployeeRepository _employeeRepo;
       

        public UpdateSalaryCommandHandler(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
           
        }

        public async Task<Unit> Handle(UpdateSalaryCommand command, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepo.GetByIdAsync(command.EmployeeId);
            if (employee is null)
                throw new ArgumentException("Employee not found");

          
            await _employeeRepo.UpdateAsync(employee);

            return Unit.Value;
        }
    }
}