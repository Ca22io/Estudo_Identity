namespace App.Services
{
    public interface IEmailService
    {
        Task EnviarEmail(string email, string subject, string htmlMessage);
    }
}