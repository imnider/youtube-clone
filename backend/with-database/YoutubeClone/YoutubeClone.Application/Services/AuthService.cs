using Microsoft.Extensions.Configuration;
using YoutubeClone.Application.Helpers;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Models.Helpers;
using YoutubeClone.Application.Models.Requests.Auth;
using YoutubeClone.Application.Models.Responses;
using YoutubeClone.Application.Models.Responses.Auth;
using YoutubeClone.Domain.Database;
using YoutubeClone.Domain.Exceptions;
using YoutubeClone.Shared;
using YoutubeClone.Shared.Constants;
using YoutubeClone.Shared.Helpers;

namespace YoutubeClone.Application.Services
{
    public class AuthService(
        IUnitOfWork uow,
        IConfiguration configuration,
        ICacheService cacheService,
        IEmailTemplateService emailTemplateService,
        SMTP smtp,
        IUserService userService)
        : IAuthServices
    {
        public async Task<GenericResponse<LoginAuthResponse>> Login(LoginAuthRequest model)
        {
            var userAccount = await uow.userRepository.Get(model.Email)
                ?? throw new BadRequestException(ResponseConstants.AUTH_USER_OR_PASSWORD_NOT_FOUND);

            var validatePassword = Hasher.ComparePassword(model.Password, userAccount.Password);
            if (!validatePassword)
            {
                var templateFailed = await emailTemplateService.Get(EmailTemplateNameConstants.AUTH_LOGIN_FAILED, []);
                await smtp.Send(model.Email, templateFailed.Subject, templateFailed.Body);
                throw new BadRequestException(ResponseConstants.AUTH_USER_OR_PASSWORD_NOT_FOUND);
            }

            var token = TokenHelper.Create(userAccount.UserId, [.. userAccount.UserAccountRoles.Select(x => x.Role.Name)], configuration, cacheService);
            var refreshToken = TokenHelper.CreateRefresh(userAccount.UserId, configuration, cacheService);

            var templateSuccess = await emailTemplateService.Get(EmailTemplateNameConstants.AUTH_LOGIN_SUCCESS, new Dictionary<string, string>
            {
                { "datetime", DateTimeHelper.UtcNow().ToString() }
            });
            await smtp.Send(model.Email, templateSuccess.Subject, templateSuccess.Body);

            return ResponseHelper.Create(new LoginAuthResponse
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        public async Task<GenericResponse<LoginAuthResponse>> Renew(RenewAuthRequest model)
        {
            var findRefreshToken = cacheService.Get<RefreshToken>(CacheHelper.AuthRefreshTokenKey(model.RefreshToken))
                ?? throw new NotFoundException(ResponseConstants.AUTH_REFRESH_TOKEN_NOT_FOUND);

            var userAccount = await uow.userRepository.Get(findRefreshToken.UserId)
                ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST);

            var token = TokenHelper.Create(findRefreshToken.UserId, [.. userAccount.UserAccountRoles.Select(x => x.Role.Name)], configuration, cacheService);
            var refreshToken = TokenHelper.CreateRefresh(findRefreshToken.UserId, configuration, cacheService);

            cacheService.Delete(CacheHelper.AuthRefreshTokenKey(model.RefreshToken));

            return ResponseHelper.Create(new LoginAuthResponse
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }
    }
}
