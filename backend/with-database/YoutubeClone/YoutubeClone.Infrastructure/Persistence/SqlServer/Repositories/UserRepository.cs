using Microsoft.EntityFrameworkCore;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Database.SqlServer.Entities;
using YoutubeClone.Domain.Interfaces.Repositories;

namespace YoutubeClone.Infrastructure.Persistence.SqlServer.Repositories
{
    public class UserRepository(YoutubeCloneContext context) : GenericRepository<UserAccount>(context), IUserRepository

    {
        public async Task<bool> ClearRoles(List<UserAccountRole> roles)
        {
            context.UserAccountRoles.RemoveRange(roles);
            return true;
        }

        public async Task<UserAccount?> Get(Guid userId)
        {
            try
            {
                return await context.UserAccounts
                    .Include(user => user.UserAccountRoles)
                    .ThenInclude(userRoles => userRoles.Role)
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.DeletedAt == null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserAccount?> Get(string email)
        {
            try
            {
                return await context.UserAccounts
                    .Include(user => user.UserAccountRoles)
                    .ThenInclude(userRoles => userRoles.Role)
                    .FirstOrDefaultAsync(x => x.Email == email && x.DeletedAt == null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> HasCreated()
        {
            try
            {
                return await context.UserAccounts.AnyAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
