namespace Notification.Api.Models;

public sealed record SendEmailModel(string To, string Body, string Title);
