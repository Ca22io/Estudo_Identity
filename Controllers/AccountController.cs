using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace App.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UsuarioModel> _userManager;

        public AccountController(UserManager<UsuarioModel> userManager)
        {
            _userManager = userManager;
        }

        // Este método é executado quando o usuário clica no link do e-mail
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            if (userId == 0 || token == null)
            {
                TempData["StatusMessage"] = "Erro: Parâmetros de confirmação ausentes.";
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            
            if (user == null)
            {
                TempData["StatusMessage"] = "Erro: Usuário não encontrado.";
                return View();
            }

            // ----------------------------------------------------------------------
            // 1. DECIFRA O TOKEN: Converte o token URL-seguro de volta para o original
            // ----------------------------------------------------------------------
            var tokenBytes = WebEncoders.Base64UrlDecode(token);
            var tokenDecodificado = Encoding.UTF8.GetString(tokenBytes);
            
            // ----------------------------------------------------------------------
            // 2. Confirma o e-mail usando o token decodificado
            // ----------------------------------------------------------------------
            var result = await _userManager.ConfirmEmailAsync(user, tokenDecodificado); 

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "Obrigado por confirmar seu e-mail. Você já pode fazer login!";
            }
            else
            {
                TempData["StatusMessage"] = "Erro: O token de confirmação é inválido ou expirou. Tente registrar-se novamente.";
            }

            return View(); 
        }

        public async Task<IActionResult> RecuperarSenha(int userId, string token)
        {
            if (userId == 0 || token == null)
            {
                TempData["StatusMessage"] = "Erro: Parâmetros de redefinição de senha ausentes.";
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            
            if (user == null)
            {
                TempData["StatusMessage"] = "Erro: Usuário não encontrado.";
                return View();
            }

            // Decodifica o token
            var tokenBytes = WebEncoders.Base64UrlDecode(token);
            var tokenDecodificado = Encoding.UTF8.GetString(tokenBytes);

            var model = new App.Dto.RecuperarSenhaDto
            {
                Email = user.Email,
                Token = tokenDecodificado
            };

            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarSenha(App.Dto.RecuperarSenhaDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["StatusMessage"] = "Erro: Usuário não encontrado.";
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "Senha redefinida com sucesso. Você já pode fazer login com sua nova senha.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }
    }
}