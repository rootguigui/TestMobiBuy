using System.Net;
using System.Net.Mail;
using MassTransit;
using TestMobiBuy.Application.Dtos;
using TestMobiBuy.Domain.Settings;

namespace TestMobiBuy.Consumer.Consumers;

public class EmailSendConsumer : IConsumer<CustomerCreateDto>
{
    private readonly ILogger<EmailSendConsumer> _logger;
    private readonly SmtpSettings _smtpSettings;

    public EmailSendConsumer(ILogger<EmailSendConsumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _smtpSettings = configuration.GetSection(SmtpSettings.SectionName).Get<SmtpSettings>() 
            ?? throw new ArgumentNullException(nameof(SmtpSettings));
    }

    public async Task Consume(ConsumeContext<CustomerCreateDto> context)
    {
        var message = context.Message;
        _logger.LogInformation("Mensagem recebida: {Text} - {Email} em {Timestamp}", message.Name, message.Email, DateTime.Now);
        
        var emailTemplate = $@"
            <html>
                <body>
                    <h1>Bem-vindo ao TestMobiBuy!</h1>
                    <p>Olá {message.Name},</p>
                    <p>Seu cadastro foi realizado com sucesso!</p>
                    <p>Seus dados:</p>
                    <ul>
                        <li>Nome: {message.Name}</li>
                        <li>E-mail: {message.Email}</li>
                        <li>CEP: {message.ZipCode}</li>
                    </ul>
                    <p>Atenciosamente,<br>Equipe TestMobiBuy</p>
                </body>
            </html>";

        var smtpClient = new SmtpClient(_smtpSettings.Host)
        {
            Port = _smtpSettings.Port,
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = _smtpSettings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            TargetName = $"STARTTLS/{_smtpSettings.Host}"
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
            Subject = "Bem-vindo ao TestMobiBuy!",
            IsBodyHtml = true,
            Body = emailTemplate
        };

        mailMessage.To.Add(message.Email);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
            _logger.LogInformation("Email enviado com sucesso para {Email}", message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email para {Email}", message.Email);
            throw;
        }
    }
}
