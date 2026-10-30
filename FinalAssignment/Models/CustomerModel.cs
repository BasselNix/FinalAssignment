namespace ParaBank.Models;

public class CustomerModel
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public AddressModel Address { get; set; } = new AddressModel();
    public required string PhoneNumber { get; set; }
    public required string Ssn { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}