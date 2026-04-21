using Microsoft.EntityFrameworkCore;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Database.SqlServer.Entities;
using YoutubeClone.Domain.Interfaces.Repositories;

namespace YoutubeClone.Infrastructure.Persistence.SqlServer.Repositories
{
    public class RoleRepository(YoutubeCloneContext context) : GenericRepository<Role>(context), IRoleRepository
    {
        public async Task<Role?> Get(Guid id)
        {
            return await context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
        }
    }
}
