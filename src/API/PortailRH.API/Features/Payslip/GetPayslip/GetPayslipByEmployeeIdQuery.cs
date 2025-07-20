namespace PortailRH.API.Features.Payslips.GetAllPayslips
{
    public record GetAllPayslipsQuery() : IRequest<List<GetAllPayslipsResult>>;

    public record GetAllPayslipsResult(
        int EmployeeId,
        string FullName,
        string Email,
        string Poste,
        int Month,
        int Year,
        decimal BasicSalary,
        decimal TaxDeduction,
        decimal SocialSecurityDeduction,
        decimal OtherDeductions,
        decimal NetSalary,
        string NetSalaryInWords
    );

    public class GetAllPayslipsHandler : IRequestHandler<GetAllPayslipsQuery, List<GetAllPayslipsResult>>
    {
        private readonly IPayslipRepository _repository;

        public GetAllPayslipsHandler(IPayslipRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetAllPayslipsResult>> Handle(GetAllPayslipsQuery request, CancellationToken cancellationToken)
        {
            var payslips = await _repository.GetAllAsync();

            return payslips.Select(p => new GetAllPayslipsResult(
                p.EmployeeId,
                $"{p.Employee.Prenom} {p.Employee.Nom}",
                p.Employee.Email,
                p.Employee.Fonction,
                p.GeneratedAt.Month,
                p.GeneratedAt.Year,
                p.BasicSalary,
                p.TaxDeduction,
                p.SocialSecurityDeduction,
                p.OtherDeductions,
                p.NetSalary,
                $"{p.NetSalary} euros"
            )).ToList();
        }
    }
}
