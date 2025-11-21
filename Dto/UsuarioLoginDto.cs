using System.ComponentModel.DataAnnotations;

namespace App.Dto
{
    public class UsuarioLoginDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public required string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}