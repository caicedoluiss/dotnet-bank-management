using System;

namespace BankManagement.Domain;

public class Transaction : IRepoEntity
{
  public DateTime Date { get; set; }
  public TransactionType Type => Value == 0 ? TransactionType.Check : Value < 0 ? TransactionType.Debit : TransactionType.Credit;
  public decimal Value { get; set; }
  public decimal Balance { get; set; }
  public int AccountId { get; set; }
  public Account? Account { get; set; }

  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
