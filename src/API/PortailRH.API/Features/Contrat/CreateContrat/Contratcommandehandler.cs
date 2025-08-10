namespace PortailRH.API.Features.Contrats.CreateContrat
{
    public record CreateContratCommand(
       
        DateTime DateDebut,
        DateTime? DateFin,
       
        int EmployeeId
    ) : ICommand<CreateContratResult>;

    public record CreateContratResult(int Id);

    public class CreateContratCommandValidator : AbstractValidator<CreateContratCommand>
    {
        public CreateContratCommandValidator()
        {
           
            RuleFor(x => x.DateDebut).LessThanOrEqualTo(DateTime.Today);
          
            RuleFor(x => x.EmployeeId).GreaterThan(0);
        }
    }

    public class CreateContratCommandHandler : ICommandHandler<CreateContratCommand, CreateContratResult>
    {
        private readonly PortailRHContext _context;

        public CreateContratCommandHandler(PortailRHContext context)
        {
            _context = context;
        }

        public async Task<CreateContratResult> Handle(CreateContratCommand request, CancellationToken cancellationToken)
        {
            // Vérifie que l'employé existe
            var employee = await _context.Employee
                .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);

            if (employee == null)
            {
                throw new Exception($"Aucun employé trouvé avec l'ID {request.EmployeeId}");
            }

            var contrat = new Contrat
            {
               
               
                DateDebut = request.DateDebut,
                DateFin = request.DateFin,
                
                EmployeeId = request.EmployeeId,
                Employee = employee
            };

            _context.Contrats.Add(contrat);
            await _context.SaveChangesAsync(cancellationToken);


            return new CreateContratResult(contrat.Id);
        }
    }
}
