namespace ParaBank.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "Tester";
        public string LastName { get; set; } = "Testeroo";
        public AddressModel Address { get; set; } = new AddressModel();
        public string PhoneNumber { get; set; } = "555-0199";
        public string Ssn { get; set; } = "000-00-0000";
        public string Username { get; set; } = $"user_{Guid.NewGuid().ToString()[..8]}";
        public string Password { get; set; } = "Password123!";
    }

    public class AddressModel
    {
        public string Street { get; set; } = "123 Main St";
        public string City { get; set; } = "Metropolis";
        public string State { get; set; } = "NY";
        public string ZipCode { get; set; } = "10001";
    }
}