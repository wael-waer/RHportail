namespace PortailRH.API.Features.Employees.GetEmployeeById
{
    public record GetEmployeeByIdQuery(int Id) : IQuery<GetEmployeeByIdResult>;

    public record GetEmployeeByIdResult(int Id, string LastName, string FirstName, string Email, DateTime BirthDate, string Poste , decimal Salary);

    public class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
        : IQueryHandler<GetEmployeeByIdQuery, GetEmployeeByIdResult>
    {
        public async Task<GetEmployeeByIdResult> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(query.Id);
            if (employee is null)
            {
                throw new NotFoundException("employee ", query.Id);
            }
            var employeToReturn= employee.Adapt<GetEmployeeByIdResult>();

            return employeToReturn;
        }
    }
}
