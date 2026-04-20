using YoutubeClone.Domain.Database;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Interfaces.Repositories;

namespace YoutubeClone.Infrastructure.Persistence.SqlServer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YoutubeCloneContext context;
        public IUserRepository userRepository { get; set; }
        public IEmailTemplateRepository emailTemplateRepository { get; set; }

        public UnitOfWork(YoutubeCloneContext _context,
            IUserRepository _userRepository,
            IEmailTemplateRepository _emailTemplateRepository)
        {
            userRepository = _userRepository;
            context = _context;
            emailTemplateRepository = _emailTemplateRepository;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}