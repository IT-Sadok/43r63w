using Notification.Api.Models;

namespace Notification.Api.Interfaces;

public interface IEmailSender
{
    public Task<SendEmailResponseModel> SendEmailAsync(
        SendEmailModel model,
        CancellationToken cancellationToken = default);
}