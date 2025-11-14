namespace User.Application.Models;

public sealed class AddressModel
{
    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string? ZipCode { get; set; } = null!;
}