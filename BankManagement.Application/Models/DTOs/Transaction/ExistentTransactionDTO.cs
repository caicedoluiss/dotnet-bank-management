using System;
using BankManagement.Domain;

namespace BankManagement.Application;

public class ExistentTransactionDTO
{
  public DateTime Date { get; set; }
  public string Type => Value == 0 ? TransactionType.Check.ToString() : Value < 0 ? TransactionType.Debit.ToString() : TransactionType.Credit.ToString();
  public decimal Value { get; set; }
  public decimal Balance { get; set; }
  public int AccountId { get; set; }
  public ExistentAccountDTO? Account { get; set; }

  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
