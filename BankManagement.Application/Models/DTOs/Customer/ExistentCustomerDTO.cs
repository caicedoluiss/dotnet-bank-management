using System;

namespace BankManagement.Application;

public class ExistentCustomerDTO
{
  public string IdNumber { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string? Gender { get; set; }
  public int Age { get; set; }
  public string PhoneNumber { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public bool State { get; set; }

  public int Id { get; set; }
  public string CreatedAt { get; set; } = string.Empty;
  public string UpdatedAt { get; set; } = string.Empty;
}
