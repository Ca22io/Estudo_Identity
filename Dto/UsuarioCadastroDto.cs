namespace App.Dto
{
    public class UsuarioCadastroDto
    {
        public required string Nome { get; set; }

        public required string Email { get; set; }

        public string? Cpf { get; set; }

        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}