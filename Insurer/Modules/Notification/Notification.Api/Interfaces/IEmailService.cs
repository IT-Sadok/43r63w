using Notification.Api.Models;

namespace Notification.Api.Interfaces;

public interface IEmailService
{
    public Task SendEmailAsync(
        SendEmailModel model,
        CancellationToken cancellationToken = default);
}