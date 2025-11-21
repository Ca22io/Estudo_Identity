using System.ComponentModel.DataAnnotations;

namespace App.Dto
{   
    public class TrocarSenhaDto
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Antiga")]
        public string SenhaAntiga { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string NovaSenha { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "A nova senha e a confirmação de senha não coincidem.")]
        [Display(Name = "Confirmar Nova Senha")]
        public string ConfirmarSenha { get; set; }
    }
}