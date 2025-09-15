using Microsoft.EntityFrameworkCore;
using VibeCheckAPI_Dotnet8.Data.Context;
using VibeCheckAPI_Dotnet8.DTOs;
using VibeCheckAPI_Dotnet8.Repositories;

namespace VibeCheckAPI_Dotnet8.Services;

public interface IRegistroEmocionalService
{
    Task<IEnumerable<RegistroEmocionalDTO>> GetDashboardAsync();
}

public class RegistroEmocionalService : IRegistroEmocionalService
{
    private readonly IUnitOfWork _uow;
    public RegistroEmocionalService(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public async Task<IEnumerable<RegistroEmocionalDTO>> GetDashboardAsync()
    {
        // Exemplo simples: agrupar avaliações ativas por turma e tipo
        var registrosEmocionais = await _uow.RegistroEmocionalRepository
            .ObterTodosAsync(re => true, query => query
                .Include(re => re.Avaliacao)
                    .ThenInclude(a => a!.Turma)
                .Include(re => re.Emocao)
            );

        var registrosEmocionaisFormatados = registrosEmocionais
            .OrderBy(re => re.Avaliacao!.DataCriacao)
            .Select(re => new RegistroEmocionalDTO
            {
                Turma = re.Avaliacao!.Turma!.Nome,
                Tipo = re.Avaliacao!.TipoAvaliacao.ToString(),
                Data = re.Avaliacao!.DataCriacao.ToString("dd/MM/yyyy HH:mm"),
                Emocao = re.Emocao!.ValorNumerico
            }).ToList();

        return registrosEmocionaisFormatados;
    }
}
