using Microsoft.EntityFrameworkCore;
using VibeCheckAPI_Dotnet8.Data.Context;
using System.Security.Claims;
using VibeCheckAPI_Dotnet8.Services;
using VibeCheckAPI_Dotnet8.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configuração do Entity Framework com PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Serviços de domínio
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
builder.Services.AddScoped<ITurmaService, TurmaService>();
builder.Services.AddScoped<IRegistroEmocionalService, RegistroEmocionalService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configuração de autenticação OAuth2 com Google
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Google";
})
.AddCookie("Cookies")
.AddGoogle("Google", options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
    options.CallbackPath = "/signin-google";

    // Solicitar scopes necessários
    options.Scope.Add("profile");
    options.Scope.Add("email");

    // Configurar eventos de autenticação
    options.Events.OnCreatingTicket = context =>
    {
        var email = context.Principal?.FindFirst(ClaimTypes.Email)?.Value;

        if (!string.IsNullOrEmpty(email) && context.Principal?.Identity is ClaimsIdentity identity)
        {
            if (email.EndsWith("@belojardim.ifpe.edu.br") || email.Contains("professor"))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "ROLE_PROFESSOR"));
            }
            else
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "ROLE_ALUNO"));
            }
        }

        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization(options =>
{
    // Política para professores
    options.AddPolicy("ApenasProfessor", policy =>
        policy.RequireRole("ROLE_PROFESSOR"));

    // Política para alunos
    options.AddPolicy("ApenasAluno", policy =>
        policy.RequireRole("ROLE_ALUNO"));

    // Política para usuários autenticados
    options.AddPolicy("ApenasAutenticado", policy =>
        policy.RequireAuthenticatedUser());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();