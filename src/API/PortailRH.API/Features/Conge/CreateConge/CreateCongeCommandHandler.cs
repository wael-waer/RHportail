namespace PortailRH.API.Features.Conges.CreateConge
{
    public record CreateCongeCommand(
         string TypeConge,
         DateTime DateDebut,
         DateTime DateFin,
         string Statut,
         string Motif,
         int EmployeeId
     ) : ICommand<CreateCongeResult>;

    public record CreateCongeResult(int Id);

    public class CreateCongeCommandValidator : AbstractValidator<CreateCongeCommand>
    {
        public CreateCongeCommandValidator()
        {
            RuleFor(x => x.TypeConge).NotEmpty().WithMessage("Le type de congé est requis");
            RuleFor(x => x.DateDebut).NotEmpty().WithMessage("La date de début est requise");
            RuleFor(x => x.DateFin).NotEmpty().WithMessage("La date de fin est requise");
            RuleFor(x => x.Statut).NotEmpty().WithMessage("Le statut est requis");
            RuleFor(x => x.Motif).NotEmpty().WithMessage("Le motif est requis");
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("L'identifiant de l'employé est requis");

            RuleFor(x => x.DateFin)
                .GreaterThanOrEqualTo(x => x.DateDebut)
                .WithMessage("La date de fin doit être postérieure ou égale à la date de début");

            RuleFor(x => x.DateDebut)
                .Must(date => date >= DateTime.Today)
                .WithMessage("La date de début ne peut pas être dans le passé");

            // Check if start or end date is a public holiday
            RuleFor(x => x.DateDebut)
                .Must(date => !FrenchPublicHolidays.IsPublicHoliday(date))
                .WithMessage("La date de début ne peut pas être un jour férié");

            RuleFor(x => x.DateFin)
                .Must(date => !FrenchPublicHolidays.IsPublicHoliday(date))
                .WithMessage("La date de fin ne peut pas être un jour férié");
        }
    }

    public class CreateCongeCommandHandler : ICommandHandler<CreateCongeCommand, CreateCongeResult>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICongeRepository _congeRepository;

        public CreateCongeCommandHandler(
            IEmployeeRepository employeeRepository,
            ICongeRepository congeRepository)
        {
            _employeeRepository = employeeRepository;
            _congeRepository = congeRepository;
        }

        public async Task<CreateCongeResult> Handle(CreateCongeCommand command, CancellationToken cancellationToken)
        {
            // 1. Vérification de l'existence de l'employé
            var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId);
            if (employee is null)
                throw new Exception("Employé introuvable");

            // 2. Validation des dates
            if (command.DateFin < command.DateDebut)
                throw new Exception("La date de fin doit être postérieure ou égale à la date de début");

            // 3. Vérification des jours fériés
            var holidaysInRange = FrenchPublicHolidays.GetHolidaysInRange(
                command.DateDebut.Date,
                command.DateFin.Date
            );

            if (holidaysInRange.Any())
            {
                var holidayList = string.Join(", ",
                    holidaysInRange.Select(h => h.ToString("dd/MM/yyyy")));

                throw new Exception(
                    $"Impossible de demander des congés pendant les jours fériés. " +
                    $"Jours fériés dans la période : {holidayList}");
            }

            // 4. Calcul des jours ouvrés
            var totalDays = (command.DateFin - command.DateDebut).TotalDays + 1;
            var workingDays = Enumerable.Range(0, (int)totalDays)
                .Select(offset => command.DateDebut.AddDays(offset))
                .Count(d => d.DayOfWeek != DayOfWeek.Saturday &&
                           d.DayOfWeek != DayOfWeek.Sunday);

            if (workingDays <= 0)
            {
                throw new Exception("Aucun jour ouvrable dans la période sélectionnée");
            }

            // 5. Vérification du solde de congé
            var anneeConge = command.DateDebut.Year;
            var suiviConge = await _employeeRepository.GetSuiviCongeAsync(command.EmployeeId, anneeConge);

            if (suiviConge == null || !suiviConge.Actif)
                throw new Exception("Aucun suivi de congé actif trouvé pour cette année.");

            // Utilisation des jours ouvrés pour le calcul du solde
            if (suiviConge.SoldeRestant < workingDays)
                throw new Exception("Solde de congé insuffisant.");

            // 6. Création de la demande de congé
            var conge = new Conge
            {
                TypeConge = command.TypeConge,
                DateDebut = command.DateDebut,
                DateFin = command.DateFin,
                Statut = command.Statut,
                Motif = command.Motif,
                EmployeeId = command.EmployeeId
            };

            await _congeRepository.AddAsync(conge);

            // 7. Déduction du solde si la demande est approuvée
            if (command.Statut == "Approved")
            {
                suiviConge.SoldeRestant -= workingDays;
                await _employeeRepository.UpdateSuiviCongeAsync(suiviConge);
            }

            return new CreateCongeResult(conge.Id);
        }
    }

    // Classe utilitaire pour gérer les jours fériés en France
    public static class FrenchPublicHolidays
    {
        public static List<DateTime> GetHolidays(int year)
        {
            var holidays = new List<DateTime>
            {
                // Jours fériés à date fixe
                new DateTime(year, 1, 1),   // Jour de l'An
                new DateTime(year, 5, 1),   // Fête du Travail
                new DateTime(year, 5, 8),   // Victoire 1945
                new DateTime(year, 7, 14),  // Fête Nationale
                new DateTime(year, 8, 15),  // Assomption
                new DateTime(year, 11, 1),  // Toussaint
                new DateTime(year, 11, 11), // Armistice 1918
                new DateTime(year, 12, 25), // Noël
                
                // Jours fériés à date variable (basés sur Pâques)
                GetEasterSunday(year).AddDays(1),   // Lundi de Pâques
                GetEasterSunday(year).AddDays(39),  // Jeudi de l'Ascension
                GetEasterSunday(year).AddDays(50),  // Lundi de Pentecôte
            };

            return holidays;
        }

        // Algorithme pour calculer le dimanche de Pâques (algorithme de Meeus/Jones/Butcher)
        private static DateTime GetEasterSunday(int year)
        {
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 1;

            return new DateTime(year, month, day);
        }

        public static bool IsPublicHoliday(DateTime date)
        {
            var holidays = GetHolidays(date.Year);
            return holidays.Any(h => h.Date == date.Date);
        }

        public static List<DateTime> GetHolidaysInRange(DateTime startDate, DateTime endDate)
        {
            var holidays = new List<DateTime>();
            for (int year = startDate.Year; year <= endDate.Year; year++)
            {
                holidays.AddRange(GetHolidays(year));
            }
            return holidays
                .Where(h => h >= startDate.Date && h <= endDate.Date)
                .ToList();
        }
    }
}