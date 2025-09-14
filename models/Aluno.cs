namespace VibeCheckAPI_Dotnet8.Models
{
    public class Aluno : Usuario
    {
        // FK obrigatória para Turma
        public int TurmaId { get; set; }
        public Turma? Turma { get; set; }
    }
}
