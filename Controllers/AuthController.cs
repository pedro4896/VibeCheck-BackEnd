using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("")]
public class AuthController : ControllerBase
{
    [HttpGet("login")]
    public IActionResult TriggerGoogleLogin()
    {
        var redirectUrl = Url.Action("GoogleResponse", "Auth");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, "Google");
    }

    [HttpGet("")]
    [AllowAnonymous]
    public IActionResult PublicPage()
    {
        return Ok("Bem-vindo ao Vibe Check! Faça login para continuar via /login.");
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync("Google");

        if (result?.Succeeded == true)
        {
            // Usuário autenticado com sucesso
            // Aqui você pode redirecionar para uma página de sucesso ou retornar dados
            return Ok(new { message = "Login realizado com sucesso!", user = result.Principal?.Identity?.Name });
        }

        return BadRequest(new { error = "Falha na autenticação" });
    }

    [HttpGet("user/details")]
    [Authorize]
    public IActionResult GetUserDetails()
    {
        var user = HttpContext.User;
        if (user?.Identity == null || !user.Identity.IsAuthenticated)
        {
            return Unauthorized(new { error = "Usuário não autenticado" });
        }

        var name = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var googleId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var userDetails = new Dictionary<string, object>
        {
            { "name", name ?? string.Empty },
            { "email", email ?? string.Empty },
            { "googleId", googleId ?? string.Empty },
            { "roles", roles }
        };

        return Ok(userDetails);
    }
}
