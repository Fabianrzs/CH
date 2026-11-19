using GA2.Domain.Core.Dodo.Notification.Models;
using GA2.Transversals.Commons;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Notification.Services
{
    public interface IDodoNotificationService
    {
        Task<Response<SendEmailResponse>> SendEmailAsync(
            SendEmailRequest request,
            CancellationToken cancellationToken = default);
    }
}
