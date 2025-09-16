using Microsoft.EntityFrameworkCore;
using VibeCheckAPI_Dotnet8.Data.Context;
using VibeCheckAPI_Dotnet8.DTOs;
using VibeCheckAPI_Dotnet8.Models;
using VibeCheckAPI_Dotnet8.Repositories;

namespace VibeCheckAPI_Dotnet8.Services;

public interface IAvaliacaoService
{
    Task<CodigoAvaliacaoResponseDTO> GerarCodigoCheckinAsync(string googleId, string nomeTurma, int validadeSegundos);
    Task<CodigoAvaliacaoResponseDTO> GerarCodigoCheckoutAsync(string googleId, string nomeTurma, int validadeSegundos);
    Task<bool> VerificarCodigo(string codigo);
}

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IUnitOfWork _uow;

    public AvaliacaoService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    private static string GerarCodigo(int length = 6)
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        return new string(Enumerable.Range(0, length).Select(_ => chars[Random.Shared.Next(chars.Length)]).ToArray());
    }

    private async Task<CodigoAvaliacaoResponseDTO> CriarAvaliacaoAsync(TipoAvaliacao tipo, Turma turma, int validadeSegundos)
    {
        var agora = DateTime.UtcNow;
        var avaliacao = new Avaliacao
        {
            Codigo = GerarCodigo(),
            TipoAvaliacao = tipo,
            DataCriacao = agora,
            DataExpiracao = agora.AddSeconds(validadeSegundos),
            Ativa = true,
            TurmaId = turma.Id
        };

        _uow.AvaliacaoRepository.Criar(avaliacao);
        await _uow.CommitAsync();

        return new CodigoAvaliacaoResponseDTO
        {
            Codigo = avaliacao.Codigo,
            ExpiraEm = avaliacao.DataExpiracao,
            Tipo = tipo.ToString()
        };
    }

    public async Task<CodigoAvaliacaoResponseDTO> GerarCodigoCheckinAsync(string googleId, string nomeTurma, int validadeSegundos)
    {
        var turma = await _uow.TurmaRepository.ObterAsync(t => t.Nome == nomeTurma && t.Professor!.GoogleId == googleId);
        return await CriarAvaliacaoAsync(TipoAvaliacao.Checkin, turma!, validadeSegundos);
    }

    public async Task<CodigoAvaliacaoResponseDTO> GerarCodigoCheckoutAsync(string googleId, string nomeTurma, int validadeSegundos)
    {
        var turma = await _uow.TurmaRepository.ObterAsync(t => t.Nome == nomeTurma && t.Professor!.GoogleId == googleId);
        return await CriarAvaliacaoAsync(TipoAvaliacao.Checkout, turma!, validadeSegundos);
    }

    public async Task<bool> VerificarCodigo(string codigo)
    {
        var avaliacao = await _uow.AvaliacaoRepository.ObterAsync(a => a.Codigo == codigo);
        return avaliacao != null && avaliacao.Ativa;
    }
}
