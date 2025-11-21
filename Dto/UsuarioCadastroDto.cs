using System.ComponentModel.DataAnnotations;

namespace App.Dto
{
    public class UsuarioCadastroDto
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nome Completo")]
        [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres.")]
        public required string Nome { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public required string Email { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "CPF")]
        public string? Cpf { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
        public required string ConfirmPassword { get; set; }
    }
}