using Microsoft.AspNetCore.Identity.UI.Services;

public class ConsoleEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine("--- NOVO E-MAIL ---");
        Console.WriteLine($"Para: {email}");
        Console.WriteLine($"Assunto: {subject}");
        Console.WriteLine("--- Link de Confirmação (copie e cole no browser) ---");
        Console.WriteLine(htmlMessage);
        Console.WriteLine("-------------------");
        return Task.CompletedTask;
    }
}