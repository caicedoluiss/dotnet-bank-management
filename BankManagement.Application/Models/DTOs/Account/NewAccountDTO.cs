namespace BankManagement.Application;

public class NewAccountDTO
{
  public string Number { get; set; } = string.Empty;
  public string? Type { get; set; }
  public decimal Balance { get; set; }
  public bool State { get; set; }
  public int CustomerId { get; set; }
}
