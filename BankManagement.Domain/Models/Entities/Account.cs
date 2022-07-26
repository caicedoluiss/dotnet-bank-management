using System;

namespace BankManagement.Domain;

public class Account : IRepoEntity
{
  public string Number { get; set; } = string.Empty;
  public AccountType Type { get; set; }
  public decimal Balance { get; set; }
  public bool State { get; set; }
  public int CustomerId { get; set; }
  public Customer? Customer { get; set; }

  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
