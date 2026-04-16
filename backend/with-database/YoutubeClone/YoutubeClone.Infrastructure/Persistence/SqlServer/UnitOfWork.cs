using YoutubeClone.Domain.Database;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Interfaces.Repositories;

namespace YoutubeClone.Infrastructure.Persistence.SqlServer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YoutubeCloneContext _context;
        public IUserRepository _userRepository { get; set; }

        public UnitOfWork(YoutubeCloneContext context, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
}
