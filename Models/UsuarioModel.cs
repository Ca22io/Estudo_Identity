using Microsoft.AspNetCore.Identity;

namespace App.Models
{
    public class UsuarioModel : IdentityUser<int>
    {
        public required string Nome { get; set; }

        public string? Cpf { get; set; }
    }
}