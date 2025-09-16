using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VibeCheckAPI_Dotnet8.DTOs;
using VibeCheckAPI_Dotnet8.Services;

namespace VibeCheckAPI_Dotnet8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroEmocionalController : ControllerBase
    {
        private readonly IRegistroEmocionalService _registroService;
        public RegistroEmocionalController(IRegistroEmocionalService registroService)
        {
            _registroService = registroService;
        }

        private string? GetGoogleSub() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

        [HttpPost("registrar")]
        [Authorize(Policy = "ApenasAluno")]
        public async Task<ActionResult<RegistroEmocionalDTO>> RegistrarEmocao([FromQuery] string codigo, [FromQuery] int emocaoId)
        {
            var alunoGoogleId = GetGoogleSub();
            if (alunoGoogleId is null) return Unauthorized();

            var registro = await _registroService.RegistrarEmocaoAsync(alunoGoogleId, codigo, emocaoId);

            var dto = new RegistroEmocionalDTO
            {
                Emocao = registro.Emocao!.ValorNumerico,
                Tipo = registro.Avaliacao!.TipoAvaliacao.ToString(),
                Data = registro.Avaliacao!.DataCriacao.ToString("O"),
                Turma = registro.Avaliacao!.Turma!.Nome
            };

            return Ok(dto);
        }
    }
}