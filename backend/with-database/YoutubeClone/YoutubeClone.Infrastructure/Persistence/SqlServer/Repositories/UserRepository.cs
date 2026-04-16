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

                return userAccount;
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
                return await context.UserAccounts.FirstOrDefaultAsync(x => x.UserId == userId && x.DeletedAt == null);
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
                return await context.UserAccounts.FirstOrDefaultAsync(x => x.Email == email && x.DeletedAt == null);
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
                return context.UserAccounts.Where(x => x.DeletedAt == null).AsQueryable();
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

                return userAccount;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
