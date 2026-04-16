using Microsoft.Extensions.Configuration;
using YoutubeClone.Application.Helpers;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Models.Helpers;
using YoutubeClone.Application.Models.Requests.Auth;
using YoutubeClone.Application.Models.Responses;
using YoutubeClone.Application.Models.Responses.Auth;
using YoutubeClone.Domain.Exceptions;
using YoutubeClone.Domain.Interfaces.Repositories;
using YoutubeClone.Shared;
using YoutubeClone.Shared.Constants;

namespace YoutubeClone.Application.Services
{
    public class AuthService(IUserRepository userRepository, IConfiguration configuration, ICacheService cacheService) : IAuthServices
    {
        public async Task<GenericResponse<LoginAuthResponse>> Login(LoginAuthRequest model)
        {
            var userAccount = await userRepository.Get(model.Email)
                ?? throw new BadRequestException(ResponseConstants.AUTH_USER_OR_PASSWORD_NOT_FOUND);

            var validatePassword = Hasher.ComparePassword(model.Password, userAccount.Password);
            if (!validatePassword)
            {
                throw new BadRequestException(ResponseConstants.AUTH_USER_OR_PASSWORD_NOT_FOUND);
            }

            var token = TokenHelper.Create(userAccount.UserId, configuration, cacheService);
            var refreshToken = TokenHelper.CreateRefresh(userAccount.UserId, configuration, cacheService);

            return ResponsesHelper.Create(new LoginAuthResponse
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        public async Task<GenericResponse<LoginAuthResponse>> Renew(RenewAuthRequest model)
        {
            var findRefreshToken = cacheService.Get<RefreshToken>(CacheHelper.AuthRefreshTokenKey(model.RefreshToken))
                ?? throw new NotFoundException(ResponseConstants.AUTH_REFRESH_TOKEN_NOT_FOUND);

            var token = TokenHelper.Create(findRefreshToken.UserId, configuration, cacheService);
            var refreshToken = TokenHelper.CreateRefresh(findRefreshToken.UserId, configuration, cacheService);

            cacheService.Delete(CacheHelper.AuthRefreshTokenKey(model.RefreshToken));

            return ResponsesHelper.Create(new LoginAuthResponse
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }
    }
}
