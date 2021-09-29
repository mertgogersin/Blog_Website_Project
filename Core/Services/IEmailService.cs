using Core.Repositories;
using System.Threading.Tasks;


namespace Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest mailRequest);
    }
}
