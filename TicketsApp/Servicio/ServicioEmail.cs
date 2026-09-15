using DocumentFormat.OpenXml.Drawing;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

public class ServicioCorreo : IEmailService
{
    private readonly EmailSettings emailConfig;

    public ServicioCorreo(IOptions<EmailSettings> _emailConfig)
    {
        emailConfig = _emailConfig.Value;
    }

    public async Task MandarEmailAsync(string para, string asunto, string htmlBody)
    {
        var correo = new MimeMessage();
        correo.From.Add(new MailboxAddress(emailConfig.SenderName, emailConfig.Username));
        if (!MailboxAddress.TryParse(para, out var destino))
        {
            Console.WriteLine("Direccion invalida no se pudo parsear");
        }
        correo.To.Add(destino);
        correo.Subject = asunto;

        correo.Body = new TextPart("html")
        {
            Text = htmlBody
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            emailConfig.Host,
            emailConfig.Port,
            SecureSocketOptions.StartTls
        );

        await smtp.AuthenticateAsync(emailConfig.Username, emailConfig.Password);
        await smtp.SendAsync(correo);
        await smtp.DisconnectAsync(true);
    }
}