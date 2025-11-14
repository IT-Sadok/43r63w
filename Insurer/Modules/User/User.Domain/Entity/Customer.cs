using Microsoft.AspNetCore.Identity;
using User.Domain.ValueObject;

namespace User.Domain.Entity;

public class Customer
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public Address Address { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<int> PolicyIds { get; set; } = [];
}