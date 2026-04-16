using YoutubeClone.Domain.Database;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Interfaces.Repositories;

namespace YoutubeClone.Infrastructure.Persistence.SqlServer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YoutubeCloneContext context;
        public IUserRepository userRepository { get; set; }

        public UnitOfWork(YoutubeCloneContext _context, IUserRepository _userRepository)
        {
            userRepository = _userRepository;
            context = _context;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}