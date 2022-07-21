using System;

namespace BankManagement.Application;

public class ExistentTransactionDTO
{
  public DateTime Date { get; set; }
  public string? Type { get; set; }
  public decimal Value { get; set; }
  public decimal Balance { get; set; }
  public int AccountId { get; set; }
  public ExistentAccountDTO? Account { get; set; }

  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
