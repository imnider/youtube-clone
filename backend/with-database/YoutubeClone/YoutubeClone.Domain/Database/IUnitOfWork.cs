using YoutubeClone.Domain.Interfaces.Repositories;

namespace YoutubeClone.Domain.Database
{
    public interface IUnitOfWork
    {
        IUserRepository _userRepository { get; set; }
        Task SaveChangesAsync();
    }
}
