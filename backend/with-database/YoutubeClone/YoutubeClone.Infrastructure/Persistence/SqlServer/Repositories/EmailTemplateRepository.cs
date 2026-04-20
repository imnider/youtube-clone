using Microsoft.EntityFrameworkCore;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Database.SqlServer.Entities;
using YoutubeClone.Domain.Interfaces.Repositories;

namespace YoutubeClone.Infrastructure.Persistence.SqlServer.Repositories
{
    public class EmailTemplateRepository(YoutubeCloneContext context) : IEmailTemplateRepository
    {
        public async Task<List<EmailTemplate>> Get()
        {
            return await context.EmailTemplates.ToListAsync();
        }
    }
}
