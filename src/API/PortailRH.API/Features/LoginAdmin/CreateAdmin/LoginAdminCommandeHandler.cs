using Microsoft.AspNetCore.Identity;

namespace PortailRH.API.Features.Admins.Login
{
    public record LoginAdminCommand(string Email, string Password) : ICommand<LoginAdminResult>;

    public record LoginAdminResult(bool IsSuccess, string? Message , string? Role);

    public class LoginAdminCommandValidator : AbstractValidator<LoginAdminCommand>
    {
        public LoginAdminCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class LoginAdminCommandHandler : ICommandHandler<LoginAdminCommand, LoginAdminResult>
    {
        private readonly IAdminRepository _adminRepository;

        public LoginAdminCommandHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<LoginAdminResult> Handle(LoginAdminCommand command, CancellationToken cancellationToken)
        {
            var admin = await _adminRepository.GetByEmailAsync(command.Email);

            if (admin == null)
                return new LoginAdminResult(false, "Admin not found", null);

            var passwordHasher = new PasswordHasher<Admin>();
            var result = passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, command.Password);

            if (result == PasswordVerificationResult.Failed)
                return new LoginAdminResult(false, "Invalid password", null);

            
            return new LoginAdminResult(true, null, "Admin");
        }
    }
}
