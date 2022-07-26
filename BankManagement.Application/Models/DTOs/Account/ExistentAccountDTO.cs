using System;

namespace BankManagement.Application;

public class ExistentAccountDTO
{
  public string Number { get; set; } = string.Empty;
  public string? Type { get; set; }
  public decimal Balance { get; set; }
  public bool State { get; set; }
  public int CustomerId { get; set; }
  public ExistentCustomerDTO? Customer { get; set; }

  public int Id { get; set; }
  public string CreatedAt { get; set; } = string.Empty;
  public string UpdatedAt { get; set; } = string.Empty;
}
