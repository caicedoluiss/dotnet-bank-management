using System;

namespace BankManagement.Domain;

public class Customer : Person, IRepoEntity
{
  public string PhoneNumber { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public bool State { get; set; }

  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
