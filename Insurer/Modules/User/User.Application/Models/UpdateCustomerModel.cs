namespace User.Application.Models;

public sealed class UpdateCustomerModel
{
    public int Id { get; set; }
    
    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    public AddressModel? Address { get; set; }
}