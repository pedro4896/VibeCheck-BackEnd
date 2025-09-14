namespace LabSolos_Server_DotNet8.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Nome { get; set; }
        public required string GoogleId { get; set; }
        public required TipoUsuario TipoUsuario { get; set; }
    }
}