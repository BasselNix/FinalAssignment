namespace ParaBank.Models;

public enum TransactionType
{
    Credit,
    Debit
}

public class TransactionModel
{
    public required string Id { get; set; }
    public required string Date { get; set; }
    public required string Description { get; set; }
    public required TransactionType Type { get; set; }
    public required string Amount { get; set; }
}