using Microsoft.EntityFrameworkCore;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Database.SqlServer.Entities;
using YoutubeClone.Domain.Interfaces.Repositories;

namespace YoutubeClone.Infrastructure.Persistence.SqlServer.Repositories
{
    public class UserRepository(YoutubeCloneContext context) : IUserRepository

    {
        public async Task<UserAccount> Create(UserAccount userAccount)
        {
            try
            {
                // insert
                await context.UserAccounts.AddAsync(userAccount);

                // execution // commit
                await context.SaveChangesAsync();

                return userAccount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Delete(UserAccount userAccount)
        {
            try
            {
                context.UserAccounts.Remove(userAccount);

                var deleteCount = await context.SaveChangesAsync();

                return deleteCount > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserAccount?> Get(Guid userId)
        {
            try
            {
                return await context.UserAccounts.FirstOrDefaultAsync(x => x.UserId == userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IfExist(Guid userId)
        {
            try
            {
                return await context.UserAccounts.AnyAsync(x => x.UserId == userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<bool> IfExist(string userName)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserAccount> Queryable()
        {
            try
            {
                return context.UserAccounts.AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserAccount> Update(UserAccount userAccount)
        {
            try
            {
                context.UserAccounts.Update(userAccount);
                await context.SaveChangesAsync();

                return userAccount;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
