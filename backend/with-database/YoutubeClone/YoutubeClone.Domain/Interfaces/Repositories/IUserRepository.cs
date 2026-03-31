using YoutubeClone.Domain.Database.SqlServer.Entities;

namespace YoutubeClone.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserAccount> Create(UserAccount userAccount);
        Task<UserAccount?> Get(Guid userId);
        IQueryable<UserAccount> Queryable();
        Task<bool> IfExist(Guid userId);
        Task<bool> IfExist(string userName);
        Task<UserAccount> Update(UserAccount userAccount);
    }
}
