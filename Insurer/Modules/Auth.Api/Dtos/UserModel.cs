namespace Auth.Api.Dtos;

public sealed class UserModel
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = null!;
}

