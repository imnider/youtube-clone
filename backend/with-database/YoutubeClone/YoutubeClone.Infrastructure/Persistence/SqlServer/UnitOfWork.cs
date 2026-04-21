using YoutubeClone.Domain.Database;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Interfaces.Repositories;

namespace YoutubeClone.Infrastructure.Persistence.SqlServer
{
    public class UnitOfWork(YoutubeCloneContext _context,
        IUserRepository _userRepository,
        IEmailTemplateRepository _emailTemplateRepository,
        IRoleRepository _roleRepository)
        : IUnitOfWork
    {
        private readonly YoutubeCloneContext context = _context;
        public IUserRepository userRepository { get; set; } = _userRepository;
        public IEmailTemplateRepository emailTemplateRepository { get; set; } = _emailTemplateRepository;
        public IRoleRepository roleRepository { get; set; } = _roleRepository;

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}