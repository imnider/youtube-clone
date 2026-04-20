using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YoutubeClone.Domain.Database.SqlServer.Context;

namespace YoutubeClone.Infrastructure.Persistence.SqlServer.Repositories
{
    public class GenericRepository<T>(YoutubeCloneContext context) where T : class
    {
        public async Task<T> Create(T entity)
        {
            context.Set<T>().Add(entity);
            return entity;
        }

        public async Task<bool> Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            return true;
        }

        public IQueryable<T> Queryable()
        {
            return context.Set<T>().AsQueryable();
        }

        public async Task<T> Update(T entity)
        {
            context.Set<T>().Update(entity);
            return entity;
        }

        public async Task<bool> IfExists(Expression<Func<T, bool>> expression)
        {
            return await context.Set<T>().AnyAsync(expression);
        }

        public async Task<T?> Get(Expression<Func<T, bool>> expression)
        {
            return await context.Set<T>().FirstOrDefaultAsync(expression);
        }
    }
}
