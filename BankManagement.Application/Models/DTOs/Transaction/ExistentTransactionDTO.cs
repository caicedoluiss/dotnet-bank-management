using System;
using BankManagement.Domain;

namespace BankManagement.Application;

public class ExistentTransactionDTO
{
  public string Date { get; set; } = string.Empty;
  public string Type => Value == 0 ? TransactionType.Check.ToString() : Value < 0 ? TransactionType.Debit.ToString() : TransactionType.Credit.ToString();
  public decimal Value { get; set; }
  public decimal Balance { get; set; }
  public int AccountId { get; set; }
  public ExistentAccountDTO? Account { get; set; }

  public int Id { get; set; }
  public string CreatedAt { get; set; } = string.Empty;
  public string UpdatedAt { get; set; } = string.Empty;
}
