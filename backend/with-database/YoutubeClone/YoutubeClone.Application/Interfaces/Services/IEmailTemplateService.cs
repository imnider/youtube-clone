using YoutubeClone.Application.Models.DTOs;

namespace YoutubeClone.Application.Interfaces.Services
{
    public interface IEmailTemplateService
    {
        Task<EmailTemplateDto> Get(string name, Dictionary<string, string> variables);
        Task Init();
    }
}
