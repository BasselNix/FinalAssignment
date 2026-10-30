namespace ParaBank.Models;

public enum AccountType
{
    CHECKING,
    SAVINGS,
    LOAN
}

public class AccountModel
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public AccountType Type { get; set; } = AccountType.CHECKING;
    public decimal Balance { get; set; }
}