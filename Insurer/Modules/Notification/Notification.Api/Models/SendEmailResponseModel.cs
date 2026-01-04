namespace Notification.Api.Models;

public sealed record SendEmailResponseModel(bool IsSuccess, string? Message = null);