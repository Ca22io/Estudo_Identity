namespace App.Models
{
    public class EmailModel
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
    }
}