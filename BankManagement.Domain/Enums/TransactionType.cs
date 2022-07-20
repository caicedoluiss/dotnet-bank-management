namespace BankManagement.Domain;

public enum TransactionType : byte
{
  Check = 0,
  Debit = 1,
  Credit = 2,
  Transference = 3
}
