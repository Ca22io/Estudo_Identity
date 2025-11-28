using App.Dto;
using App.Models;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace App.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<UsuarioModel> _userManager;
        private readonly SignInManager<UsuarioModel> _signInManager;

        public UsuarioService(UserManager<UsuarioModel> userManager, SignInManager<UsuarioModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CadastrarUsuarioAsync(UsuarioCadastroDto usuario)
        {
            var usuarioModel = new UsuarioModel { UserName = usuario.Email, Email = usuario.Email, Nome = usuario.Nome, Cpf = usuario.Cpf };

            var resultado = await _userManager.CreateAsync(usuarioModel, usuario.Password);

            return resultado;
        }

        public async Task<SignInResult> LoginUsuarioAsync(UsuarioLoginDto usuario)
        {
            var resultado = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Password, isPersistent: usuario.RememberMe, lockoutOnFailure: false);

            return resultado;
        }

        public async Task LogoutUsuarioAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}