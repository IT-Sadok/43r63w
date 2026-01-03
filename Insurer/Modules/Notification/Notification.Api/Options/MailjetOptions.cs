namespace Notification.Api.Options;

public sealed class MailjetOptions
{
    public string PublicKey { get; set; } = null!;

    public string PrivateKey { get; set; } = null!;
}
