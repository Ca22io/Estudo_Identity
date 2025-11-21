using System.ComponentModel.DataAnnotations;

namespace App.Dto
{
    public class RecuperarSenhaDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        [Compare("Password", ErrorMessage = "A nova senha e a confirmação de senha não coincidem.")]
        public string? ConfirmPassword { get; set; }

        public string? Token { get; set; }
    }
}