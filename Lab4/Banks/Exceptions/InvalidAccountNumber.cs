using Banks.Models;
namespace Banks.Exceptions;

public class InvalidAccountNumber : Exception
{
    public InvalidAccountNumber(string number, uint numberLen)
    : base($"Account number {number} is invalid: len must be {numberLen}!") { }
}