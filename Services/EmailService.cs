using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using App.Models;

namespace App.Services // Substitua pelo seu namespace
{
    // Implementa a interface IEmailSender exigida pelo Identity
    public class EmailService : IEmailService
    {
        private readonly EmailModel _options;

        // Injeta as opções lidas do appsettings.json
        public EmailService(IOptions<EmailModel> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public Task EnviarEmail(string email, string subject, string htmlMessage)
        {
            try
            {
                var message = new MailMessage();
                
                // Endereço de envio será o mesmo do UserName
                message.From = new MailAddress(_options.UserName, _options.SenderName); 
                
                // Endereço de destino
                message.To.Add(new MailAddress(email)); 
                
                // Assunto e corpo
                message.Subject = subject;
                message.Body = htmlMessage;
                message.IsBodyHtml = true;

                // Configuração do cliente SMTP (Gmail)
                using (var client = new SmtpClient(_options.Host, _options.Port))
                {
                    client.EnableSsl = _options.EnableSSL;
                    client.UseDefaultCredentials = false;
                    
                    // Credenciais de autenticação (email e App Password)
                    client.Credentials = new NetworkCredential(_options.UserName, _options.Password);

                    // Envia a mensagem de forma assíncrona
                    return client.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                // Em um projeto real, você logaria este erro (ex: Serilog)
                Console.WriteLine($"Erro ao enviar email para {email}: {ex.Message}");
                return Task.FromException(ex);
            }
        }
    }
}