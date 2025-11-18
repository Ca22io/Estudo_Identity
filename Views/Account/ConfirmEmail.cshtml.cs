using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App.Models;

namespace App.Views.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<UsuarioModel> _userManager;

        // Propriedade para mostrar o status na View
        [TempData]
        public string StatusMessage { get; set; }

        public ConfirmEmailModel(UserManager<UsuarioModel> userManager)
        {
            _userManager = userManager;
        }

        // Este método é executado quando o usuário clica no link do e-mail
        public async Task<IActionResult> OnGetAsync(int userId, string token)
        {
            // 1. Verificar se os parâmetros obrigatórios vieram na URL
            if (userId == 0 || token == null)
            {
                StatusMessage = "Erro: Parâmetros de confirmação ausentes.";
                return RedirectToPage("./Index"); // Redireciona para a home
            }

            // 2. Buscar o usuário pelo ID
            var user = await _userManager.FindByIdAsync(userId.ToString());
            
            if (user == null)
            {
                StatusMessage = "Erro: Usuário não encontrado.";
                return Page(); 
            }

            // 3. Confirmar o e-mail usando o token
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                StatusMessage = "Obrigado por confirmar seu e-mail. Você já pode fazer login!";
            }
            else
            {
                StatusMessage = "Erro: O token de confirmação é inválido ou expirou. Tente registrar-se novamente.";
            }

            // Retorna a página (View) para exibir a mensagem de status
            return Page(); 
        }
    }
}