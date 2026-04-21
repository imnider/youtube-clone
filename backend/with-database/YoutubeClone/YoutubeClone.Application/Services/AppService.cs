using Microsoft.Extensions.Configuration;
using YoutubeClone.Application.Helpers;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Models.DTOs;
using YoutubeClone.Application.Models.Responses;
using YoutubeClone.Domain.Database;
using YoutubeClone.Domain.Database.SqlServer.Entities;
using YoutubeClone.Shared.Constants;

namespace YoutubeClone.Application.Services
{
    public class AppService(IConfiguration configuration, IUnitOfWork uow) : IAppService
    {
        public async Task<GenericResponse<AppInfoDto>> Info()
        {
            return ResponseHelper.Create(new AppInfoDto
            {
                Version = configuration[ConfigurationConstants.VERSION] ?? "0.0.0",
                Roles = [.. uow.roleRepository.Queryable().Where(x => x.IsActive).ToList().Select(r => MapRole(r))]
            });
        }

        private RoleDto MapRole(Role role)
        {
            return new RoleDto
            {
                Id = role.RoleId,
                Name = role.Name,
                Description = role.Description,
            };
        }
    }
}
