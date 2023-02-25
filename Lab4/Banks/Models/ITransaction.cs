namespace Banks.Models;

public interface ITransaction
{
    TransactionNumber Id { get; }
    AccountNumber? SrcAccountNumber { get; }
    AccountNumber? DstAccountNumber { get; }
    string? SrcBankName { get; }
    string? DstBankName { get; }
    decimal Sum { get; }
    string ToString();
}