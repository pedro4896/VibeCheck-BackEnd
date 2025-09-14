namespace VibeCheckAPI_Dotnet8.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public required string Codigo { get; set; }
        public required TipoAvaliacao TipoAvaliacao { get; set; }
        public required DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public required DateTime DataExpiracao { get; set; }
        public bool Ativa { get; set; } = false;

        public int TurmaId { get; set; }
        public Turma? Turma { get; set; }

        public ICollection<Emocao>? Emocoes { get; set; }

    }
}