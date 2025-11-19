namespace App.Services
{
    public interface IEmailService
    {
        Task EnviarEmail(string emailDestino, string assunto, string mensagemTexto);
    }
}