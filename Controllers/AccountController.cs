using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using App.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

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

            var model = new RecuperarSenhaDto
            {
                Email = user.Email,
                Token = token
            };

            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarSenha(RecuperarSenhaDto model)
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

            var tokenBytes = WebEncoders.Base64UrlDecode(model.Token);
            var tokenDecodificado = Encoding.UTF8.GetString(tokenBytes);

            var result = await _userManager.ResetPasswordAsync(user, tokenDecodificado, model.Password);

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "Senha redefinida com sucesso. Você já pode fazer login com sua nova senha.";
                return RedirectToAction("Login", "Home");
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

        [Authorize]
        public IActionResult TrocarSenha()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken] // Proteção CSRF obrigatória!
        public async Task<IActionResult> TrocarSenha(TrocarSenhaDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User); 
            
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            // 3. Chamar a Função de Troca de Senha do Identity
            var result = await _userManager.ChangePasswordAsync(
                user, 
                model.SenhaAntiga, 
                model.NovaSenha
            );

            // 4. Analisar o Resultado
            if (result.Succeeded)
            {

                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                
                TempData["StatusMessage"] = "Sua senha foi alterada com sucesso. Faça login novamente.";
                return RedirectToAction("Login", "Home"); 
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            
            return View(model);
        }
    }
}