using App.Dto;
using Microsoft.AspNetCore.Identity;

namespace App.Services
{
    public interface IUsuarioService
    {
        Task<IdentityResult> CadastrarUsuarioAsync (UsuarioCadastroDto usuario);

        Task<SignInResult> LoginUsuarioAsync (UsuarioLoginDto usuario);

        Task LogoutUsuarioAsync ();

    }
}