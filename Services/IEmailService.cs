using App.Models;

namespace App.Services
{
    public interface IEmailService
    {
        Task EnviarEmail(string emailDestino, string assunto, string mensagemTexto);

        Task EnviarLinkDeConfirmacao(UsuarioModel user, string linkConfirmacao, string assunto = "Confirmação de E-mail");
    }
}