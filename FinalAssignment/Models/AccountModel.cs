namespace ParaBank.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Type { get; set; } = "CHECKING";
        public decimal Balance { get; set; }
    }
}