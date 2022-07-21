namespace BankManagement.Application;

public class UpdatingTransactionDTO
{
  public string? Date { get; set; }
  public string? Type { get; set; }
  public decimal Value { get; set; }
  public decimal Balance { get; set; }
  public int AccountId { get; set; }
}
