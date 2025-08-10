
namespace PortailRH.API.Features.Employees.LoginEmployee
{

    public record LoginEmployeeCommand(string NumeroIdentification, string MotDePasse)
        : IRequest<LoginEmployeeResult>;

    public record LoginEmployeeResult(string Token, string Nom, string Prenom, string Email , int Id);

    public class LoginEmployeeCommandValidator : AbstractValidator<LoginEmployeeCommand>
    {
        public LoginEmployeeCommandValidator()
        {
            RuleFor(x => x.NumeroIdentification).NotEmpty().WithMessage("Le matricule est requis.");
            RuleFor(x => x.MotDePasse).NotEmpty().WithMessage("Le mot de passe est requis.");
        }
    }

    public class LoginEmployeeCommandHandler : IRequestHandler<LoginEmployeeCommand, LoginEmployeeResult>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IConfiguration _config;

        public LoginEmployeeCommandHandler(IEmployeeRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public async Task<LoginEmployeeResult> Handle(LoginEmployeeCommand request, CancellationToken cancellationToken)
        {

            var employe = await _repository.GetByNumeroIdentificationAsync(request.NumeroIdentification);
            Console.WriteLine($"Recherche employé pour matricule {request.NumeroIdentification} : {(employe != null ? "trouvé" : "non trouvé")}");

            if (employe == null)
                return null!;

            var isValid = BCrypt.Net.BCrypt.Verify(request.MotDePasse, employe.MotDePasse);
            Console.WriteLine($"Mot de passe validé : {isValid}");

            if (!isValid)
                return null!;


            var token = GenerateJwtToken(employe);

            return new LoginEmployeeResult(
                Token: token,
                Nom: employe.Nom,
                Prenom: employe.Prenom,
                Email: employe.Email,
                Id: employe.Id
            );
        }

        private string GenerateJwtToken(Employee employe)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, employe.NumeroIdentification),
            new Claim("EmployeeId", employe.Id.ToString()),

            new Claim("nom", employe.Nom),
            new Claim("prenom", employe.Prenom),
            new Claim(JwtRegisteredClaimNames.Email, employe.Email),
            new Claim("id", employe.Id.ToString()),
            new Claim(ClaimTypes.Name, employe.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var keyBytes = Convert.FromBase64String(_config["Jwt:Key"]!);
            var key = new SymmetricSecurityKey(keyBytes);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(72),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
