using System;

using Banks.Entities;
using Banks.Events;
using Banks.Exceptions;
using Banks.Models;

namespace A
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var centralBank = CentralBank.GetInstance();
            SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
            depositPercents.Add(200, 2);
            depositPercents.Add(4000, 3);
            depositPercents.Add(10000, 5);

            centralBank.AddBank("Sber", 2, 2, depositPercents, 500);

            var bank = centralBank.GetBankByName("Sber");
            bank.AddClient("k.kryukov");
            var debit1 = bank.CreateDebitAccount("k.kryukov");
            var transTopUp = bank.TopUpAccount("k.kryukov", new AccountNumber(debit1.ToString()), 200);
            return;

            // var transGetCash = bank.GetCash("k.kryukov", debit1, 100);
            // var transTransfer = bank.TransferBetweenAccounts("k.kryukov", debit1, "d.ravdonikas", debit2, 50);

            // foreach (var trans in CentralBank.GetInstance().TransactionHistory)
            //     Console.WriteLine(trans);

            // bank.DisplayInfo();
            // CentralBank.GetInstance().CancelTransaction(transTopUp);
            // bank.DisplayInfo();
            // CentralBank.GetInstance().CancelTransaction(transGetCash);
            // bank.DisplayInfo();
            // CentralBank.GetInstance().CancelTransaction(transTransfer);
            // bank.DisplayInfo();

            // bank.AddClient("k.kryukov");
            // var debit = bank.CreateDebitAccount("k.kryukov");
            // bank.SpecifyClientPersonalData("k.kryukov", new PassportData("1234", "567890"), "Pushkina 1");
            // bank.AddClient("d.ravdonikas");
            // bank.SpecifyClientPersonalData("d.ravdonikas", new PassportData("123434", "567890"), "Pushkina 2");
            // var credit = bank.CreateCreditAccount("d.ravdonikas");
            // bank.TopUpAccount("k.kryukov", debit, 1000);
            // bank.TopUpAccount("d.ravdonikas", credit, 1000);

            // var transNumber = bank.TransferBetweenAccounts("d.ravdonikas", credit, "k.kryukov", debit, 700);
            // Time.MoveTimeForward(365);

            // var raif = centralBank.AddBank("Raiffeisen Bank", 2, 3, depositPercents, 500);
            // raif.AddClient("t.kryukova");
            // var debit_raif = raif.CreateDebitAccount("t.kryukova");
            // raif.SpecifyClientPersonalData("t.kryukova", new PassportData("123434", "567890"), "Pushkina 3");
            // raif.TopUpAccount("t.kryukova", debit_raif, 1000);

            // centralBank.DisplayInfo();
            // centralBank.TransferBetweenBanks("Raiffeisen Bank", "Sber", debit_raif, debit, 100);
            // centralBank.TransferBetweenBanks("Raiffeisen Bank", "Sber", debit_raif, debit, 100);
            // centralBank.TransferBetweenBanks("Sber", "Raiffeisen Bank", debit, debit_raif, 100);
            // centralBank.DisplayInfo();

            // // foreach (var trans in CentralBank.GetInstance().TransactionHistory)
            // //     Console.WriteLine(trans);
            // CentralBank.GetInstance().DisplayInfo();
            // CentralBank.GetInstance().CancelTransaction(transNumber);
            // CentralBank.GetInstance().DisplayInfo();
        }
    }
}