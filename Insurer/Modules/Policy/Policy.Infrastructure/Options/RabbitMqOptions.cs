namespace Policy.Infrastructure.Options;

public class RabbitMqOptions
{
    public int Port { get; set; }

    public string Host { get; set; } = null!;

    public string VirtualHost { get; set; } = "/";

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string RetryCount { get; set; } = null!;
}