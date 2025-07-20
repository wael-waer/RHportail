public class EmailRepository : IEmailRepository
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailRepository> _logger;

    public EmailRepository(IConfiguration config, ILogger<EmailRepository> logger)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(to))
            throw new ArgumentException("Adresse destinataire invalide.", nameof(to));
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Sujet de l'email invalide.", nameof(subject));
        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Corps de l'email invalide.", nameof(body));

        var smtpHost = _config["Email:SmtpHost"];
        var smtpPortStr = _config["Email:SmtpPort"];
        var smtpUser = _config["Email:SmtpUser"];
        var smtpPass = _config["Email:SmtpPass"];

        if (string.IsNullOrWhiteSpace(smtpHost) ||
            string.IsNullOrWhiteSpace(smtpPortStr) ||
            string.IsNullOrWhiteSpace(smtpUser) ||
            string.IsNullOrWhiteSpace(smtpPass))
        {
            throw new InvalidOperationException("La configuration SMTP est incomplète.");
        }

        if (!int.TryParse(smtpPortStr, out int smtpPort))
            throw new InvalidOperationException("Le port SMTP n'est pas un nombre valide.");

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(smtpUser));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart("plain") { Text = body };

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(smtpUser, smtpPass);
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            // Tu peux logger l'exception ici si tu as un logger injecté
            throw new InvalidOperationException("Erreur lors de l'envoi de l'email.", ex);
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}