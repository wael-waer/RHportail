
namespace PortailRH.API.Features.Employees.DeleteEmployee
{
    public record DeleteEmployeeCommand(int Id) : ICommand<DeleteEmployeeResult>;

    public record DeleteEmployeeResult(bool IsSuccess);
    public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
    {
        public DeleteEmployeeCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than zero");
        }
    }


    public class DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        : ICommandHandler<DeleteEmployeeCommand, DeleteEmployeeResult>
    {
        public async Task<DeleteEmployeeResult> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
           
            var employee = await employeeRepository.GetByIdAsync(command.Id);

            if (employee == null)
            {
                
                return new DeleteEmployeeResult(false);
            }

           
            await employeeRepository.DeleteAsync(employee);

            return new DeleteEmployeeResult(true);
        }
    }
}
