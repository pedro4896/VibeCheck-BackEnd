

using VibeCheckAPI_Dotnet8.Data.Context;
using LabSolos_Server_DotNet8.Models;

namespace VibeCheckAPI_Dotnet8.Repositories
{

    public interface IUnitOfWork
    {
        IRepository<Usuario> UsuarioRepository { get; }

        Task CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;

        private IRepository<Usuario>? _usuarioRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<Usuario> UsuarioRepository
        {
            get
            {
                return _usuarioRepository ??= new Repository<Usuario>(_context);
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
