using YoutubeClone.Domain.Interfaces.Repositories;

namespace YoutubeClone.Domain.Database
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; set; }
        Task SaveChangesAsync();
    }
}
