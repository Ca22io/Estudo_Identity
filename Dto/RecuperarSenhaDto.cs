namespace App.Dto
{
    public class RecuperarSenhaDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Token { get; set; }
    }
}