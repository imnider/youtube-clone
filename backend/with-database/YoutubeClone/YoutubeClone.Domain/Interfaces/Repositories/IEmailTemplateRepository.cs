using YoutubeClone.Domain.Database.SqlServer.Entities;

namespace YoutubeClone.Domain.Interfaces.Repositories
{
    public interface IEmailTemplateRepository
    {
        Task<List<EmailTemplate>> Get();
    }
}
