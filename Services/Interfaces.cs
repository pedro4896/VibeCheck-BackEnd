using VibeCheckAPI_Dotnet8.DTOs;
using VibeCheckAPI_Dotnet8.Models;

namespace VibeCheckAPI_Dotnet8.Services;



public interface ITurmaService
{
    Task<IEnumerable<TurmaDTO>> ListarTurmasProfessorAsync(string googleId);
}

