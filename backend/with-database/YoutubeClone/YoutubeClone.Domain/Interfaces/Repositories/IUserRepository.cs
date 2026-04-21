using YoutubeClone.Domain.Database.SqlServer.Entities;

namespace YoutubeClone.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<UserAccount>
    {
        Task<UserAccount?> Get(Guid userId);
        Task<UserAccount?> Get(string email);
        Task<bool> HasCreated();
        Task<bool> ClearRoles(List<UserAccountRole> roles);
    }
}
