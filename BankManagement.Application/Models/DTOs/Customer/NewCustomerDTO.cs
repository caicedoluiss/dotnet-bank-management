namespace BankManagement.Application;

public class NewCustomerDTO
{
  public string IdNumber { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string? Gender { get; set; }
  public int Age { get; set; }
  public string PhoneNumber { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public bool State { get; set; }
}
