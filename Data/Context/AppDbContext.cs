
using LabSolos_Server_DotNet8.Models;
using Microsoft.EntityFrameworkCore;

namespace VibeCheckAPI_Dotnet8.Data.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de herança para Usuario
            modelBuilder.Entity<Usuario>()
                .HasDiscriminator<TipoUsuario>("TipoUsuario")
                .HasValue<Usuario>(TipoUsuario.Aluno)
                .HasValue<Usuario>(TipoUsuario.Professor);
        }
    }
}