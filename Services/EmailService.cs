using App.Dto;
using App.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace App.Services // Substitua pelo seu namespace
{
    // Implementa a interface IEmailSender exigida pelo Identity
    public class EmailService : IEmailService
    {
        private readonly EmailModel _settings;

        // Injeta as opções lidas do appsettings.json
        public EmailService(IOptions<EmailModel> settings)
        {
            _settings = settings.Value;
        }

        public async Task EnviarEmail(string emailDestino, string assunto, string mensagemTexto)

        {
            // 1. Criação da Mensagem (MimeMessage)
            var message = new MimeMessage();
            
            // Quem envia
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            
            // Para quem vai
            message.To.Add(MailboxAddress.Parse(emailDestino));
            
            // Assunto
            message.Subject = assunto;

            // Corpo do E-mail
            var builder = new BodyBuilder
            {
                HtmlBody = mensagemTexto // Ou TextBody se for texto puro
            };
            message.Body = builder.ToMessageBody();

            // 2. Conexão e Envio (SmtpClient do MailKit)
            using (var client = new SmtpClient())
            {
                try
                {
                    // Conecta ao servidor (Use StartTls para porta 587 ou SslOnConnect para 465)
                    await client.ConnectAsync(_settings.Server, _settings.Port, SecureSocketOptions.StartTls);

                    // Autentica
                    await client.AuthenticateAsync(_settings.Username, _settings.Password);

                    // Envia
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    // Logar o erro aqui
                    throw new InvalidOperationException($"Erro ao enviar e-mail: {ex.Message}");
                }
                finally
                {
                    // Desconecta limpo
                    await client.DisconnectAsync(true);
                }
            }
        }
    
        public async Task EnviarLinkDeConfirmacao(UsuarioModel user, string linkConfirmacao, string assunto = "Confirmação de E-mail")
        {

            var corpoEmail = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <h2>Confirme seu endereço de e-mail</h2>
                <p>Olá {user.Nome},</p>
                <p>Obrigado por se cadastrar. Por favor, clique no botão abaixo para confirmar seu e-mail e ativar sua conta.</p>
                <p style='text-align: center;'>
                    <a href='{linkConfirmacao}' style='background-color: #007bff; color: white; padding: 14px 25px; text-align: center; text-decoration: none; display: inline-block; border-radius: 5px; font-size: 16px;'>
                    Confirmar E-mail
                    </a>
                </p>
                <p>Se o botão acima não funcionar, copie e cole o seguinte link no seu navegador:</p>
                <p><a href='{linkConfirmacao}'>{linkConfirmacao}</a></p>
                <hr>
                <p><small>Se você não criou esta conta, por favor, ignore este e-mail.</small></p>
                </div>";

            await EnviarEmail(
                user.Email,
                assunto,
                corpoEmail
            );
        }
    }
}