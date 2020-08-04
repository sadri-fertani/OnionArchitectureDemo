using System.Threading.Tasks;

namespace Application.Interfaces.Shared
{
    public interface IEmailService
    {
        Task Send(string toAdresse, string toUsername, string messageHTML);
    }
}
