namespace PortailRH.API.Features.Employees.UpdateEmployee
{
    public record UpdateEmployeeCommand(int Id, string LastName, string FirstName, string Email, DateTime BirthDate, string Poste, decimal Salary)
        : ICommand<UpdateEmployeeResult>;

    public record UpdateEmployeeResult(bool IsSuccess);

    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
          
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.BirthDate).LessThan(DateTime.Now).WithMessage("BirthDate must be in the past");
            RuleFor(x => x.Poste).NotEmpty().WithMessage("Poste is required");
        }
    }

    public class UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        : ICommandHandler<UpdateEmployeeCommand, UpdateEmployeeResult>
    {
        public async Task<UpdateEmployeeResult> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(command.Id);
            if (employee is null)
            {
                throw new NotFoundException("employee ", command.Id );
            }
            employee.LastName = command.LastName;

            employee.FirstName = command.FirstName;
            employee.Email = command.Email;
            employee.BirthDate = command.BirthDate;
            employee.Poste = command.Poste;

            await employeeRepository.UpdateAsync(employee);
            return new UpdateEmployeeResult(true);
        }
    }
}
