namespace Notification.Api.Messaging;

public class RabbitMqQueue
{
    public string PolicyCreated { get; set; } = null!;

    public string PolicyCreatedRetry { get; set; } = null!;

    public string PolicyCreatedDlq { get; set; } = null!;

    public string PolicyUpdated { get; set; } = null!;

    public int MaxRetryCount { get; set; }
}