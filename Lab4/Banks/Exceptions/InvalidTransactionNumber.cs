using Banks.Models;
namespace Banks.Exceptions;

public class InvalidTransactionNumber : Exception
{
    public InvalidTransactionNumber(string number, uint numberLen)
    : base($"Transaction number {number} is invalid: len must be {numberLen}!") { }
}