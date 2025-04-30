namespace PortailRH.API.Features.Employees.CreateEmployee
{
    public record CreateEmployeeCommand(string LastName, string FirstName, string Email, DateTime BirthDate, string Poste)
        : ICommand<CreateEmployeeResult>;
    public record CreateEmployeeResult(int Id);

    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.BirthDate).LessThan(DateTime.Now).WithMessage("BirthDate must be in the past");
                
        }
    }
    public class CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        : ICommandHandler<CreateEmployeeCommand, CreateEmployeeResult>
    {
        public async Task<CreateEmployeeResult> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employeeToCreate = command.Adapt<Employee>();
            var Employee = await employeeRepository.AddAsync(employeeToCreate);
            return new CreateEmployeeResult(Employee.Id);
        }
    }
}
