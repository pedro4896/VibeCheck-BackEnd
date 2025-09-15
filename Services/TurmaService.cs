using Microsoft.EntityFrameworkCore;
using VibeCheckAPI_Dotnet8.Data.Context;
using VibeCheckAPI_Dotnet8.DTOs;
using VibeCheckAPI_Dotnet8.Models;
using VibeCheckAPI_Dotnet8.Repositories;

namespace VibeCheckAPI_Dotnet8.Services;

public class TurmaService(IUnitOfWork uow) : ITurmaService
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<IEnumerable<TurmaDTO>> ListarTurmasProfessorAsync(string googleId)
    {
        var turmas = await _uow.TurmaRepository
            .ObterTodosAsync(t => t.Professor!.GoogleId == googleId);
            
        return turmas.Select(t => new TurmaDTO { Id = t.Id, Nome = t.Nome });
    }
}
