


namespace PortailRH.API.Features.Employees.CreateEmployee
{
   
        public record CreateEmployeeCommand(
            string Nom,
            string Prenom,
            string Email,
            DateTime DateNaissance,
            string Fonction,
            string MotDePasse,
            string Etablissement,
            DateTime DateEntree,
            string NumeroTelephone,
            string? EmailSecondaire,
            string? NumeroTelephoneSecondaire,
            string NumeroIdentification,
            int SoldeConge,
            int SoldeCongeMaladie,
            decimal Salaire
        ) : ICommand<CreateEmployeeResult>;

        public record CreateEmployeeResult(int Id);
    

    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.Nom).NotEmpty().WithMessage("Le nom est obligatoire.");
            RuleFor(x => x.Prenom).NotEmpty().WithMessage("Le prénom est obligatoire.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email invalide.");
            RuleFor(x => x.DateNaissance).LessThan(DateTime.Today).WithMessage("La date de naissance doit être dans le passé.");
            RuleFor(x => x.Fonction).NotEmpty().WithMessage("La fonction est obligatoire.");
            RuleFor(x => x.Etablissement).NotEmpty().WithMessage("L'établissement est obligatoire.");
            RuleFor(x => x.DateEntree).LessThanOrEqualTo(DateTime.Today).WithMessage("La date d'entrée doit être aujourd'hui ou dans le passé.");
            RuleFor(x => x.NumeroTelephone).NotEmpty().WithMessage("Le numéro de téléphone est obligatoire.");
            RuleFor(x => x.NumeroIdentification).NotEmpty().WithMessage("Le numéro d'identification est obligatoire.");
            RuleFor(x => x.Salaire).GreaterThan(0).WithMessage("Le salaire doit être supérieur à 0.");
            RuleFor(x => x.SoldeConge).GreaterThanOrEqualTo(0);
            RuleFor(x => x.SoldeCongeMaladie).GreaterThanOrEqualTo(0);
            RuleFor(x => x.MotDePasse).NotEmpty().MinimumLength(8).WithMessage("Le mot de passe est obligatoire et doit contenir au moins 8 caractères.");

        }
    }
    public class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, CreateEmployeeResult>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly ISuiviCongeRepository _suiviCongeRepository;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository , IEmailRepository emailRepository, ISuiviCongeRepository suiviCongeRepository)
        {
            _employeeRepository = employeeRepository;
            _emailRepository = emailRepository;
            _suiviCongeRepository = suiviCongeRepository;
        }

        public async Task<CreateEmployeeResult> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            // Hash the password
            string motDePasseHash = BCrypt.Net.BCrypt.HashPassword(command.MotDePasse);

            // Map to Employee entity
            var employe = new Employee
            {
                Nom = command.Nom,
                Prenom = command.Prenom,
                Email = command.Email,
                DateNaissance = command.DateNaissance,
                Fonction = command.Fonction,
                Etablissement = command.Etablissement,
                DateEntree = command.DateEntree,
                NumeroTelephone = command.NumeroTelephone,
                EmailSecondaire = command.EmailSecondaire,
                NumeroTelephoneSecondaire = command.NumeroTelephoneSecondaire,
                NumeroIdentification = command.NumeroIdentification,
                SoldeConge = command.SoldeConge,
                SoldeCongeMaladie = command.SoldeCongeMaladie,
                Salaire = command.Salaire,
                MotDePasse = motDePasseHash
            };

            var employeCree = await _employeeRepository.AddAsync(employe);
            var currentYear = DateTime.UtcNow.Year;

            var suiviConge = new SuiviConge
            {
                EmployeeId = employeCree.Id,
                SoldeInitial = command.SoldeConge,
                SoldeRestant = command.SoldeConge,
                Annee = currentYear,
                Actif = true
            };


            await _suiviCongeRepository.AddSuiviCongeAsync(suiviConge); 


            var subject = "Bienvenue sur le portail RH";
            var body = $"""
Bonjour {employe.Prenom},

Votre compte RH a été créé avec succès.

🆔 Matricule : {employe.NumeroIdentification}
🔐 Mot de passe : {command.MotDePasse}

Veuillez vous connecter à l'application mobile RH.

Cordialement,
L'équipe RH
""";
            await _emailRepository.SendEmailAsync(employe.Email, subject, body);
            return new CreateEmployeeResult(employeCree.Id);
        }
    }
}


